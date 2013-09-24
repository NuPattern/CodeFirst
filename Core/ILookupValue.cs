namespace NuPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ILookupValue<TKey, TValue> : IEnumerable<TValue>
    {
        bool Contains(TKey key);
        int Count { get; }
        IEnumerable<TKey> Keys { get; }
        TValue this[TKey key] { get; }
    }
}