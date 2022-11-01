using System;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex in a 2D structure.</summary>
    public abstract class Vertex : IEquatable<Vertex>
    {
        /// <summary>Returns the coordinates of this vertex in SVG space.</summary>
        public abstract PointD Point { get; }

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
        public override string ToString()
        {
            var p = Point;
            return $"({p.X},{p.Y})";
        }

        /// <summary>
        ///     Provides a means for derived classes to override the SVG path generation for a line segment from another
        ///     vertex <paramref name="from"/> to this one.</summary>
        /// <param name="from">
        ///     Previous vertex along the path.</param>
        /// <param name="getVertexPoint">
        ///     Function to obtain the 2D coordinates of the vertex. Overrides must use this instead of calling <see
        ///     cref="Point"/> directly to allow client code to use <see cref="SvgInstructions.GetVertexPoint"/> to override
        ///     the positions of vertices.</param>
        /// <param name="isLast">
        ///     <c>true</c> if we are generating a closed path and this is the last invocation. This vertex is therefore the
        ///     first in the path, as the path is returning to its starting position.</param>
        /// <returns>
        ///     A string that can be inserted in the <c>d</c> attribute of an SVG <c>&lt;path&gt;</c> element.</returns>
        /// <remarks>
        ///     The implementation must assume that the SVG path is already at the <paramref name="from"/> vertex and return
        ///     only the part of the path that leads to the current vertex. For example, the default implementation returns
        ///     <c>$"L{<see cref="PointD.X"/>} {<see cref="PointD.Y"/>}"</c> and does not reference <paramref name="from"/> at
        ///     all.</remarks>
        public virtual string SvgPathFragment(Vertex from, Func<Vertex, PointD> getVertexPoint, bool isLast)
        {
            if (isLast)
                return "";
            var p = getVertexPoint(this);
            return $"L{p.X} {p.Y}";
        }
    }
}
