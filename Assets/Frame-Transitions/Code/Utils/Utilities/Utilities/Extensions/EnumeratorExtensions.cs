using System.Collections.Generic;

namespace tdk.Utilities
{
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Converts an IEnumerator<T> to an IEnumerable<T>.
        /// </summary>
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> e)
        {
            while (e.MoveNext())
            {
                yield return e.Current;
            }
        }
    }
}