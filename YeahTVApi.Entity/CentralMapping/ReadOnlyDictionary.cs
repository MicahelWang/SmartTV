using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// Read only Dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>
    {
        IDictionary<TKey, TValue> _dict;

        #region Indexer
        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// true if the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                if (value == false)
                {
                    throw new ArgumentException();
                }
                _IsReadOnly = value;
            }
        }
        private bool _IsReadOnly = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ReadOnlyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public ReadOnlyDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public ReadOnlyDictionary(Dictionary<TKey, TValue> pDict)
        {
            _dict = pDict;
        }
        #endregion

        #region Methods


        /// <summary>
        /// Determines whether the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// contains an element with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to locate in the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </param>
        /// <returns>
        /// true if the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }
        /// <summary>
        /// Gets an 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// containing the keys of the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// containing the keys of the object that implements 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TKey> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }
        /// <summary>
        /// Removes the element with the specified key from the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; 
        /// otherwise, false.  
        /// This method also returns false if 
        /// <paramref name="key"/> was not found in 
        /// the original 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// is read-only.
        /// </exception>
        public bool Remove(TKey key)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            return _dict.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        /// When this method returns, 
        /// the value associated with the specified key, 
        /// if the key is found; otherwise, the default 
        /// value for the type of the 
        /// <paramref name="value"/> parameter. 
        /// This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets an 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// containing the values in the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// containing the values in the object that implements 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<TValue> Values
        {
            get
            {
                return _dict.Values;
            }
        }
        /// <summary>
        /// Adds an item to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            _dict.Add(item);
        }


        /// <summary>
        /// Adds an element with the provided key and value to the 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        /// The object to use as the value of the element to add.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// An element with the same key already exists in the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The 
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> 
        /// is read-only.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            _dict.Add(key, value);
        }

        /// <summary>
        /// Removes all items from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        public void Clear()
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            _dict.Clear();


        }
        /// <summary>
        /// Determines whether the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; 
        /// otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies the elements of the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// to an <see cref="T:System.Array"/>, 
        /// starting at a particular 
        /// <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> 
        /// that is the destination of the elements copied from 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. 
        /// The <see cref="T:System.Array"/> 
        /// must have zero-based indexing.</param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> 
        /// at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or
        /// greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is greater than the available space from 
        /// <paramref name="arrayIndex"/> to the end of the 
        /// destination <paramref name="array"/>.
        /// -or-
        /// Type <paramref name="T"/> cannot be cast 
        /// automatically to the type of the 
        /// destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(
            KeyValuePair<TKey,
            TValue>[] array,
            int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from 
        /// the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully 
        /// removed from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; 
        /// otherwise, false. This method also returns false if 
        /// <paramref name="item"/> is not found in the original 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            return _dict.Remove(item.Key);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/>
        /// that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object 
        /// that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.
            GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }
        #endregion




        #region IDictionary<TKey,TValue> Members
        /// <summary>
        /// Gets or sets the value with the specified key.
        /// <para>
        /// NOTE: Throws an error if the key is not found.
        /// </para>
        /// </summary>
        /// <value></value>
        public TValue this[TKey key]
        {
            get { return _dict[key]; }
            set
            {
                if (_IsReadOnly)
                {
                    throw new InvalidOperationException();
                }
                _dict[key] = value;
            }
        }






        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds an item to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        /// The 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.
            Add(KeyValuePair<TKey, TValue> item)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            _dict.Add(item);
        }

        /// <summary>
        /// Removes all items from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            _dict.Clear();
        }

        /// <summary>
        /// Determines whether the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/>
        /// is found in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; 
        /// otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.
            Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// to an 
        /// <see cref="T:System.Array"/>, 
        /// starting at a particular 
        /// <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that 
        /// is the destination of the elements copied from 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. 
        /// The <see cref="T:System.Array"/> must have 
        /// zero-based indexing.</param>
        /// <param name="arrayIndex">
        /// The zero-based index in 
        /// <paramref name="array"/> at 
        /// which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> 
        /// is equal to or greater than the length of 
        /// <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is greater than the available space from 
        /// <paramref name="arrayIndex"/> to the end 
        /// of the destination <paramref name="array"/>.
        /// -or-
        /// Type <paramref name="T"/> cannot be cast 
        /// automatically to the type of the 
        /// destination <paramref name="array"/>.
        /// </exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
            KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);

        }

        /// <summary>
        /// Gets the number of elements contained in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        int ICollection<KeyValuePair<TKey, TValue>>.Count
        {
            get { return _dict.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>
        /// is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// true if the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only; otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return _IsReadOnly;

            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object 
        /// from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> 
        /// was successfully removed from the 
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>; 
        /// otherwise, false. 
        /// This method also returns false if 
        /// <paramref name="item"/> is not found in the 
        /// original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> 
        /// is read-only.
        /// </exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
            KeyValuePair<TKey, TValue> item)
        {
            if (_IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            return _dict.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/>
        /// that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        #endregion
    }
}
