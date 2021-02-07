using System.Reflection;

namespace System.ComponentModel
{
    /// <summary>
    /// Implements <see cref="PropertyDescriptor"/> using reflection on a native .NET type.
    /// </summary>
    class ReflectPropertyDescriptor: PropertyDescriptor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectPropertyDescriptor"/> class using the given component data.
        /// </summary>
        /// <param name="componentClass"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        public ReflectPropertyDescriptor(Type componentClass, string name, Type type, Attribute[] attributes): base(name, attributes)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.componentClass = componentClass ?? throw new ArgumentNullException(nameof(componentClass));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectPropertyDescriptor"/> class using the given component data.
        /// </summary>
        /// <param name="componentClass"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="propInfo"></param>
        /// <param name="getMethod"></param>
        /// <param name="setMethod"></param>
        /// <param name="attrs"></param>
        public ReflectPropertyDescriptor(Type componentClass, string name, Type type, PropertyInfo propInfo, MethodInfo getMethod, MethodInfo setMethod, Attribute[] attrs) : this(componentClass, name, type, attrs)
        {
            this._PropInfo = propInfo;
            this.propInfoQueried = propInfo != null;

            this.getMethod = getMethod;
            this.getValueQueried = getMethod != null;

            this.setMethod = setMethod;
            this.setMethodQueried = setMethod != null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the component this <see cref="PropertyDescriptor"/> is bound to.
        /// </summary>
        public override Type ComponentType => this.componentClass;

        /// <summary>
        /// Gets whether this property is read only.
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                if (this.SetMethodValue != null)
                {
                    return ((ReadOnlyAttribute)this.Attributes[typeof(ReadOnlyAttribute)])?.IsReadOnly ?? false;
                }
                else
                {
                    return true;
                }  
            }
        }
        
        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public override Type PropertyType => this.type;

        #endregion

        #region Methods

        /// <summary>
        /// Gets if the value can be reset.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool CanResetValue(object component)
        {
            // Read-only properties cannot be reset.
            if (this.IsReadOnly)
                return false;

            // If we got a default value attribute, then we can reset the value if the value is not already the default value.
            if (this.DefaultValue != noValue)
            {
                return !Equals(this.GetValue(component), this.DefaultValue);
            }

            return false;
        }

        /// <summary>
        /// Get the value on the component.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override object GetValue(object component)
        {
            if (component != null)
            {
                component = this.GetInvocationTarget(this.componentClass, component);
                return this.GetMethodValue.Invoke(component, null);
            }
            return null;
        }

        /// <summary>
        /// Resets the value on the component back to its default value.
        /// </summary>
        /// <param name="component"></param>
        public override void ResetValue(object component)
        {
            if (this.DefaultValue != noValue)
            {
                this.SetValue(component, this.DefaultValue);
            }
        }

        /// <summary>
        /// Set the value on the component.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="value"></param>
        public override void SetValue(object component, object value)
        {
            if (component != null)
            {
                object invokee = this.GetInvocationTarget(this.componentClass, component);
                if (!this.IsReadOnly)
                {
                    dynamic dyn = invokee;

                    // Invoke the set method.
                    this.SetMethodValue.Invoke(dyn, value);
                    this.OnValueChanged(invokee, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets if the value should be serialized for persistence.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool ShouldSerializeValue(object component)
        {
            // Not implemented.
            return true;
        }

        #endregion

        #region Privates

        /// <summary>
        /// This should be called by your property descriptor implementation when the property value has changed.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="e"></param>
        protected override void OnValueChanged(object component, EventArgs e)
        {
            base.OnValueChanged(component, e);
        }
        
        /// <summary>
        /// Gets the default value of this property according to the <see cref="DefaultValueAttribute"/>.
        /// </summary>
        object DefaultValue
        {
            get
            {
                // Return the default value already queried if we have.
                if (this.defaultValueQueried)
                    return this.defaultValue;

                // Query it now.
                this.defaultValueQueried = true;
                Attribute a = this.Attributes[typeof(DefaultValueAttribute)];
                if (a != null)
                {
                    this.defaultValue = ((DefaultValueAttribute)a).Value;
                }
                else
                {
                    this.defaultValue = noValue;
                }
                return this.defaultValue;
            }
        }
        object defaultValue = null;
        bool defaultValueQueried = false;

        /// <summary>
        /// Gets the GET- method for this property.
        /// </summary>
        MethodInfo GetMethodValue
        {
            get
            {
                // Return the default value already queried if we have.
                if (this.getValueQueried)
                    return this.getMethod;

                // Query it now.
                this.getValueQueried = true;
                this.getMethod = this.PropInfo?.GetMethod;
                if (this.getMethod == null)
                {
                    throw new InvalidOperationException($"Property {this.Name} has no Get- accessor.");
                }
                // NOTE: Original code also looks for GetXXX methods.
                return this.getMethod;
            }
        }
        bool getValueQueried = false;
        MethodInfo getMethod = null;

        /// <summary>
        /// Gets the SET- method for this property.
        /// </summary>
        MethodInfo SetMethodValue
        {
            get
            {
                // Get the cached value if already checked.
                if (this.setMethodQueried)
                {
                    return this.setMethod;
                }
                this.setMethodQueried = true;

                // Try to grab it from reflection.
                PropertyInfo propInfo = this.PropInfo;
                if (propInfo != null)
                {
                    // Try to find the method in our own class first.
                    this.setMethod = propInfo.GetSetMethod(true);
                    if (this.setMethod != null)
                    {
                        return this.setMethod;
                    }

                    // Iterate through the base-types to find the closest set- accessor available.
                    BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                    string name = propInfo.Name;
                    for (Type t = this.ComponentType.BaseType; t != null && t != typeof(object); t = t.BaseType)
                    {
                        if (t == null)
                            break;

                        // Try to grab the set method of this type.
                        PropertyInfo p = t.GetProperty(name, bindingFlags);
                        if (p != null)
                        {
                            this.setMethod = p.SetMethod;
                            if (this.setMethod != null)
                                break;
                        }
                    }
                }
                return this.setMethod;
            }
        }
        MethodInfo setMethod = null;
        bool setMethodQueried = false;

        /// <summary>
        /// Gets the property info for this method.
        /// </summary>
        PropertyInfo PropInfo
        {
            get
            {
                // Get the value if already queried.
                if (this.propInfoQueried)
                    return this._PropInfo;

                // Query now if first access.
                this.propInfoQueried = true;
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;
                this._PropInfo = this.componentClass.GetProperty(this.Name, bindingFlags);
                return this._PropInfo;
            }
        }
        bool propInfoQueried = false;
        PropertyInfo _PropInfo = null;


        static readonly object noValue = new object();
        readonly Type componentClass;
        readonly Type type;

        #endregion
    }
}
