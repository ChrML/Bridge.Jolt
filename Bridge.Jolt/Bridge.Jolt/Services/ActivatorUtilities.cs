﻿using System;
using System.Reflection;

namespace Jolt
{
    /// <summary>
    /// Provides utilities to create classes using a service provider to resolve its dependencies.
    /// </summary>
    public static class ActivatorUtilities
    {
        #region Methods

        /// <summary>
        /// Creates a new instance of the class of type <typeparamref name="T"/> by resolving its dependencies from <paramref name="provider"/>.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="provider">The service provider for resolving services.</param>
        /// <param name="values">Additional values that could be provided to the constructor.</param>
        /// <returns>Returns the new instance created.</returns>
        public static T CreateInstance<T>(IServices provider, object values = null) where T : class
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            return (T)CreateInstance(provider, typeof(T), values);
        }

        /// <summary>
        /// Creates a new instance of the class of type <paramref name="type"/> by resolving its dependencies from <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The service provider for resolving services.</param>
        /// <param name="type">The type to create.</param>
        /// <param name="values">Additional values that could be used for the constructor.</param>
        /// <returns>Returns the new instance created.</returns>
        public static object CreateInstance(IServices provider, Type type, object values = null)
        {
            // Check sanity.
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Get the constructor.
            ConstructorInfo defaultCtor = null;
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors == null || constructors.Length == 0)
            {
                throw new InvalidOperationException($"Unable to create {type.FullName}. Make sure this class is Bridge.NET reflectable, metadata is included and has a public constructor.");
            }
            else if (constructors.Length == 1)
            {
                defaultCtor = constructors[0];
            }
            else
            {
                for (int i = 0; i < constructors.Length; i++)
                {
                    if (constructors[i].ParameterTypes?.Length == 0)
                    {
                        defaultCtor = constructors[i];
                    }
                }

                if (defaultCtor == null)
                {
                    throw new InvalidOperationException($"Unable to create {type.FullName}. More than 1 constructor was found for this type, but no default constructor.");
                }
            }

            // Create the instance.
            object[] parameters = ConstructorArgumentResolver.Resolve(type, defaultCtor, provider, values);
            return defaultCtor.Invoke(parameters);
        }

        #endregion
    }
}
