using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
    /// <summary>
    /// Represents a class member, such as a property or event. This is an abstract base class.
    /// </summary>
    public abstract class MemberDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescriptor"/> class with the specified name of the member.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <exception cref="ArgumentException">The name is an empty string ("") or null.</exception>
        protected MemberDescriptor(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescriptor"/> class with the specified name of the member and an array of attributes.
        /// </summary>
        /// <param name="name">The name of the member.</param>
        /// <param name="attributes">An array of type <see cref="Attribute"/> that contains the member attributes.</param>
        protected MemberDescriptor(string name, Attribute[] attributes)
        {
            try
            {
                if (name == null || name.Length == 0)
                {
                    throw new ArgumentException("Invalid member name.");
                }
                this.name = name;
                this.displayName = name;
                this.nameHash = name.GetHashCode();
                if (attributes != null)
                {
                    this.attributes = attributes;
                    //attributesFiltered = false;
                }

                this.originalAttributes = this.attributes;
            }
            catch (Exception t)
            {
                Debug.Fail(t.ToString());
                throw t;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescriptor"/> class with the specified <see cref="MemberDescriptor"/>.
        /// </summary>
        /// <param name="descr">A <see cref="MemberDescriptor"/> that contains the name of the member and its attributes.</param>
        protected MemberDescriptor(MemberDescriptor descr)
        {
            this.name = descr.Name;
            this.displayName = this.name;
            this.nameHash = this.name.GetHashCode();

            this.attributes = new Attribute[descr.Attributes.Count];
            descr.Attributes.CopyTo(this.attributes, 0);

            //attributesFiltered = true;

            this.originalAttributes = this.attributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberDescriptor"/> class with the name in the specified <see cref="MemberDescriptor"/> and the attributes in both the old <see cref="MemberDescriptor"/> and the <see cref="Attribute"/> array.
        /// </summary>
        /// <param name="oldMemberDescriptor">A <see cref="MemberDescriptor"/> that has the name of the member and its attributes.</param>
        /// <param name="newAttributes">An array of <see cref="Attribute"/> objects with the attributes you want to add to the member.</param>
        protected MemberDescriptor(MemberDescriptor oldMemberDescriptor, Attribute[] newAttributes)
        {
            this.name = oldMemberDescriptor.Name;
            this.displayName = oldMemberDescriptor.DisplayName;
            this.nameHash = this.name.GetHashCode();

            List<object> newArray = new List<object>();

            if (oldMemberDescriptor.Attributes.Count != 0)
            {
                foreach (object o in oldMemberDescriptor.Attributes)
                {
                    newArray.Add(o);
                }
            }

            if (newAttributes != null)
            {
                foreach (object o in newAttributes)
                {
                    newArray.Add(o);
                }
            }

            this.attributes = new Attribute[newArray.Count];
            newArray.CopyTo(this.attributes, 0);
            //attributesFiltered = false;

            this.originalAttributes = this.attributes;
        }

        /// <summary>
        /// Gets or sets an array of attributes.
        /// </summary>
        /// <value>An array of type <see cref="Attribute"/> that contains the attributes of this member.</value>
        /// <remarks>Accessing this member allows derived classes to modify the default set of attributes that are used in the <see cref="CreateAttributeCollection"/> method.</remarks>
        protected virtual Attribute[] AttributeArray
        {
            get
            {
                this.CheckAttributesValid();
                this.FilterAttributesIfNeeded();
                return this.attributes;
            }
            set
            {
                this.attributes = value;
                this.originalAttributes = value;
                //attributesFiltered = false;
                this.attributeCollection = null;
            }
        }

        /// <summary>
        /// Gets the collection of attributes for this member.
        /// </summary>
        public virtual AttributeCollection Attributes
        {
            get
            {
                this.CheckAttributesValid();
                if (this.attributeCollection == null)
                {
                    this.attributeCollection = this.CreateAttributeCollection();
                }
                return this.attributeCollection;
            }
        }

        /// <summary>
        /// Gets the name of the category to which the member belongs, as specified in the <see cref="CategoryAttribute"/>.
        /// </summary>
        public virtual string Category
        {
            get
            {
                if (this.category == null)
                {
                    this.category = ((CategoryAttribute)this.Attributes[typeof(CategoryAttribute)])?.Category;
                }
                return this.category;
            }
        }

        /// <summary>
        /// Gets the description of the member, as specified in the <see cref="DescriptionAttribute"/>.
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (this.description == null)
                {
                    this.description = ((DescriptionAttribute)this.Attributes[typeof(DescriptionAttribute)])?.Description;
                }
                return this.description;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the member is browsable, as specified in the <see cref="BrowsableAttribute"/>.
        /// </summary>
        public virtual bool IsBrowsable
        {
            get
            {
                if (!this.browsable.HasValue)
                {
                    this.browsable = ((BrowsableAttribute)this.Attributes[typeof(BrowsableAttribute)])?.Browsable;
                }
                return this.browsable ?? true;
            }
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public virtual string Name => this.name ?? "";

        /// <summary>
        /// Gets the hash code for the name of the member, as specified in GetHashCode().
        /// </summary>
        protected virtual int NameHashCode => this.nameHash;

        /// <summary>
        /// Gets whether this member should be set only at design time, as specified in the <see cref="DesignOnlyAttribute"/>.
        /// </summary>
        public virtual bool DesignTimeOnly => false;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <value>The name to display for the member.</value>
        public virtual string DisplayName
        {
            get
            {
                if (!(this.Attributes[typeof(DisplayNameAttribute)] is DisplayNameAttribute displayNameAttr))
                {
                    return this.displayName;
                }
                return displayNameAttr.DisplayName;
            }
        }

        /// <summary>
        /// Called each time we access the attribtes on this member descriptor to give deriving classes a chance to change them on the fly.
        /// </summary>
        void CheckAttributesValid()
        {
        }

        /// <summary>
        /// Creates a collection of attributes using the array of attributes passed to the constructor.
        /// </summary>
        /// <returns>A new <see cref="AttributeCollection"/> that contains the AttributeArray attributes.</returns>
        protected virtual AttributeCollection CreateAttributeCollection() => new AttributeCollection(this.AttributeArray);

        /// <summary>
        /// Compares this instance to the given object to see if they are equivalent.
        /// </summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns>true if equivalent; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            MemberDescriptor mdObj = (MemberDescriptor)obj;
            this.FilterAttributesIfNeeded();
            mdObj.FilterAttributesIfNeeded();

            if (mdObj.nameHash != this.nameHash)
            {
                return false;
            }

            if (mdObj.category == null != (this.category == null) ||
                (this.category != null && !mdObj.category.Equals(this.category)))
            {
                return false;
            }


            if (mdObj.description == null != (this.description == null) ||
                (this.description != null && !mdObj.description.Equals(this.description)))
            {
                return false;
            }

            if (mdObj.attributes == null != (this.attributes == null))
            {
                return false;
            }

            bool sameAttrs = true;

            if (this.attributes != null)
            {
                if (this.attributes.Length != mdObj.attributes.Length)
                {
                    return false;
                }
                for (int i = 0; i < this.attributes.Length; i++)
                {
                    if (!this.attributes[i].Equals(mdObj.attributes[i]))
                    {
                        sameAttrs = false;
                        break;
                    }
                }
            }
            return sameAttrs;
        }

        /// <summary>
        /// When overridden in a derived class, adds the attributes of the inheriting class to the specified list of attributes in the parent class.
        /// </summary>
        /// <param name="attributeList">An <see cref="IList"/> that lists the attributes in the parent class. Initially, this is empty.</param>
        protected virtual void FillAttributes(IList attributeList)
        {
            if (this.originalAttributes != null)
            {
                foreach (Attribute attr in this.originalAttributes)
                {
                    attributeList.Add(attr);
                }
            }
        }


        void FilterAttributesIfNeeded()
        {
        }

        /// <summary>
        /// Finds the given method through reflection, searching only for public methods.
        /// </summary>
        /// <param name="componentClass">The component that contains the method.</param>
        /// <param name="name">The name of the method to find.</param>
        /// <param name="args">An array of parameters for the method, used to choose between overloaded methods.</param>
        /// <param name="returnType">The type to return for the method.</param>
        /// <returns></returns>
        protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType)
            => FindMethod(componentClass, name, args, returnType, true);

        /// <summary>
        /// Finds the given method through reflection, with an option to search only public methods.
        /// </summary>
        /// <param name="componentClass"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="returnType"></param>
        /// <param name="publicOnly"></param>
        /// <returns></returns>
#pragma warning disable IDE0060 // Remove unused parameter
        protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType, bool publicOnly)
        {
            return componentClass.GetMethod(name, args);
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current <see cref="MemberDescriptor"/>.</returns>
        public override int GetHashCode() => this.nameHash;

        /// <summary>
        /// Retrieves the object that should be used during invocation of members.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the invocation target.</param>
        /// <param name="instance">The potential invocation target.</param>
        /// <returns>The object to be used during member invocations.</returns>
        protected virtual object GetInvocationTarget(Type type, object instance)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (instance == null) throw new ArgumentNullException("instance");

            return instance;
        }

        #region Privates

        readonly string name;
        readonly string displayName;
        readonly int nameHash;
        AttributeCollection attributeCollection;
        Attribute[] attributes;
        Attribute[] originalAttributes;
        string category;
        string description;
        bool? browsable;

        #endregion
    }
}
