using System.Collections;
using System.Collections.Generic;

namespace System.ComponentModel
{
    /// <summary>
    /// Represents a collection of properties.
    /// </summary>
    public class PropertyDescriptorCollection : ICollection, IList, IDictionary
    {
        #region Constructors

        /// <summary>
        /// An empty PropertyDescriptorCollection that can used instead of creating a new one with no items.
        /// </summary>
        public static readonly PropertyDescriptorCollection Empty = new PropertyDescriptorCollection(null, true);

        /// <summary>
        /// Initializes a new instaoce of the <see cref="PropertyDescriptorCollection"/> using the properties in <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties"></param>
        public PropertyDescriptorCollection(PropertyDescriptor[] properties)
        {
            this.properties = properties;
            if (properties == null)
            {
                this.properties = new PropertyDescriptor[0];
                this.Count = 0;
            }
            else
            {
                this.Count = properties.Length;
            }
            this.propsOwned = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDescriptorCollection"/> using the given properties and read-only value.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="readOnly"></param>
        public PropertyDescriptorCollection(PropertyDescriptor[] properties, bool readOnly) : this(properties)
        {
            this.readOnly = readOnly;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of property descriptors in the collection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the <see cref="PropertyDescriptor"/> at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual PropertyDescriptor this[int index]
        {
            get
            {
                if (index >= this.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                this.EnsurePropsOwned();
                return this.properties[index];
            }
        }

        /// <summary>
        /// Gets the property with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new virtual PropertyDescriptor this[string name] => this.Find(name, false);

        #endregion

        #region Methods
        
        /// <summary>
        /// Adds a new <see cref="PropertyDescriptor"/> to the collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(PropertyDescriptor value)
        {
            // Check sanity.
            if (this.readOnly)
            {
                throw new NotSupportedException();
            }

            // Add it.
            this.EnsureSize(this.Count + 1);
            this.properties[this.Count] = value;
            this.Count += 1;
            return this.Count - 1;
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            // Check sanity.
            if (this.readOnly)
            {
                throw new NotSupportedException();
            }

            // Clear items and cache.
            this.Count = 0;
            this.cacheIgnoreCase = null;
            this.cacheRespectCase = null;
        }

        /// <summary>
        /// Gets if the collection contains the given property descriptor.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(PropertyDescriptor value) => this.IndexOf(value) >= 0;

        /// <summary>
        /// Copy the content to the given array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            this.EnsurePropsOwned();
            Array.Copy(this.properties, 0, array, index, this.Count);
        }

        /// <summary>
        /// Gets the property descriptor with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public virtual PropertyDescriptor Find(string name, bool ignoreCase)
        {
            // Check sanity.
            if (String.IsNullOrEmpty(name)) throw new ArgumentException("Name can not be null or empty.", nameof(name));

            // Execute the correct find function.
            if (ignoreCase)
                return this.FindIgnoreCase(name);
            else
                return this.FindRespectCase(name);
        }

        /// <summary>
        /// Gets an enumerator that may be used to iterate the items in this collection.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            this.EnsurePropsOwned();
            // we can only return an enumerator on the props we actually have...
            int propCount = this.Count;
            if (this.properties.Length != propCount)
            {
                PropertyDescriptor[] enumProps = new PropertyDescriptor[propCount];
                Array.Copy(this.properties, 0, enumProps, 0, propCount);
                return enumProps.GetEnumerator();
            }
            return this.properties.GetEnumerator();
        }

        /// <summary>
        /// Gets the index of the given property descriptor.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(PropertyDescriptor value) => Array.IndexOf(this.properties, value, 0, this.Count);

        /// <summary>
        /// Inserts a property descriptor at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, PropertyDescriptor value)
        {
            if (this.readOnly)
            {
                throw new NotSupportedException();
            }

            this.EnsureSize(this.Count + 1);
            if (index < this.Count)
            {
                Array.Copy(this.properties, index, this.properties, index + 1, this.Count - index);
            }
            this.properties[index] = value;
            this.Count++;
        }

        /// <summary>
        /// Removes the given property descriptor.
        /// </summary>
        /// <param name="value"></param>
        public void Remove(PropertyDescriptor value)
        {
            // Check sanity.
            if (this.readOnly)
            {
                throw new NotSupportedException();
            }

            // Remove it.
            int index = this.IndexOf(value);
            if (index != -1)
            {
                this.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes the item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            // Check sanity.
            if (this.readOnly)
            {
                throw new NotSupportedException();
            }

            // Remove it.
            if (index < this.Count - 1)
            {
                Array.Copy(this.properties, index + 1, this.properties, index, this.Count - index - 1);
            }
            this.properties[this.Count - 1] = null;
            this.Count--;
        }

        #endregion

        #region ICollection

        int ICollection.Count => this.Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        #endregion

        #region IDictionary

        void IDictionary.Add(object key, object value)
        {
            if (!(value is PropertyDescriptor newProp))
            {
                throw new ArgumentException(nameof(value));
            }
            this.Add(newProp);
        }

        void IDictionary.Clear() => this.Clear();

        bool IDictionary.Contains(object key)
        {
            if (key is string keyString)
                return this.Find(keyString, false) != null;
            else
                return false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() => new PropertyDescriptorEnumerator(this);
        
        bool IDictionary.IsFixedSize => this.readOnly;
        
        bool IDictionary.IsReadOnly => this.readOnly;
        
        object IDictionary.this[object key]
        {
            get
            {
                if (key is string keyString)
                    return this.Find(keyString, false);
                else
                    return null;
            }
            set
            {
                if (this.readOnly)
                {
                    throw new NotSupportedException();
                }

                if (value != null && !(value is PropertyDescriptor))
                {
                    throw new ArgumentException("value");
                }

                int index = -1;

                if (key is int temp1)
                {
                    index = temp1;
                    if (index < 0 || index >= this.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
                else if (key is string temp2)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if (this.properties[i].Name.Equals(temp2))
                        {
                            index = i;
                            break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("key");
                }

                if (index == -1)
                {
                    this.Add((PropertyDescriptor)value);
                }
                else
                {
                    this.EnsurePropsOwned();

                    PropertyDescriptor p = (PropertyDescriptor)value;
                    this.properties[index] = p;
                    if (key is string keyString)
                    {
                        if (this.cacheRespectCase != null)
                            this.cacheRespectCase[keyString] = p;
                        if (this.cacheIgnoreCase != null)
                            this.cacheIgnoreCase[keyString.ToLower()] = p;
                    }
                }
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                int propCount = this.Count;
                string[] keys = new string[propCount];
                for (int i = 0; i < propCount; i++)
                {
                    keys[i] = this.properties[i].Name;
                }
                return keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                // we can only return an enumerator on the props we actually have...
                int propCount = this.Count;
                if (this.properties.Length != propCount)
                {
                    PropertyDescriptor[] newProps = new PropertyDescriptor[propCount];
                    Array.Copy(this.properties, 0, newProps, 0, propCount);
                    return newProps;
                }
                else
                {
                    return (ICollection)this.properties.Clone();
                }
            }
        }
        
        void IDictionary.Remove(object key)
        {
            if (key is string temp3)
            {
                PropertyDescriptor pd = this[temp3];
                if (pd != null)
                {
                    ((IList)this).Remove(pd);
                }
            }
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region IList

        int IList.Add(object value) => this.Add((PropertyDescriptor)value);
        
        void IList.Clear() => this.Clear();
        
        bool IList.Contains(object value) => this.Contains((PropertyDescriptor)value);
        
        int IList.IndexOf(object value) => this.IndexOf((PropertyDescriptor)value);
        
        void IList.Insert(int index, object value) => this.Insert(index, (PropertyDescriptor)value);
        
        bool IList.IsReadOnly => this.readOnly;
        
        bool IList.IsFixedSize => this.readOnly;
        
        void IList.Remove(object value) => this.Remove((PropertyDescriptor)value);
        
        void IList.RemoveAt(int index) => this.RemoveAt(index);
        
        object IList.this[int index]
        {
            get => this[index];
            set
            {
                if (this.readOnly)
                {
                    throw new NotSupportedException();
                }

                if (index >= this.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                
                if (value != null && !(value is PropertyDescriptor))
                {
                    throw new ArgumentException("value");
                }

                this.EnsurePropsOwned();
                this.properties[index] = (PropertyDescriptor)value;
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Ensures that the properties are owned by our instance so we don't sort someone elses array.
        /// </summary>
        void EnsurePropsOwned()
        {
            // Ensure that we own the array.
            if (!this.propsOwned)
            {
                this.propsOwned = true;
                if (this.properties != null)
                {
                    PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
                    Array.Copy(this.properties, 0, newProps, 0, this.Count);
                    this.properties = newProps;
                }
            }
        }

        /// <summary>
        /// Ensures that our array is big enough.
        /// </summary>
        /// <param name="sizeNeeded"></param>
        void EnsureSize(int sizeNeeded)
        {

            if (sizeNeeded <= this.properties.Length)
            {
                return;
            }

            if (this.properties == null || this.properties.Length == 0)
            {
                this.Count = 0;
                this.properties = new PropertyDescriptor[sizeNeeded];
                return;
            }

            this.EnsurePropsOwned();

            int newSize = Math.Max(sizeNeeded, this.properties.Length * 2);
            PropertyDescriptor[] newProps = new PropertyDescriptor[newSize];
            Array.Copy(this.properties, 0, newProps, 0, this.Count);
            this.properties = newProps;
        }

        /// <summary>
        /// Does a cached search for a property ignoring case-sensitivity.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PropertyDescriptor FindIgnoreCase(string name)
        {
            // Always search in lower-case.
            name = name.ToLower();
            lock (this)
            {
                // Attempt to find it in our cache.
                if (this.cacheIgnoreCase != null)
                {
                    if (this.cacheIgnoreCase.TryGetValue(name, out PropertyDescriptor result))
                        return result;
                }

                // Create it if we don't have it.
                else
                {
                    this.cacheIgnoreCase = new Dictionary<string, PropertyDescriptor>();
                }

                // Start walking the properties where we left off caching as we go.
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    PropertyDescriptor p = this.properties[i];
                    string itemNameLower = p.Name.ToLower();
                    this.cacheIgnoreCase[itemNameLower] = p;

                    // Stop if we found it.
                    if (name == itemNameLower)
                        return p;
                }

                // Not found.
                return null;
            }
        }

        /// <summary>
        ///  Does a cached search for a property respecting case-sensitivity.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PropertyDescriptor FindRespectCase(string name)
        {
            lock (this)
            {
                // Attempt to find it in our cache.
                if (this.cacheRespectCase != null)
                {
                    if (this.cacheRespectCase.TryGetValue(name, out PropertyDescriptor result))
                        return result;
                }

                // Create it if we don't have it.
                else
                {
                    this.cacheRespectCase = new Dictionary<string, PropertyDescriptor>();
                }

                // Start walking the properties where we left off caching as we go.
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    PropertyDescriptor p = this.properties[i];
                    string itemNameLower = p.Name;
                    this.cacheRespectCase[itemNameLower] = p;

                    // Stop if we found it.
                    if (name == itemNameLower)
                        return p;
                }

                // Not found.
                return null;
            }
        }


        Dictionary<string, PropertyDescriptor> cacheIgnoreCase;
        Dictionary<string, PropertyDescriptor> cacheRespectCase;
        PropertyDescriptor[] properties;
        bool propsOwned = true;
        readonly bool readOnly = false;

        #endregion

        #region Enumerator class
        
        class PropertyDescriptorEnumerator : IDictionaryEnumerator
        {
            readonly PropertyDescriptorCollection owner;
            int index = -1;

            public PropertyDescriptorEnumerator(PropertyDescriptorCollection owner)
            {
                this.owner = owner;
            }

            public object Current => this.Entry;

            public DictionaryEntry Entry
            {
                get
                {
                    PropertyDescriptor curProp = this.owner[this.index];
                    return new DictionaryEntry(curProp.Name, curProp);
                }
            }

            public object Key => this.owner[this.index].Name;

            public object Value => this.owner[this.index].Name;
            
            public bool MoveNext()
            {
                if (this.index < (this.owner.Count - 1))
                {
                    this.index++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                this.index = -1;
            }
        }

        #endregion
    }
}
