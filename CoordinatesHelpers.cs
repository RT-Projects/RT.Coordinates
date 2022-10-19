using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RT.Coordinates
{
    internal static class CoordinatesHelpers
    {
        /// <summary>
        ///     Adds an item to a list inside a dictionary, or creates a new list with that item if the dictionary does not
        ///     already contain the key.</summary>
        public static void AddSafe<TKey, TValue>(this Dictionary<TKey, List<TValue>> dic, TKey key, TValue newValue)
        {
            if (dic.TryGetValue(key, out var list))
                list.Add(newValue);
            else
                dic[key] = new List<TValue> { newValue };
        }

        /// <summary>
        ///     Enumerates all consecutive pairs of the elements.</summary>
        /// <param name="source">
        ///     The input enumerable.</param>
        /// <param name="closed">
        ///     If true, an additional pair containing the last and first element is included. For example, if the source
        ///     collection contains { 1, 2, 3, 4 } then the enumeration contains { (1, 2), (2, 3), (3, 4) } if <paramref
        ///     name="closed"/> is <c>false</c>, and { (1, 2), (2, 3), (3, 4), (4, 1) } if <paramref name="closed"/> is
        ///     <c>true</c>.</param>
        /// <param name="selector">
        ///     The selector function to run each consecutive pair through.</param>
        public static IEnumerable<TResult> SelectConsecutivePairs<T, TResult>(this IEnumerable<T> source, bool closed, Func<T, T, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            IEnumerable<TResult> selectConsecutivePairsIterator()
            {
                using var e = source.GetEnumerator();
                bool any = e.MoveNext();
                if (!any)
                    yield break;
                T first = e.Current;
                T last = e.Current;
                while (e.MoveNext())
                {
                    yield return selector(last, e.Current);
                    last = e.Current;
                }
                if (closed)
                    yield return selector(last, first);
            }
            return selectConsecutivePairsIterator();
        }

        /// <summary>
        ///     Turns all elements in the enumerable to strings and joins them using the specified <paramref
        ///     name="separator"/> and the specified <paramref name="prefix"/> and <paramref name="suffix"/> for each string.</summary>
        /// <param name="values">
        ///     The sequence of elements to join into a string.</param>
        /// <param name="separator">
        ///     Optionally, a separator to insert between each element and the next.</param>
        /// <param name="prefix">
        ///     Optionally, a string to insert in front of each element.</param>
        /// <param name="suffix">
        ///     Optionally, a string to insert after each element.</param>
        /// <param name="lastSeparator">
        ///     Optionally, a separator to use between the second-to-last and the last element.</param>
        /// <example>
        ///     <code>
        ///         // Returns "[Paris], [London], [Tokyo]"
        ///         (new[] { "Paris", "London", "Tokyo" }).JoinString(", ", "[", "]")
        ///         
        ///         // Returns "[Paris], [London] and [Tokyo]"
        ///         (new[] { "Paris", "London", "Tokyo" }).JoinString(", ", "[", "]", " and ");</code></example>
        public static string JoinString<T>(this IEnumerable<T> values, string separator = null, string prefix = null, string suffix = null, string lastSeparator = null)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            lastSeparator ??= separator;

            using var enumerator = values.GetEnumerator();
            if (!enumerator.MoveNext())
                return "";

            // Optimize the case where there is only one element
            var one = enumerator.Current;
            if (!enumerator.MoveNext())
                return prefix + one + suffix;

            // Optimize the case where there are only two elements
            var two = enumerator.Current;
            if (!enumerator.MoveNext())
            {
                // Optimize the (common) case where there is no prefix/suffix; this prevents an array allocation when calling string.Concat()
                if (prefix == null && suffix == null)
                    return one + lastSeparator + two;
                return prefix + one + suffix + lastSeparator + prefix + two + suffix;
            }

            StringBuilder sb = new StringBuilder()
                .Append(prefix).Append(one).Append(suffix).Append(separator)
                .Append(prefix).Append(two).Append(suffix);
            var prev = enumerator.Current;
            while (enumerator.MoveNext())
            {
                sb.Append(separator).Append(prefix).Append(prev).Append(suffix);
                prev = enumerator.Current;
            }
            sb.Append(lastSeparator).Append(prefix).Append(prev).Append(suffix);
            return sb.ToString();
        }

        /// <summary>
        ///     Turns the specified value into a nullable value.</summary>
        /// <typeparam name="TInput">
        ///     Any non-nullable value type.</typeparam>
        /// <param name="input">
        ///     Any value.</param>
        /// <returns>
        ///     The same value cast as nullable.</returns>
        public static TInput? Nullable<TInput>(this TInput input) where TInput : struct
        {
            return (TInput?) input;
        }

        /// <summary>
        ///     Returns only the non-<c>null</c> elements from the specified collection of nullable values as non-nullable
        ///     values.</summary>
        /// <typeparam name="T">
        ///     The inner value type.</typeparam>
        /// <param name="src">
        ///     A collection of nullable values.</param>
        /// <returns>
        ///     A collection containing only those values that aren�t <c>null</c>.</returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> src) where T : struct
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            IEnumerable<T> whereNotNullIterator()
            {
                foreach (var tq in src)
                    if (tq != null)
                        yield return tq.Value;
            }
            return whereNotNullIterator();
        }

        public static IEnumerable<Link<Vertex>> MakeEdges(this IEnumerable<Vertex> vertices) => vertices.SelectConsecutivePairs(true, (v1, v2) => new Link<Vertex>(v1, v2));

        /// <summary>
        ///     Returns the index of the first element in this <paramref name="source"/> satisfying the specified <paramref
        ///     name="predicate"/>. If no such elements are found, returns <c>-1</c>.</summary>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            int index = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                    return index;
                index++;
            }
            return -1;
        }
    }
}
