namespace NuPattern
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class LookupValue<TKey, TValue> : ILookupValue<TKey, TValue>
    {
        private Dictionary<TKey, TValue> values = new Dictionary<TKey, TValue>();

        public bool Contains(TKey key)
        {
            return values.ContainsKey(key);
        }

        public int Count
        {
            get { return values.Count; }
        }

        public IEnumerable<TKey> Keys
        {
            get { return values.Keys; }
        }

        public TValue this[TKey key]
        {
            get { return values.Find(key); }
            set { values[key] = value; }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return values.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}