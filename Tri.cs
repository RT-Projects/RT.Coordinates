using System;
using System.Collections.Generic;
using System.Linq;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a tile in a 2D triangular grid.</para></summary>
    /// <remarks>
    ///     Represents a triangular tile in a two-dimensional grid in which the tiles alternative between being up-pointing
    ///     and down-pointing triangles. Each tri is represented as a pair of coordinates (X, Y), where X counts the tris in a
    ///     row and Y identifies the row. The (0, 0) tri is an up-pointing one.</remarks>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='0.25 0.39711432 7 7'&gt;&lt;path d='M0 0L0.75
    ///     1.29903810567666L-0.75 1.29903810567666M2.25 1.29903810567666L1.5 0L0.75 1.29903810567666zM3.75 1.29903810567666L3
    ///     0L2.25 1.29903810567666zM5.25 1.29903810567666L4.5 0L3.75 1.29903810567666zM6.75 1.29903810567666L6 0L5.25
    ///     1.29903810567666zM7.5 0L6.75 1.29903810567666L8.25 1.29903810567666M1.5 2.59807621135332L0.75 1.29903810567666L0
    ///     2.59807621135332zM3 2.59807621135332L2.25 1.29903810567666L1.5 2.59807621135332zM4.5 2.59807621135332L3.75
    ///     1.29903810567666L3 2.59807621135332zM6 2.59807621135332L5.25 1.29903810567666L4.5 2.59807621135332zM7.5
    ///     2.59807621135332L6.75 1.29903810567666L6 2.59807621135332zM0 2.59807621135332L0.75 3.89711431702997L-0.75
    ///     3.89711431702997M2.25 3.89711431702997L1.5 2.59807621135332L0.75 3.89711431702997zM3.75 3.89711431702997L3
    ///     2.59807621135332L2.25 3.89711431702997zM5.25 3.89711431702997L4.5 2.59807621135332L3.75 3.89711431702997zM6.75
    ///     3.89711431702997L6 2.59807621135332L5.25 3.89711431702997zM7.5 2.59807621135332L6.75 3.89711431702997L8.25
    ///     3.89711431702997M1.5 5.19615242270663L0.75 3.89711431702997L0 5.19615242270663zM3 5.19615242270663L2.25
    ///     3.89711431702997L1.5 5.19615242270663zM4.5 5.19615242270663L3.75 3.89711431702997L3 5.19615242270663zM6
    ///     5.19615242270663L5.25 3.89711431702997L4.5 5.19615242270663zM7.5 5.19615242270663L6.75 3.89711431702997L6
    ///     5.19615242270663zM0 5.19615242270663L0.75 6.49519052838329L-0.75 6.49519052838329M2.25 6.49519052838329L1.5
    ///     5.19615242270663L0.75 6.49519052838329zM3.75 6.49519052838329L3 5.19615242270663L2.25 6.49519052838329zM5.25
    ///     6.49519052838329L4.5 5.19615242270663L3.75 6.49519052838329zM6.75 6.49519052838329L6 5.19615242270663L5.25
    ///     6.49519052838329zM7.5 5.19615242270663L6.75 6.49519052838329L8.25 6.49519052838329M7.5 7.79422863405995L6.75
    ///     6.49519052838329L6 7.79422863405995L5.25 6.49519052838329L4.5 7.79422863405995L3.75 6.49519052838329L3
    ///     7.79422863405995L2.25 6.49519052838329L1.5 7.79422863405995L0.75 6.49519052838329L0 7.79422863405995' fill='none'
    ///     stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    public struct Tri(int x, int y) : IEquatable<Tri>, INeighbor<Tri>, INeighbor<object>, IHasSvgGeometry
    {

        /// <summary>
        ///     Returns a set of tris that make up a large up-pointing triangle structure.</summary>
        /// <param name="sideLength">
        ///     Number of tri cells per side.</param>
        /// <param name="dx">
        ///     Offset to shift the x-coordinates by. Default is <c>0</c>.</param>
        /// <param name="dy">
        ///     Offset to shift the y-coordinates by. Default is <c>0</c>.</param>
        /// <remarks>
        ///     With <paramref name="dx"/> and <paramref name="dy"/> equal to <c>0</c>, the bottom-left tri has an
        ///     x-coordinate of <c>0</c> for odd values of <paramref name="sideLength"/> and <c>1</c> for even values, while
        ///     the top tri has a y-coordinate of <c>0</c>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="sideLength"/> was zero or negative.</exception>
        /// <exception cref="ArgumentException">
        ///     The values of <paramref name="dx"/> and <paramref name="dy"/> are not both even or both odd.</exception>
        public static IEnumerable<Tri> LargeUpPointingTriangle(int sideLength, int dx = 0, int dy = 0)
        {
            if (sideLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(sideLength), $"‘{nameof(sideLength)}’ must be positive.");
            if (((dx ^ dy) & 1) != 0)
                throw new ArgumentException($"The values of ‘{nameof(dx)}’ and ‘{nameof(dy)}’ must be both even or both odd.", nameof(dy));
            for (var row = 0; row < sideLength; row++)
                for (var x = 0; x < 2 * row + 1; x++)
                    yield return new Tri((1 - sideLength % 2) + sideLength - row + x - 1 + dx, row + dy);
        }

        /// <summary>
        ///     Returns a set of tris that make up a large down-pointing triangle structure.</summary>
        /// <param name="sideLength">
        ///     Number of tri cells per side.</param>
        /// <param name="dx">
        ///     Offset to shift the x-coordinates by. Default is <c>0</c>.</param>
        /// <param name="dy">
        ///     Offset to shift the y-coordinates by. Default is <c>0</c>.</param>
        /// <remarks>
        ///     With <paramref name="dx"/> and <paramref name="dy"/> equal to <c>0</c>, the top-left tri has coordinates
        ///     <c>(1, 0)</c>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="sideLength"/> was zero or negative.</exception>
        /// <exception cref="ArgumentException">
        ///     The values of <paramref name="dx"/> and <paramref name="dy"/> are not both even or both odd.</exception>
        public static IEnumerable<Tri> LargeDownPointingTriangle(int sideLength, int dx = 0, int dy = 0)
        {
            if (sideLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(sideLength), $"‘{nameof(sideLength)}’ must be positive.");
            if (((dx ^ dy) & 1) != 0)
                throw new ArgumentException($"The values of ‘{nameof(dx)}’ and ‘{nameof(dy)}’ must be both even or both odd.", nameof(dy));
            for (var row = 0; row < sideLength; row++)
                for (var x = 0; x < 2 * (sideLength - row) - 1; x++)
                    yield return new Tri(row + 1 + x + dx, row + dy);
        }

        /// <summary>
        ///     Returns a set of tris that make up a large hexagon structure.</summary>
        /// <param name="sideLength">
        ///     Number of triangles along each side of the hexagon.</param>
        /// <param name="dx">
        ///     Offset to shift the x-coordinates by. Default is <c>0</c>.</param>
        /// <param name="dy">
        ///     Offset to shift the y-coordinates by. Default is <c>0</c>.</param>
        /// <remarks>
        ///     With <paramref name="dx"/> and <paramref name="dy"/> equal to <c>0</c>, the tris at the leftmost corner have
        ///     an x-coordinate of <c>0</c> for odd values of <paramref name="sideLength"/> and <c>1</c> for even values,
        ///     while the top row of tris has y-coordinates of <c>0</c>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="sideLength"/> was zero or negative.</exception>
        /// <exception cref="ArgumentException">
        ///     The values of <paramref name="dx"/> and <paramref name="dy"/> are not both even or both odd.</exception>
        public static IEnumerable<Tri> LargeHexagon(int sideLength, int dx = 0, int dy = 0)
        {
            if (sideLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(sideLength), $"‘{nameof(sideLength)}’ must be positive.");
            if (((dx ^ dy) & 1) != 0)
                throw new ArgumentException($"The values of ‘{nameof(dx)}’ and ‘{nameof(dy)}’ must be both even or both odd.", nameof(dy));
            for (var row = 0; row < sideLength; row++)
                for (var x = 0; x < 2 * sideLength + 1 + 2 * row; x++)
                {
                    yield return new Tri(x - row + (sideLength & ~1) + dx, row + dy);
                    yield return new Tri(x - row + (sideLength & ~1) + dx, 2 * sideLength - 1 - row + dy);
                }
        }

        /// <summary>Returns the position of the tri within its row.</summary>
        public int X { get; private set; } = x;
        /// <summary>Returns the row containing this tri.</summary>
        public int Y { get; private set; } = y;
        /// <summary>Determines whether this tri is up-pointing (<c>true</c>) or down-pointing (<c>false</c>).</summary>
        public readonly bool IsUpPointing => ((X ^ Y) & 1) == 0;

        /// <inheritdoc/>
        public readonly bool Equals(Tri other) => other.X == X && other.Y == Y;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Tri tri && Equals(tri);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => X * 1073741827 + Y;

        /// <inheritdoc/>
        public readonly IEnumerable<Tri> Neighbors
        {
            get
            {
                yield return new Tri(X - 1, Y);
                yield return new Tri(X + 1, Y);
                yield return IsUpPointing ? new Tri(X, Y + 1) : new Tri(X, Y - 1);
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Tri"/>, going clockwise from the top (up-pointing)
        ///     or top-left (down-pointing).</summary>
        public readonly Coordinates.Vertex[] Vertices => IsUpPointing
            ? [new Vertex(this), new Vertex(new Tri(X + 1, Y + 1)), new Vertex(new Tri(X - 1, Y + 1))]
            : [new Vertex(new Tri(X - 1, Y)), new Vertex(new Tri(X + 1, Y)), new Vertex(new Tri(X, Y + 1))];

        private const double cos60 = .5;
        private const double sin60 = .86602540378443864676372317075294;

        /// <inheritdoc/>
        public readonly PointD Center => new(X * (1.5d * cos60), (IsUpPointing ? Y + 2d / 3d : Y + 1d / 3d) * (1.5d * sin60));

        /// <inheritdoc/>
        public override readonly string ToString() => $"T({X},{Y})";

        /// <summary>Describes a 2D grid of triangular cells.</summary>
        /// <remarks>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</remarks>
        public class Grid(IEnumerable<Tri> cells, IEnumerable<Link<Tri>> links = null, Func<Tri, IEnumerable<Tri>> getNeighbors = null) : Structure<Tri>(cells, links, getNeighbors)
        {
            /// <summary>
            ///     Constructs a grid of the specified width and height in which the top-left triangle is an up-pointing one.</summary>
            /// <param name="width">
            ///     Number of triangles per row.</param>
            /// <param name="height">
            ///     Number of rows.</param>
            public Grid(int width, int height) : this(Enumerable.Range(0, width * height).Select(i => new Tri(i % width, i / width)))
            {
            }
        }

        /// <summary>Describes a vertex (gridline intersection) in a triangular grid (<see cref="Grid"/>).</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>
            ///     Constructs a <see cref="Vertex"/> representing the top vertex of an up-pointing triangle.</summary>
            /// <param name="tri">
            ///     The triangle representing this vertex.</param>
            public Vertex(Tri tri)
            {
                if (!tri.IsUpPointing)
                    throw new ArgumentException("The tri must be an up-pointing tri.", nameof(tri));
                Tri = tri;
            }

            /// <summary>Returns the tri whose top vertex is this vertex.</summary>
            public Tri Tri { get; private set; }

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex tv && tv.Tri.Equals(Tri);
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex tv && tv.Tri.Equals(Tri);
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Tri.GetHashCode() + 47);

            /// <inheritdoc/>
            public override PointD Point => new(Tri.X * cos60 * 1.5, Tri.Y * sin60 * 1.5);
        }
    }
}
