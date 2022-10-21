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

        /// <summary>
        ///     Provides a means for derived classes to override the SVG path generation for a line segment from another
        ///     vertex <paramref name="from"/> to this one.</summary>
        /// <returns>
        ///     A string that can be inserted in the <c>d</c> attribute of an SVG <c>&lt;path&gt;</c> element.</returns>
        /// <remarks>
        ///     The implementation must assume that the SVG path is already at the <paramref name="from"/> vertex and return
        ///     only the part of the path that leads to the current vertex. For example, the default implementation returns
        ///     <c>$"L{<see cref="X"/>} {<see cref="Y"/>}"</c> and does not reference <paramref name="from"/> at all.</remarks>
        public virtual string SvgPathFragment(Vertex from) => $"L{X} {Y}";
    }
}
