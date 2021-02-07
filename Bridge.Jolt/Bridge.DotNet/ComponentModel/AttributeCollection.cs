using System.Collections;

namespace System.ComponentModel
{
    /// <summary>
    /// Represents a collection of attributes.
    /// </summary>
    public class AttributeCollection : ICollection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        protected AttributeCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection"/> class.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute"/> that provides the attributes for this collection.</param>
        public AttributeCollection(params Attribute[] attributes)
        {
            // Init properties.
            if (attributes == null)
            {
                attributes = new Attribute[0];
            }
            this._attributes = attributes;

            // Check for null- values.
            for (int idx = 0; idx < attributes.Length; idx++)
            {
                if (attributes[idx] == null)
                {
                    throw new ArgumentNullException(nameof(attributes));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the attribute with the specified index number.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Attribute this[int index] => this.Attributes[index];

        /// <summary>
        /// Gets the attribute with the specified type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public virtual Attribute this[Type attributeType]
        {
            get
            {
                lock (internalSyncObject)
                {
                    // 2 passes here for perf.  Really!  first pass, we just 
                    // check equality, and if we don't find it, then we
                    // do the IsAssignableFrom dance.   Turns out that's
                    // a relatively expensive call and we try to avoid it
                    // since we rarely encounter derived attribute types
                    // and this list is usually short. 
                    //
                    if (this._foundAttributeTypes == null)
                    {
                        this._foundAttributeTypes = new AttributeEntry[FOUND_TYPES_LIMIT];
                    }

                    int ind = 0;

                    for (; ind < FOUND_TYPES_LIMIT; ind++)
                    {
                        if (this._foundAttributeTypes[ind].type == attributeType)
                        {
                            int index = this._foundAttributeTypes[ind].index;
                            if (index != -1)
                            {
                                return this.Attributes[index];
                            }
                            else
                            {
                                return this.GetDefaultAttribute(attributeType);
                            }
                        }
                        if (this._foundAttributeTypes[ind].type == null)
                            break;
                    }

                    ind = this._index++;

                    if (this._index >= FOUND_TYPES_LIMIT)
                    {
                        this._index = 0;
                    }

                    this._foundAttributeTypes[ind].type = attributeType;

                    int count = this.Attributes.Length;


                    for (int i = 0; i < count; i++)
                    {
                        Attribute attribute = this.Attributes[i];
                        Type aType = attribute.GetType();
                        if (aType == attributeType)
                        {
                            this._foundAttributeTypes[ind].index = i;
                            return attribute;
                        }
                    }

                    // now check the hierarchies.
                    for (int i = 0; i < count; i++)
                    {
                        Attribute attribute = this.Attributes[i];
                        Type aType = attribute.GetType();
                        if (attributeType.IsAssignableFrom(aType))
                        {
                            this._foundAttributeTypes[ind].index = i;
                            return attribute;
                        }
                    }

                    this._foundAttributeTypes[ind].index = -1;
                    return this.GetDefaultAttribute(attributeType);
                }
            }
        }

        /// <summary>
        /// Gets the attribute collection.
        /// </summary>
        protected virtual Attribute[] Attributes => this._attributes;

        /// <summary>
        /// Gets the number of attributes.
        /// </summary>
        public int Count => this.Attributes.Length;

        /// <summary>
        /// Specifies an empty collection that you can use, rather than creating a new one. This field is read-only.
        /// </summary>
        public static readonly AttributeCollection Empty = new AttributeCollection((Attribute[])null);

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether this collection of attributes has the specified attribute.
        /// </summary>
        /// <param name="attribute">An <see cref="Attribute"/> to find in the collection.</param>
        /// <returns>true if the collection contains the attribute or is the default attribute for the type of attribute; otherwise, false.</returns>
        public bool Contains(Attribute attribute)
        {
            Attribute attr = this[attribute.GetType()];
            if (attr != null && attr.Equals(attribute))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether this attribute collection contains all the specified attributes in the attribute array.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute"/> to find in the collection.</param>
        /// <returns>true if the collection contains all the attributes; otherwise, false.</returns>
        public bool Contains(Attribute[] attributes)
        {
            if (attributes == null)
            {
                return true;
            }

            for (int i = 0; i < attributes.Length; i++)
            {
                if (!this.Contains(attributes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Copies the collection to an array, starting at the specified index.
        /// </summary>
        /// <param name="array">The <see cref="Array"/> to copy the collection to.</param>
        /// <param name="index">The index to start from.</param>
        public void CopyTo(Array array, int index) => Array.Copy(this.Attributes, 0, array, index, this.Attributes.Length);

        /// <devdoc>
        ///     Returns the default value for an attribute.  This uses the following hurestic:
        ///     1.  It looks for a public static field named "Default".
        /// </devdoc>
#pragma warning disable IDE0060 // Remove unused parameter
        protected Attribute GetDefaultAttribute(Type attributeType)
        {
            // Not implemented.
            return null;
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Returns the default <see cref="Attribute"/> of a given <see cref="Type"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => this.Attributes.GetEnumerator();

        #endregion

        #region ICollection- implementation

        int ICollection.Count => this.Count;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => null;

        #endregion

        #region IEnumerable- implementation

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Private

        //static Hashtable _defaultAttributes;
        readonly Attribute[] _attributes;
        static readonly object internalSyncObject = new object();
        const int FOUND_TYPES_LIMIT = 5;
        AttributeEntry[] _foundAttributeTypes;
        int _index = 0;

        struct AttributeEntry
        {
            public Type type;
            public int index;
        }

        #endregion
    }
}
