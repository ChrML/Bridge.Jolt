using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Cache of <see cref="ValidationAttribute"/>s
    /// </summary>
    /// <remarks>
    /// This internal class serves as a cache of validation attributes and [Display] attributes.
    /// It exists both to help performance as well as to abstract away the differences between Reflection and TypeDescriptor.
    /// </remarks>
    internal class ValidationAttributeStore
    {
        readonly Dictionary<Type, TypeStoreItem> _typeStoreItems = new Dictionary<Type, TypeStoreItem>();

        /// <summary>
        /// Gets the singleton <see cref="ValidationAttributeStore"/>
        /// </summary>
        internal static ValidationAttributeStore Instance { get; } = new ValidationAttributeStore();

        /// <summary>
        /// Retrieves the type level validation attributes for the given type.
        /// </summary>
        /// <param name="validationContext">The context that describes the type.  It cannot be null.</param>
        /// <returns>The collection of validation attributes.  It could be empty.</returns>
        internal IEnumerable<ValidationAttribute> GetTypeValidationAttributes(ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            TypeStoreItem item = this.GetTypeStoreItem(validationContext.ObjectType);
            return item.ValidationAttributes;
        }

        /// <summary>
        /// Retrieves the set of validation attributes for the property
        /// </summary>
        /// <param name="validationContext">The context that describes the property.  It cannot be null.</param>
        /// <returns>The collection of validation attributes.  It could be empty.</returns>
        internal IEnumerable<ValidationAttribute> GetPropertyValidationAttributes(ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            TypeStoreItem typeItem = this.GetTypeStoreItem(validationContext.ObjectType);
            PropertyStoreItem item = typeItem.GetPropertyStoreItem(validationContext.MemberName);
            return item.ValidationAttributes;
        }

        /// <summary>
        /// Retrieves the Type of the given property.
        /// </summary>
        /// <param name="validationContext">The context that describes the property.  It cannot be null.</param>
        /// <returns>The type of the specified property</returns>
        internal Type GetPropertyType(ValidationContext validationContext)
        {
            EnsureValidationContext(validationContext);
            TypeStoreItem typeItem = this.GetTypeStoreItem(validationContext.ObjectType);
            PropertyStoreItem item = typeItem.GetPropertyStoreItem(validationContext.MemberName);
            return item.PropertyType;
        }

        /// <summary>
        /// Retrieves or creates the store item for the given type
        /// </summary>
        /// <param name="type">The type whose store item is needed.  It cannot be null</param>
        /// <returns>The type store item.  It will not be null.</returns>
        TypeStoreItem GetTypeStoreItem(Type type)
        {
            // Check sanity.
            if (type == null) throw new ArgumentNullException(nameof(type));

            lock (this._typeStoreItems)
            {
                if (!this._typeStoreItems.TryGetValue(type, out TypeStoreItem item))
                {
                    IEnumerable<Attribute> attributes = TypeDescriptor.GetAttributes(type).OfType<Attribute>();
                    item = new TypeStoreItem(type, attributes);
                    this._typeStoreItems[type] = item;
                }
                return item;
            }
        }

        /// <summary>
        /// Throws an ArgumentException of the validation context is null
        /// </summary>
        /// <param name="validationContext">The context to check</param>
        static void EnsureValidationContext(ValidationContext validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));
        }

        /// <summary>
        /// Private abstract class for all store items
        /// </summary>
        abstract class StoreItem
        {
            internal StoreItem(IEnumerable<Attribute> attributes)
            {
                this.ValidationAttributes = attributes.OfType<ValidationAttribute>();
                this.DisplayAttribute = attributes.OfType<DisplayAttribute>().SingleOrDefault();
            }

            internal IEnumerable<ValidationAttribute> ValidationAttributes { get; }

            internal DisplayAttribute DisplayAttribute { get; set; }
        }

        /// <summary>
        /// Private class to store data associated with a type
        /// </summary>
        class TypeStoreItem : StoreItem
        {
            readonly object _syncRoot = new object();
            readonly Type _type;
            Dictionary<string, PropertyStoreItem> _propertyStoreItems;

            internal TypeStoreItem(Type type, IEnumerable<Attribute> attributes): base(attributes)
            {
                this._type = type;
            }

            internal PropertyStoreItem GetPropertyStoreItem(string propertyName)
            {
                if (!this.TryGetPropertyStoreItem(propertyName, out PropertyStoreItem item))
                {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Unknown property", this._type.Name, propertyName), nameof(propertyName));
                }
                return item;
            }

            internal bool TryGetPropertyStoreItem(string propertyName, out PropertyStoreItem item)
            {
                if (String.IsNullOrEmpty(propertyName))
                {
                    throw new ArgumentNullException(nameof(propertyName));
                }

                if (this._propertyStoreItems == null)
                {
                    lock (this._syncRoot)
                    {
                        if (this._propertyStoreItems == null)
                        {
                            this._propertyStoreItems = this.CreatePropertyStoreItems();
                        }
                    }
                }
                if (!this._propertyStoreItems.TryGetValue(propertyName, out item))
                {
                    return false;
                }
                return true;
            }

            Dictionary<string, PropertyStoreItem> CreatePropertyStoreItems()
            {
                Dictionary<string, PropertyStoreItem> propertyStoreItems = new Dictionary<string, PropertyStoreItem>();

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._type);
                foreach (PropertyDescriptor property in properties)
                {
                    PropertyStoreItem item = new PropertyStoreItem(property.PropertyType, GetExplicitAttributes(property).OfType<Attribute>());
                    propertyStoreItems[property.Name] = item;
                }

                return propertyStoreItems;
            }

            /// <summary>
            /// Method to extract only the explicitly specified attributes from a <see cref="PropertyDescriptor"/>
            /// </summary>
            /// <remarks>
            /// Normal TypeDescriptor semantics are to inherit the attributes of a property's type.  This method
            /// exists to suppress those inherited attributes.
            /// </remarks>
            /// <param name="propertyDescriptor">The property descriptor whose attributes are needed.</param>
            /// <returns>A new <see cref="AttributeCollection"/> stripped of any attributes from the property's type.</returns>
            public static AttributeCollection GetExplicitAttributes(PropertyDescriptor propertyDescriptor)
            {
                List<Attribute> attributes = propertyDescriptor.Attributes.OfType<Attribute>().ToList();
                IEnumerable<Attribute> typeAttributes = TypeDescriptor.GetAttributes(propertyDescriptor.PropertyType).OfType<Attribute>();
                bool removedAttribute = false;
                foreach (Attribute attr in typeAttributes)
                {
                    for (int i = attributes.Count - 1; i >= 0; --i)
                    {
                        // We must use ReferenceEquals since attributes could Match if they are the same.
                        // Only ReferenceEquals will catch actual duplications.
                        if (ReferenceEquals(attr, attributes[i]))
                        {
                            attributes.RemoveAt(i);
                            removedAttribute = true;
                        }
                    }
                }
                return removedAttribute ? new AttributeCollection(attributes.ToArray()) : propertyDescriptor.Attributes;
            }
        }

        /// <summary>
        /// Private class to store data associated with a property
        /// </summary>
        class PropertyStoreItem : StoreItem
        {
            internal PropertyStoreItem(Type propertyType, IEnumerable<Attribute> attributes): base(attributes)
            {
                this.PropertyType = propertyType;
            }

            internal Type PropertyType { get; }
        }
    }
}
