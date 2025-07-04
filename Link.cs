using System;
using System.Collections;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents an unordered set of two items.</summary>
    /// <typeparam name="T">
    ///     The type of elements in the set.</typeparam>
    /// <remarks>
    ///     <para>
    ///         This type is used in this library for two purposes:</para>
    ///     <list type="bullet">
    ///         <item><description>
    ///             When used with cell types (<see cref="Square"/>, <see cref="Hex"/>, etc.), it represents a traversible
    ///             link between two cells.</description></item>
    ///         <item><description>
    ///             When used with <see cref="Vertex"/>, it represents an edge connecting two vertices when rendering SVG.</description></item></list>
    ///     <para>
    ///         Two instances of <see cref="Link{T}"/> containing the same two elements will compare equal regardless of the
    ///         order in which the elements are specified.</para></remarks>
    public readonly struct Link<T> : IEquatable<Link<T>>, IEnumerable<T>
    {
        private readonly T _elem1;
        private readonly T _elem2;

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="elem1">
        ///     One of the two elements.</param>
        /// <param name="elem2">
        ///     One of the two elements.</param>
        public Link(T elem1, T elem2)
        {
            if (elem2.Equals(elem1))
                throw new ArgumentException($"‘{nameof(elem1)}’ and ‘{nameof(elem2)}’ must be different.", nameof(elem2));
            _elem1 = elem1;
            _elem2 = elem2;
        }

        /// <summary>Returns the elements linked by this <see cref="Link{T}"/> without guaranteeing an order.</summary>
        public LinkEnumerator GetEnumerator() => new(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new LinkEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new LinkEnumerator(this);

        /// <summary>Iterates the two elements in a <see cref="Link{T}"/>.</summary>
        public struct LinkEnumerator(Link<T> link) : IEnumerator<T>
        {
            private int _pos = 0;

            /// <inheritdoc/>
            public readonly T Current => _pos switch { 1 => link._elem1, 2 => link._elem2, _ => throw new InvalidOperationException() };
            readonly object IEnumerator.Current => Current;
            /// <inheritdoc/>
            public bool MoveNext() { if (_pos < 3) _pos++; return _pos < 3; }
            /// <inheritdoc/>
            public readonly void Dispose() { }
            /// <inheritdoc/>
            public void Reset() => _pos = 0;
        }

        /// <summary>
        ///     Given an element in this link, returns the other element.</summary>
        /// <param name="one">
        ///     One of the elements in this link.</param>
        /// <exception cref="ArgumentException">
        ///     The specified element is not part of this link.</exception>
        public readonly T Other(T one) => _elem1.Equals(one) ? _elem2 : _elem2.Equals(one) ? _elem1 : throw new ArgumentException($"‘{one}’ is not part of the link.", nameof(one));

        /// <summary>Returns the elements of this link in no particular order.</summary>
        public readonly T Apart(out T other) { other = _elem2; return _elem1; }

        /// <inheritdoc/>
        public readonly bool Equals(Link<T> other) => (_elem1.Equals(other._elem1) && _elem2.Equals(other._elem2)) || (_elem1.Equals(other._elem2) && _elem2.Equals(other._elem1));

        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Link<T> link && Equals(link);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var eq = EqualityComparer<T>.Default;
            return eq.GetHashCode(_elem1) ^ eq.GetHashCode(_elem2);
        }

        /// <summary>Determines whether this link and <paramref name="other"/> have at least one element in common.</summary>
        public readonly bool HalfEquals(Link<T> other) => _elem1.Equals(other._elem1) || _elem2.Equals(other._elem2) || _elem1.Equals(other._elem2) || _elem2.Equals(other._elem1);

        /// <inheritdoc/>
        public override readonly string ToString() => $"{_elem1} ↔ {_elem2}";

        /// <summary>Compares two <see cref="Link{T}"/> values for equality.</summary>
        public static bool operator ==(Link<T> one, Link<T> two) => one.Equals(two);
        /// <summary>Compares two <see cref="Link{T}"/> values for inequality.</summary>
        public static bool operator !=(Link<T> one, Link<T> two) => !one.Equals(two);

        /// <summary>
        ///     Deconstructor.</summary>
        /// <remarks>
        ///     The elements are not returned in any predictable order.</remarks>
        public void Deconstruct(out T elem1, out T elem2)
        {
            elem1 = _elem1;
            elem2 = _elem2;
        }

        /// <summary>Projects the elements of this link and creates a new link between the resulting new elements.</summary>
        public Link<TResult> SelectLink<TResult>(Func<T, TResult> selector) => new(selector(_elem1), selector(_elem2));
    }
}
