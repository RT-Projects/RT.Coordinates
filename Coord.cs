using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Represents a square cell in a 2D rectilinear grid.</summary>
    public struct Coord : IEquatable<Coord>, INeighbor<Coord>, IHasSvgGeometry
    {
        /// <summary>Returns the X coordinate of the cell.</summary>
        public int X { get; private set; }
        /// <summary>Returns the Y coordinate of the cell.</summary>
        public int Y { get; private set; }

        /// <summary>Constructs a <see cref="Coord"/> from an X and Y coordinate.</summary>
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <inheritdoc/>
        public override string ToString() => $"({X}, {Y})";

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveXBy(int dx) => MoveBy(dx, 0);

        /// <summary>
        ///     Moves the current cell <paramref name="dy"/> number of spaces down.</summary>
        /// <param name="dy">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveYBy(int dy) => MoveBy(0, dy);

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right and <paramref name="dy"/> number of
        ///     spaces down.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by in the X direction.</param>
        /// <param name="dy">
        ///     Amount of cells to move by in the Y direction.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveBy(int dx, int dy) => new Coord(X + dx, Y + dy);

        /// <summary>
        ///     Moves the current cell a number of spaces in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of cells to move by. Default is <c>1</c>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The value of <paramref name="dir"/> was not one of the defined enum values of <see cref="GridDirection"/>.</exception>
        public Coord MoveBy(GridDirection dir, int amount = 1) => dir switch
        {
            GridDirection.Up => MoveBy(0, -amount),
            GridDirection.UpRight => MoveBy(amount, -amount),
            GridDirection.Right => MoveBy(amount, 0),
            GridDirection.DownRight => MoveBy(amount, amount),
            GridDirection.Down => MoveBy(0, amount),
            GridDirection.DownLeft => MoveBy(-amount, amount),
            GridDirection.Left => MoveBy(-amount, 0),
            GridDirection.UpLeft => MoveBy(-amount, -amount),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), $"Invalid {nameof(GridDirection)} enum value."),
        };

        /// <summary>Compares this cell to another for equality.</summary>
        public bool Equals(Coord other) => other.X == X && other.Y == Y;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Coord other && Equals(other);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(X * 1048583 + Y);

        /// <summary>Compares two <see cref="Coord"/> values for equality.</summary>
        public static bool operator ==(Coord one, Coord two) => one.Equals(two);
        /// <summary>Compares two <see cref="Coord"/> values for inequality.</summary>
        public static bool operator !=(Coord one, Coord two) => !one.Equals(two);

        /// <summary>
        ///     Returns a collection of all cells in a grid of the specified size.</summary>
        /// <param name="width">
        ///     Width of the grid.</param>
        /// <param name="height">
        ///     Height of the grid.</param>
        /// <param name="dx">
        ///     Specifies the x-coordinate of the top-left corner. Default is <c>0</c>.</param>
        /// <param name="dy">
        ///     Specifies the y-coordinate of the top-left corner. Default is <c>0</c>.</param>
        public static IEnumerable<Coord> Rectangle(int width, int height, int dx = 0, int dy = 0) => Enumerable.Range(0, width * height).Select(ix => new Coord(ix % width + dx, ix / width + dy));

        /// <summary>
        ///     Determines whether two cells are orthogonally adjacent (not including diagonals).</summary>
        /// <param name="other">
        ///     Other cell to compare against.</param>
        /// <param name="includeDiagonal">
        ///     If <c>true</c>, diagonal neighbors are allowed.</param>
        public bool IsAdjacentTo(Coord other, bool includeDiagonal = false)
        {
            for (var i = 0; i < 8; i++)
                if ((includeDiagonal || i % 2 == 0) && MoveBy((GridDirection) i) == other)
                    return true;
            return false;
        }

        /// <summary>
        ///     Returns a collection of all of this cell’s neighbors.</summary>
        /// <param name="includeDiagonal">
        ///     If <c>true</c>, diagonal neighbors are included.</param>
        public IEnumerable<Coord> GetNeighbors(bool includeDiagonal = false)
        {
            for (var i = 0; i < 8; i++)
                if ((includeDiagonal || i % 2 == 0))
                    yield return MoveBy((GridDirection) i);
        }

        /// <summary>Returns the sequence of vertices that describe the shape of this cell.</summary>
        public IEnumerable<Vertex> Vertices
        {
            get
            {
                // Clockwise from top-left
                yield return new CoordVertex(X, Y);
                yield return new CoordVertex(X + 1, Y);
                yield return new CoordVertex(X + 1, Y + 1);
                yield return new CoordVertex(X, Y + 1);
            }
        }

        /// <summary>Returns the center point of this cell.</summary>
        public PointD Center => new PointD(X + .5, Y + .5);

        /// <summary>Returns a collection of all of this cell’s orthogonal neighbors (no diagonals).</summary>
        public IEnumerable<Coord> Neighbors => GetNeighbors();
    }
}
