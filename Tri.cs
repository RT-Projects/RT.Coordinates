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
    public struct Tri : IEquatable<Tri>, INeighbor<Tri>, IHasSvgGeometry
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
        public int X { get; private set; }
        /// <summary>Returns the row containing this tri.</summary>
        public int Y { get; private set; }
        /// <summary>Determines whether this tri is up-pointing (<c>true</c>) or down-pointing (<c>false</c>).</summary>
        public bool IsUpPointing => ((X ^ Y) & 1) == 0;

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
        public IEnumerable<Vertex> Vertices
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