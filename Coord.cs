using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Represents a coordinate on a 2D grid.</summary>
    public struct Coord : IEquatable<Coord>, IComparable<Coord>, IHasVertices<Coord>
    {
        /// <summary>Returns the index (in reading order, top-left corner is 0) of the cell.</summary>
        public int Index { get; private set; }
        /// <summary>Returns the width of the grid.</summary>
        public int Width { get; private set; }
        /// <summary>Returns the height of the grid.</summary>
        public int Height { get; private set; }
        /// <summary>Returns the X coordinate of the cell.</summary>
        public int X => Index % Width;
        /// <summary>Returns the Y coordinate of the cell.</summary>
        public int Y => Index / Width;

        /// <summary>Constructs a <see cref="Coord"/> from an index (in reading order, top-left corner is 0).</summary>
        public Coord(int width, int height, int index)
        {
            if (index < 0 || index >= width * height)
                throw new ArgumentOutOfRangeException(nameof(index), "‘index’ must be between 0 and w×h−1.");
            Width = width;
            Height = height;
            Index = index;
        }

        /// <summary>Constructs a <see cref="Coord"/> from an X and Y coordinate.</summary>
        public Coord(int width, int height, int x, int y)
        {
            if (x < 0 || x >= width)
                throw new ArgumentOutOfRangeException(nameof(x), "‘x’ must be between 0 and w−1.");
            if (y < 0 || y >= height)
                throw new ArgumentOutOfRangeException(nameof(x), "‘y’ must be between 0 and h−1.");

            Width = width;
            Height = height;
            Index = x + width * y;
        }

        /// <inheritdoc/>
        public override string ToString() => $"({X}, {Y})/({Width}×{Height})";

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by.</param>
        /// <param name="toroidal">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveXBy(int dx, bool toroidal = false) => MoveBy(dx, 0, toroidalX: toroidal);

        /// <summary>
        ///     Moves the current cell <paramref name="dy"/> number of spaces down.</summary>
        /// <param name="dy">
        ///     Amount of cells to move by.</param>
        /// <param name="toroidal">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveYBy(int dy, bool toroidal = false) => MoveBy(0, dy, toroidalY: toroidal);

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right and <paramref name="dy"/> number of
        ///     spaces down.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by in the X direction.</param>
        /// <param name="dy">
        ///     Amount of cells to move by in the Y direction.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveBy(int dx, int dy, bool toroidalX = false, bool toroidalY = false) => CanMoveBy(dx, dy, toroidalX, toroidalY)
            ? new Coord(Width, Height, ((X + dx) % Width + Width) % Width, ((Y + dy) % Height + Height) % Height)
            : throw new OutOfBoundsException();

        /// <summary>
        ///     Moves the current cell a number of spaces to the right and down as indicated by the X and Y coordinates of
        ///     <paramref name="coord"/>.</summary>
        /// <param name="coord">
        ///     Another cell whose X and Y coordinates are used to determine how far to move.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveBy(Coord coord, bool toroidalX = false, bool toroidalY = false) => MoveBy(coord.X, coord.Y, toroidalX, toroidalY);

        /// <summary>
        ///     Moves the current cell a number of spaces in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of cells to move by. Default is <c>1</c>.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The value of <paramref name="dir"/> was not one of the defined enum values of <see cref="GridDirection"/>.</exception>
        public Coord MoveBy(GridDirection dir, int amount = 1, bool toroidalX = false, bool toroidalY = false) => dir switch
        {
            GridDirection.Up => MoveBy(0, -amount, toroidalX, toroidalY),
            GridDirection.UpRight => MoveBy(amount, -amount, toroidalX, toroidalY),
            GridDirection.Right => MoveBy(amount, 0, toroidalX, toroidalY),
            GridDirection.DownRight => MoveBy(amount, amount, toroidalX, toroidalY),
            GridDirection.Down => MoveBy(0, amount, toroidalX, toroidalY),
            GridDirection.DownLeft => MoveBy(-amount, amount, toroidalX, toroidalY),
            GridDirection.Left => MoveBy(-amount, 0, toroidalX, toroidalY),
            GridDirection.UpLeft => MoveBy(-amount, -amount, toroidalX, toroidalY),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "Invalid GridDirection enum value."),
        };

        /// <summary>
        ///     Determines whether it is possible to move from the current cell <paramref name="dx"/> number of spaces to the
        ///     right and <paramref name="dy"/> number of spaces down.</summary>
        /// <param name="dx">
        ///     Amount of steps to move in the X direction.</param>
        /// <param name="dy">
        ///     Amount of steps to move in the Y direction.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        public bool CanMoveBy(int dx, int dy, bool toroidalX = false, bool toroidalY = false) =>
            (toroidalX || ((X + dx) >= 0 && (X + dx) < Width)) &&
            (toroidalY || ((Y + dy) >= 0 && (Y + dy) < Height));

        /// <summary>
        ///     Determines whether it is possible to move from the current cell a number of spaces to the right and down as
        ///     indicated by the X and Y coordinates of <paramref name="coord"/>.</summary>
        /// <param name="coord">
        ///     Another cell whose X and Y coordinates are used to determine how far to move.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        public bool CanMoveBy(Coord coord, bool toroidalX = false, bool toroidalY = false) => CanMoveBy(coord.X, coord.Y, toroidalX, toroidalY);

        /// <summary>
        ///     Determines whether it is possible to move from the current cell in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of cells to move by. Default is <c>1</c>.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The value of <paramref name="dir"/> was not one of the defined enum values of <see cref="GridDirection"/>.</exception>
        public bool CanMoveBy(GridDirection dir, int amount = 1, bool toroidalX = false, bool toroidalY = false) => dir switch
        {
            GridDirection.Up => CanMoveBy(0, -amount, toroidalX, toroidalY),
            GridDirection.UpRight => CanMoveBy(amount, -amount, toroidalX, toroidalY),
            GridDirection.Right => CanMoveBy(amount, 0, toroidalX, toroidalY),
            GridDirection.DownRight => CanMoveBy(amount, amount, toroidalX, toroidalY),
            GridDirection.Down => CanMoveBy(0, amount, toroidalX, toroidalY),
            GridDirection.DownLeft => CanMoveBy(-amount, amount, toroidalX, toroidalY),
            GridDirection.Left => CanMoveBy(-amount, 0, toroidalX, toroidalY),
            GridDirection.UpLeft => CanMoveBy(-amount, -amount, toroidalX, toroidalY),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "Invalid GridDirection enum value."),
        };

        /// <summary>Implements <see cref="IEquatable{T}"/>.</summary>
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
        /// <param name="w">
        ///     Width of the grid.</param>
        /// <param name="h">
        ///     Height of the grid.</param>
        public static IEnumerable<Coord> Rectangle(int w, int h) => Enumerable.Range(0, w * h).Select(ix => new Coord(w, h, ix));

        /// <summary>
        ///     Determines whether two cells are orthogonally adjacent (not including diagonals).</summary>
        /// <param name="other">
        ///     Other cell to compare against.</param>
        /// <param name="includeDiagonal">
        ///     If <c>true</c>, diagonal neighbors are allowed.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        public bool IsAdjacentTo(Coord other, bool includeDiagonal = false, bool toroidalX = false, bool toroidalY = false)
        {
            for (var i = 0; i < 8; i++)
                if ((includeDiagonal || i % 2 == 0) && CanMoveBy((GridDirection) i, 1, toroidalX, toroidalY) && MoveBy((GridDirection) i, 1, toroidalX, toroidalY) == other)
                    return true;
            return false;
        }

        /// <summary>
        ///     Returns a collection of all of this cell’s neighbors.</summary>
        /// <param name="includeDiagonal">
        ///     If <c>true</c>, diagonal neighbors are included.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        public IEnumerable<Coord> GetNeighbors(bool includeDiagonal = false, bool toroidalX = false, bool toroidalY = false)
        {
            for (var i = 0; i < 8; i++)
                if ((includeDiagonal || i % 2 == 0) && CanMoveBy((GridDirection) i, 1, toroidalX, toroidalY))
                    yield return MoveBy((GridDirection) i, 1, toroidalX, toroidalY);
        }

        /// <summary>Implements <see cref="IComparable{T}"/>.</summary>
        public int CompareTo(Coord other) => Index.CompareTo(other.Index);

        /// <summary>Returns the sequence of vertices that describe the shape of this cell.</summary>
        public IEnumerable<Vertex<Coord>> Vertices
        {
            get
            {
                // Clockwise from top-left
                yield return new CoordVertex(X, Y, Width, Height);
                yield return new CoordVertex(X + 1, Y, Width, Height);
                yield return new CoordVertex(X + 1, Y + 1, Width, Height);
                yield return new CoordVertex(X, Y + 1, Width, Height);
            }
        }
    }
}
