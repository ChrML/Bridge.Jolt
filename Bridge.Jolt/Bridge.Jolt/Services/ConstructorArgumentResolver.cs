using System;
using System.ComponentModel;
using System.Reflection;

namespace Jolt
{
    /// <summary>
    /// Provides helpers to be able to resolve constructor parameters for dependency-injection.
    /// </summary>
    static class ConstructorArgumentResolver
    {
        /// <summary>
        /// Recursively resolves the dependencies of the constructor.
        /// </summary>
        public static object[] Resolve(Type type, ConstructorInfo constructor, IServices services, object additional)
        {
            // Gets additional properties for injection if available.
            PropertyDescriptorCollection additionalProperties = null;
            if (additional != null)
            {
                additionalProperties = TypeDescriptor.GetProperties(additional);
            }

            // Try to resolve each constructor parameter by looking at the additional values first, then services after.
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parameterValues = new object[parameters.Length];
            for (int i = 0; i < parameterValues.Length; i++)
            {
                ParameterInfo param = parameters[i];
                if (TryResolveFromObjectManaged(param, additionalProperties, additional, out object value))
                {
                    parameterValues[i] = value;
                }
                else if (TryResolveFromObjectPlain(param, additional, out value))
                {
                    parameterValues[i] = value;
                }
                else if (TryResolveService(param, services, out value))
                {
                    parameterValues[i] = value;
                }
                else if (param.IsOptional)
                {
                    parameterValues[i] = null;
                }
                else
                { 
                    ThrowNotFound(type, param, additional != null);
                }
            }

            return parameterValues;
        }

        /// <summary>
        /// Tries to resolve a constructor parameter by looking at the additional object as a managed class.
        /// </summary>
        static bool TryResolveFromObjectManaged(ParameterInfo parameter, PropertyDescriptorCollection properties, object values, out object result)
        {
            if (properties == null || values == null)
            {
                result = null;
                return false;

            }

            PropertyDescriptor prop = properties.Find(parameter.Name, ignoreCase: true);
            if (prop != null)
            {
                result = prop.GetValue(values);

                Type resultType = result?.GetType();
                if (resultType != null)
                {
                    if (!parameter.ParameterType.IsAssignableFrom(prop.PropertyType))
                    {
                        throw new InvalidOperationException($"Value was provided for paramter {parameter.Name}, but the value was of type {resultType.FullName} which is incompatible with the constructor argument of type {parameter.ParameterType.FullName}.");
                    }
                }

                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to resolve a constructor parameter by looking at the additional object as a plain JS- object.
        /// </summary>
        static bool TryResolveFromObjectPlain(ParameterInfo parameter, object values, out object result)
        {
            if (values == null)
            {
                result = null;
                return false;
            }

            string paramName = parameter.Name;

            string[] props = GetOwnPropertyNames(values);
            int length = props.Length;
            for (int i = 0; i < length; i++)
            {
                if (String.Equals(paramName, props[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    result = values[props[i]];
                    return true;
                }
            }

            // Not found-
            result = null;
            return false;
        }

        /// <summary>
        /// Tries to resolve a constructor parameter by looking at our available services.
        /// </summary>
        static bool TryResolveService(ParameterInfo parameter, IServices services, out object result)
        {
            Type paramType = parameter.ParameterType;
            object service = services.GetService(paramType);
            if (service != null)
            {
                result = service;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Throws an exception due to a suitable constructor parameter not found.
        /// </summary>
        static void ThrowNotFound(Type type, ParameterInfo param, bool valuesWereProvided)
        {
            if (valuesWereProvided)
            {
                throw new InvalidOperationException($"Unable to create {type.FullName}. The required parameter {param.Name} of type {param.ParameterType.FullName} could not be resolved from the service collection, nor from the object provided with additional values. You could set a default value if the parameter is optional.");
            }
            else
            {
                throw new InvalidOperationException($"Unable to create {type.FullName}. The required parameter {param.Name} of type {param.ParameterType.FullName} could not be resolved from the service collection. You could set a default value if the parameter is optional.");
            }
        }
    }
}
