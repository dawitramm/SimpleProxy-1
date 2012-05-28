using System.Collections.Generic;

namespace SimpleProxy.Library
{
    internal static class Extensions
    {
        public static IDictionary<TKey, TValue> ReadOnly<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}