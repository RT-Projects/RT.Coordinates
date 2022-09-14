using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Represents a coordinate on a 2D grid.</summary>
    public struct Coord : IEquatable<Coord>, IComparable<Coord>
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

        /// <summary>Override.</summary>
        public override string ToString() => $"({X}, {Y})/({Width}×{Height})";

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right, treating the grid as toroidal
        ///     (wraps around the left/right edges).</summary>
        /// <param name="dx">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord AddXWrap(int dx) => new Coord(Width, Height, ((X + dx) % Width + Width) % Width, Y);

        /// <summary>
        ///     Moves the current cell <paramref name="dy"/> number of spaces down, treating the grid as toroidal (wraps
        ///     around the top/bottom edges).</summary>
        /// <param name="dy">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord AddYWrap(int dy) => new Coord(Width, Height, X, ((Y + dy) % Height + Height) % Height);

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right and <paramref name="dy"/> number of
        ///     spaces down, treating the grid as toroidal (wraps around the edges).</summary>
        /// <param name="dx">
        ///     Amount of cells to move by in the X direction.</param>
        /// <param name="dy">
        ///     Amount of cells to move by in the Y direction.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord AddWrap(int dx, int dy) => new Coord(Width, Height, ((X + dx) % Width + Width) % Width, ((Y + dy) % Height + Height) % Height);

        /// <summary>
        ///     Moves the current cell a number of spaces to the right and down as indicated by the X and Y coordinates of
        ///     <paramref name="c"/>, treating the grid as toroidal (wraps around the edges).</summary>
        /// <param name="c">
        ///     Another cell whose X and Y coordinates to use to determine how far to move.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord AddWrap(Coord c) => AddWrap(c.X, c.Y);

        /// <summary>Implements <see cref="IEquatable{T}"/>.</summary>
        public bool Equals(Coord other) => other.Index == Index && other.Width == Width && other.Height == Height;
        /// <summary>Override.</summary>
        public override bool Equals(object obj) => obj is Coord other && Equals(other);
        /// <summary>Override.</summary>
        public override int GetHashCode() => unchecked(Index * 1048583 + Width * 1031 + Height);

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
        public static IEnumerable<Coord> Cells(int w, int h) => Enumerable.Range(0, w * h).Select(ix => new Coord(w, h, ix));

        /// <summary>Determines whether two cells are adjacent, treating the grid as toroidal (wraps around the edges).</summary>
        public bool AdjacentToWrap(Coord other) => other == AddXWrap(1) || other == AddXWrap(-1) || other == AddYWrap(1) || other == AddYWrap(-1);

        /// <summary>
        ///     Determines whether it is possible to move from the current cell in the specified direction without moving off
        ///     the edge of the grid.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of cells to move by. Default is <c>1</c>.</param>
        public bool CanGoTo(GridDirection dir, int amount = 1) => dir switch
        {
            GridDirection.Up => Y >= amount,
            GridDirection.UpRight => Y >= amount && X < Width - amount,
            GridDirection.Right => X < Width - amount,
            GridDirection.DownRight => Y < Height - amount && X < Width - amount,
            GridDirection.Down => Y < Height - amount,
            GridDirection.DownLeft => Y < Height - amount && X >= amount,
            GridDirection.Left => X >= amount,
            GridDirection.UpLeft => X >= amount && Y >= amount,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "Invalid GridDirection enum value."),
        };

        /// <summary>
        ///     Determines whether it is possible to move the specified distance from the current cell without moving off the
        ///     edge of the grid.</summary>
        /// <param name="x">
        ///     Amount of steps to move in the X direction.</param>
        /// <param name="y">
        ///     Amount of steps to move in the Y direction.</param>
        public bool CanMoveBy(int x, int y) => (X + x) >= 0 && (X + x) < Width && (Y + y) >= 0 && (Y + y) < Height;

        /// <summary>
        ///     Returns a new cell that is the specified amount of steps away from the current cell in the specified
        ///     direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Amount of steps to move by.</param>
        public Coord GoTo(GridDirection dir, int amount = 1) => !CanGoTo(dir, amount) ? throw new InvalidOperationException("Taking that many steps in that direction would go off the grid.") : GoToWrap(dir, amount);

        /// <summary>
        ///     Returns a new cell that is the specified amount of steps away from the current cell in the specified
        ///     direction, treating the grid as toroidal (wraps around the edges).</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Amount of steps to move by.</param>
        public Coord GoToWrap(GridDirection dir, int amount = 1) => dir switch
        {
            GridDirection.Up => AddWrap(0, -amount),
            GridDirection.UpRight => AddWrap(amount, -amount),
            GridDirection.Right => AddWrap(amount, 0),
            GridDirection.DownRight => AddWrap(amount, amount),
            GridDirection.Down => AddWrap(0, amount),
            GridDirection.DownLeft => AddWrap(-amount, amount),
            GridDirection.Left => AddWrap(-amount, 0),
            GridDirection.UpLeft => AddWrap(-amount, -amount),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "Invalid GridDirection enum value.")
        };

        /// <summary>
        ///     Returns the cell that is the current cell’s neighbor in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction in which to find a neighboring cell.</param>
        /// <exception cref="InvalidOperationException">
        ///     There is no neighbor in the specified direction because the cell is at the edge of the grid.</exception>
        public Coord Neighbor(GridDirection dir) => !CanGoTo(dir) ? throw new InvalidOperationException("The grid has no neighbor in that direction.") : NeighborWrap(dir);

        /// <summary>
        ///     Returns the cell that is the current cell’s neighbor in the specified direction, treating the grid as toroidal
        ///     (wraps around the edges).</summary>
        /// <param name="dir">
        ///     Direction in which to find a neighboring cell.</param>
        public Coord NeighborWrap(GridDirection dir) => dir switch
        {
            GridDirection.Up => AddWrap(0, -1),
            GridDirection.UpRight => AddWrap(1, -1),
            GridDirection.Right => AddWrap(1, 0),
            GridDirection.DownRight => AddWrap(1, 1),
            GridDirection.Down => AddWrap(0, 1),
            GridDirection.DownLeft => AddWrap(-1, 1),
            GridDirection.Left => AddWrap(-1, 0),
            GridDirection.UpLeft => AddWrap(-1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "Invalid GridDirection enum value.")
        };

        /// <summary>
        ///     Returns a collection of all of this cell’s neighbors (including diagonals).</summary>
        /// <seealso cref="OrthogonalNeighbors"/>
        public IEnumerable<Coord> Neighbors
        {
            get
            {
                for (var i = 0; i < 8; i++)
                    if (CanGoTo((GridDirection) i))
                        yield return Neighbor((GridDirection) i);
            }
        }

        /// <summary>
        ///     Returns a collection of all of this cell’s orthogonal neighbors (not including diagonals).</summary>
        /// <seealso cref="Neighbors"/>
        public IEnumerable<Coord> OrthogonalNeighbors
        {
            get
            {
                for (var i = 0; i < 4; i++)
                    if (CanGoTo((GridDirection) (2 * i)))
                        yield return Neighbor((GridDirection) (2 * i));
            }
        }

        /// <summary>Implements <see cref="IComparable{T}"/>.</summary>
        public int CompareTo(Coord other) => Index.CompareTo(other.Index);
    }
}
