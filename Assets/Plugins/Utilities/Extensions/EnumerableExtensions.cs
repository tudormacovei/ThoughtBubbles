using System;
using System.Collections.Generic;

namespace tdk.Utilities
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an action on each element in the sequence.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence)
            {
                action(item);
            }
        }
    }
}