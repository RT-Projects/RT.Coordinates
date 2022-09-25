using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents a tile in a 2D triangular grid.</summary>
    /// <remarks>
    ///     Represents a triangular tile in a two-dimensional grid in which the tiles alternative between being up-pointing
    ///     and down-pointing triangles. Each tri is represented as a pair of coordinates (X, Y), where X counts the tris in a
    ///     row and Y identifies the row. The (0, 0) tri is an up-pointing one.</remarks>
    public struct Tri : IEquatable<Tri>, INeighbor<Tri>, IHasSvgGeometry<Tri>
    {
        /// <summary>
        ///     Constructor.</summary>
        /// <param name="x">
        ///     Position of the tri within its row.</param>
        /// <param name="y">
        ///     Row.</param>
        public Tri(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Returns a set of tris that make up a large up-pointing triangle structure.</summary>
        /// <param name="numRows">
        ///     Number of rows of triangles.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="numRows"/> was zero or negative.</exception>
        public static IEnumerable<Tri> LargeUpPointingTriangle(int numRows)
        {
            if (numRows <= 0)
                throw new ArgumentOutOfRangeException(nameof(numRows), $"‘{nameof(numRows)}’ must be positive.");
            for (var row = 0; row < numRows; row++)
                for (var x = 0; x < 2 * row + 1; x++)
                    yield return new Tri((1 - numRows % 2) + numRows - row + x - 1, row);
        }

        /// <summary>
        ///     Returns a set of tris that make up a large down-pointing triangle structure.</summary>
        /// <param name="numRows">
        ///     Number of rows of triangles.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="numRows"/> was zero or negative.</exception>
        public static IEnumerable<Tri> LargeDownPointingTriangle(int numRows)
        {
            if (numRows <= 0)
                throw new ArgumentOutOfRangeException(nameof(numRows), $"‘{nameof(numRows)}’ must be positive.");
            for (var row = 0; row < numRows; row++)
                for (var x = 0; x < 2 * (numRows - row) - 1; x++)
                    yield return new Tri(row + 1 + x, row);
        }

        /// <summary>
        ///     Returns a set of tris that make up a large hexagon structure.</summary>
        /// <param name="sideLength">
        ///     Number of triangles along each side of the hexagon.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="sideLength"/> was zero or negative.</exception>
        public static IEnumerable<Tri> LargeHexagon(int sideLength)
        {
            if (sideLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(sideLength), $"‘{nameof(sideLength)}’ must be positive.");
            for (var row = 0; row < sideLength; row++)
                for (var x = 0; x < 2 * sideLength + 1 + 2 * row; x++)
                {
                    yield return new Tri(x - row + (sideLength & ~1), row);
                    yield return new Tri(x - row + (sideLength & ~1), 2 * sideLength - 1 - row);
                }
        }

        /// <summary>Returns the position of the tri within its row.</summary>
        public int X { get; private set; }
        /// <summary>Returns the row containing this tri.</summary>
        public int Y { get; private set; }
        /// <summary>Determines whether this tri is up-pointing (<c>true</c>) or down-pointing (<c>false</c>).</summary>
        public bool IsUpPointing => (X ^ Y) % 2 == 0;

        /// <inheritdoc/>
        public bool Equals(Tri other) => other.X == X && other.Y == Y;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Tri tri && Equals(tri);
        /// <inheritdoc/>
        public override int GetHashCode() => X * 1073741827 + Y;

        /// <inheritdoc/>
        public IEnumerable<Tri> Neighbors
        {
            get
            {
                yield return new Tri(X - 1, Y);
                yield return new Tri(X + 1, Y);
                yield return IsUpPointing ? new Tri(X, Y + 1) : new Tri(X, Y - 1);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Vertex<Tri>> Vertices
        {
            get
            {
                if (IsUpPointing)
                {
                    yield return new TriVertex(this);
                    yield return new TriVertex(new Tri(X + 1, Y + 1));
                    yield return new TriVertex(new Tri(X - 1, Y + 1));
                }
                else
                {
                    yield return new TriVertex(new Tri(X - 1, Y));
                    yield return new TriVertex(new Tri(X + 1, Y));
                    yield return new TriVertex(new Tri(X, Y + 1));
                }
            }
        }

        /// <inheritdoc/>
        public PointD Center => new PointD(X / 2d, (IsUpPointing ? Y + 2d / 3d : Y + 1d / 3d) * 0.86602540378443864676372317075294);

        /// <inheritdoc/>
        public override string ToString() => $"({X}, {Y})";
    }
}
