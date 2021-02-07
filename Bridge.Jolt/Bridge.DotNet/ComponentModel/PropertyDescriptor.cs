using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
    /// <summary>
    /// Provides an abstraction of a property on a class.
    /// </summary>
    public abstract class PropertyDescriptor : MemberDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='PropertyDescriptor'/> class with the specified name and attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attrs"></param>
        protected PropertyDescriptor(string name, Attribute[] attrs)
            : base(name, attrs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="PropertyDescriptor"/> class with the name and attributes in the specified <see cref="MemberDescriptor"/>.
        /// </summary>
        /// <param name="descr">A <see cref="MemberDescriptor"/> containing the name of the member and its attributes.</param>
        protected PropertyDescriptor(MemberDescriptor descr): base(descr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDescriptor"/> class with the name in the specified <see cref="MemberDescriptor"/> and the attributes in both the<see cref="MemberDescriptor"/> and the <see cref="Attribute"/> array.
        /// </summary>
        /// <param name="descr">A <see cref="MemberDescriptor"/> containing the name of the member and its attributes.</param>
        /// <param name="attrs">An<see cref="Attribute"/> array containing the attributes you want to associate with the property.</param>
        protected PropertyDescriptor(MemberDescriptor descr, Attribute[] attrs): base(descr, attrs)
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <value>A <see cref="Type"/> that represents the type of component this property is bound to. When the <see cref="GetValue(Object)"/> or <see cref="SetValue(Object, Object)"/> methods are invoked, the object specified might be an instance of this type.</value>
        public abstract Type ComponentType { get; }

        /// <summary>
        /// Gets a value indicating whether this property should be localized, as specified in the <see cref="LocalizableAttribute"/>.
        /// </summary>
        /// <value>Always returns false for the moment.</value>
        public virtual bool IsLocalizable => false;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value>true if the property is read-only; otherwise, false.</value>
        public abstract bool IsReadOnly { get; }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value>A <see cref="Type"/> that represents the type of the property.</value>
        /// <remarks>Typically, this property is implemented through reflection.</remarks>
        public abstract Type PropertyType { get; }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>true if resetting the component changes its value; otherwise, false.</returns>
        public abstract bool CanResetValue(object component);

        /// <summary>
        /// Compares this to another object to see if they are equivalent.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The object to compare to this <see cref="PropertyDescriptor"/>.</returns>
        public override bool Equals(object obj)
        {
            try
            {
                if (obj == this)
                {
                    return true;
                }

                if (obj == null)
                {
                    return false;
                }

                // Assume that 90% of the time we will only do a .Equals(...) for
                // propertydescriptor vs. propertydescriptor... avoid the overhead
                // of an instanceof call.

                if (obj is PropertyDescriptor pd && pd.NameHashCode == this.NameHashCode
                    && pd.PropertyType == this.PropertyType
                    && pd.Name.Equals(this.Name))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Adds the attributes of the PropertyDescriptor to the specified list of attributes in the parent class.
        /// </summary>
        /// <param name="attributeList">An <see cref="IList"/> that lists the attributes in the parent class. Initially, this is empty.</param>
        /// <remarks>For duplicate attributes, the last one added to the list will be kept.</remarks>
        protected override void FillAttributes(IList attributeList)
        {
            base.FillAttributes(attributeList);
        }

        /// <summary>
        /// Returns the hash code for this object.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return this.NameHashCode ^ this.PropertyType.GetHashCode();
        }

        /// <summary>
        /// This method returns the object that should be used during invocation of members.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the invocation target.</param>
        /// <param name="instance">The potential invocation target.</param>
        /// <returns>The <see cref="Object"/> that should be used during invocation of members.</returns>
        /// <remarks>
        /// Typically, the return value will be the same as the instance passed in. 
        /// If someone associated another object with this instance, or if the instance is a custom type descriptor, the<see cref="GetInvocationTarget"/> method may return a different value.
        /// </remarks>
        protected override object GetInvocationTarget(Type type, object instance)
        {
            object target = base.GetInvocationTarget(type, instance);
            if (target is ICustomTypeDescriptor td)
            {
                target = td.GetPropertyOwner(this);
            }
            return target;
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>The value of a property for a given component.</returns>
        /// <remarks>Typically, this method is implemented through reflection. </remarks>
        public abstract object GetValue(object component);

        /// <summary>
        /// Raises the ValueChanged event that you implemented.
        /// </summary>
        /// <param name="component">The object that raises the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>This method should be called by your property descriptor implementation when the property value has changed.</remarks>
        protected virtual void OnValueChanged(object component, EventArgs e)
        {
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public abstract void ResetValue(object component);

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public abstract void SetValue(object component, object value);

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>true if the property should be persisted; otherwise, false.</returns>
        /// <remarks>Typically, this method is implemented through reflection.</remarks>
        public abstract bool ShouldSerializeValue(object component);

        /// <summary>
        /// Gets a value indicating whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        /// <value>true if value change notifications may originate from outside the property descriptor; otherwise, false.</value>
        /// <remarks>
        /// The SupportsChangeEvents property indicates whether value change notifications for this property may originate from
        /// outside the property descriptor, such as from the component itself, or whether notifications will only originate
        /// from direct calls made to the SetValue method.
        /// For example, the component may implement the INotifyPropertyChanged interface, or may have an explicit
        /// name.Changed event for this property.
        /// </remarks>
        public virtual bool SupportsChangeEvents => false;
    }
}
