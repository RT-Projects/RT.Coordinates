using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex in a 2D structure.</summary>
    public abstract class Vertex<TCell> : IEquatable<Vertex<TCell>>
    {
        /// <summary>Returns the set of cells that share this vertex.</summary>
        public abstract IEnumerable<TCell> Cells { get; }

        /// <summary>Returns the x-coordinate of this vertex in SVG space.</summary>
        public abstract double X { get; }
        /// <summary>Returns the y-coordinate of this vertex in SVG space.</summary>
        public abstract double Y { get; }

        /// <inheritdoc/>
        public abstract bool Equals(Vertex<TCell> other);

        /// <inheritdoc/>
        public abstract override bool Equals(object obj);

        /// <inheritdoc/>
        public abstract override int GetHashCode();

        /// <summary>Compares two <see cref="Vertex{TCell}"/> values for equality.</summary>
        public static bool operator ==(Vertex<TCell> one, Vertex<TCell> two) => one.Equals(two);
        /// <summary>Compares two <see cref="Vertex{TCell}"/> values for inequality.</summary>
        public static bool operator !=(Vertex<TCell> one, Vertex<TCell> two) => !one.Equals(two);

        /// <inheritdoc/>
        public override string ToString() => $"({X},{Y})";
    }
}
