using System;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex in a 2D structure.</summary>
    public abstract class Vertex : IEquatable<Vertex>
    {
        /// <summary>Returns the x-coordinate of this vertex in SVG space.</summary>
        public abstract double X { get; }
        /// <summary>Returns the y-coordinate of this vertex in SVG space.</summary>
        public abstract double Y { get; }

        /// <inheritdoc/>
        public abstract bool Equals(Vertex other);

        /// <inheritdoc/>
        public abstract override bool Equals(object obj);

        /// <inheritdoc/>
        public abstract override int GetHashCode();

        /// <summary>Compares two <see cref="Vertex"/> values for equality.</summary>
        public static bool operator ==(Vertex one, Vertex two) => one.Equals(two);
        /// <summary>Compares two <see cref="Vertex"/> values for inequality.</summary>
        public static bool operator !=(Vertex one, Vertex two) => !one.Equals(two);

        /// <inheritdoc/>
        public override string ToString() => $"({X},{Y})";
    }
}
