using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a square cell in a 2D rectilinear grid.</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='0.5 0.5 7 7'&gt;&lt;path d='M1 0L1 1L0 1M2 0L2 1L1 1L1 2L0 2M3
    ///     0L3 1L2 1L2 2L1 2L1 3L0 3M4 0L4 1L3 1L3 2L2 2L2 3L1 3L1 4L0 4M5 0L5 1L4 1L4 2L3 2L3 3L2 3L2 4L1 4L1 5L0 5M6 0L6
    ///     1L5 1L5 2L4 2L4 3L3 3L3 4L2 4L2 5L1 5L1 6L0 6M7 0L7 1L6 1L6 2L5 2L5 3L4 3L4 4L3 4L3 5L2 5L2 6L1 6L1 7L0 7M8 1L7
    ///     1L7 2L6 2L6 3L5 3L5 4L4 4L4 5L3 5L3 6L2 6L2 7L1 7L1 8M8 2L7 2L7 3L6 3L6 4L5 4L5 5L4 5L4 6L3 6L3 7L2 7L2 8M8 3L7
    ///     3L7 4L6 4L6 5L5 5L5 6L4 6L4 7L3 7L3 8M8 4L7 4L7 5L6 5L6 6L5 6L5 7L4 7L4 8M8 5L7 5L7 6L6 6L6 7L5 7L5 8M8 6L7 6L7
    ///     7L6 7L6 8M8 7L7 7L7 8' fill='none' stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    public struct Coord : IEquatable<Coord>, INeighbor<Coord>, INeighbor<object>, IHasSvgGeometry, IHasDirection<Coord, Coord.Direction>
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
        public override string ToString() => $"C({X},{Y})";

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveX(int dx) => Move(dx, 0);

        /// <summary>
        ///     Moves the current cell <paramref name="dy"/> number of spaces down.</summary>
        /// <param name="dy">
        ///     Amount of cells to move by.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord MoveY(int dy) => Move(0, dy);

        /// <summary>
        ///     Moves the current cell <paramref name="dx"/> number of spaces to the right and <paramref name="dy"/> number of
        ///     spaces down.</summary>
        /// <param name="dx">
        ///     Amount of cells to move by in the X direction.</param>
        /// <param name="dy">
        ///     Amount of cells to move by in the Y direction.</param>
        /// <returns>
        ///     The new <see cref="Coord"/> value.</returns>
        public Coord Move(int dx, int dy) => new Coord(X + dx, Y + dy);

        /// <summary>
        ///     Moves the current cell a number of spaces in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of cells to move by. Default is <c>1</c>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The value of <paramref name="dir"/> was not one of the defined enum values of <see cref="Direction"/>.</exception>
        public Coord Move(Direction dir, int amount = 1) => dir switch
        {
            Direction.Up => Move(0, -amount),
            Direction.UpRight => Move(amount, -amount),
            Direction.Right => Move(amount, 0),
            Direction.DownRight => Move(amount, amount),
            Direction.Down => Move(0, amount),
            Direction.DownLeft => Move(-amount, amount),
            Direction.Left => Move(-amount, 0),
            Direction.UpLeft => Move(-amount, -amount),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), $"Invalid {nameof(Direction)} enum value."),
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
        public static IEnumerable<Coord> Rectangle(int width, int height, int dx = 0, int dy = 0)
        {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    yield return new Coord(x + dx, y + dy);
        }

        /// <summary>
        ///     Determines whether two cells are orthogonally adjacent (not including diagonals).</summary>
        /// <param name="other">
        ///     Other cell to compare against.</param>
        /// <param name="includeDiagonal">
        ///     If <c>true</c>, diagonal neighbors are allowed.</param>
        public bool IsAdjacentTo(Coord other, bool includeDiagonal = false)
        {
            for (var i = 0; i < 8; i++)
                if ((includeDiagonal || i % 2 == 0) && Move((Direction) i) == other)
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
                if (includeDiagonal || i % 2 == 0)
                    yield return Move((Direction) i);
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>Returns the vertices along the perimeter of this <see cref="Coord"/>, going clockwise from the top-left.</summary>
        public Coordinates.Vertex[] Vertices => new Coordinates.Vertex[]
        {
            new Vertex(X, Y),
            new Vertex(X + 1, Y),
            new Vertex(X + 1, Y + 1),
            new Vertex(X, Y + 1)
        };

        /// <summary>Returns the center point of this cell.</summary>
        public PointD Center => new PointD(X + .5, Y + .5);

        /// <summary>Returns a collection of all of this cell’s orthogonal neighbors (no diagonals).</summary>
        public IEnumerable<Coord> Neighbors => GetNeighbors();

        IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <summary>Returns the set of chess knight’s moves from the current cell.</summary>
        public IEnumerable<Coord> KnightsMoves { get { var orig = this; return _knightsMoveOffsets.Select(k => orig.Move(k.X, k.Y)); } }
        private static readonly Coord[] _knightsMoveOffsets = { new Coord(1, -2), new Coord(2, -1), new Coord(2, 1), new Coord(1, 2), new Coord(-1, 2), new Coord(-2, 1), new Coord(-2, -1), new Coord(-1, -2) };

        /// <summary>
        ///     Tests whether this coordinate is within a given rectangular range.</summary>
        /// <param name="width">
        ///     The width of the rectangle to check.</param>
        /// <param name="height">
        ///     The height of the rectangle to check.</param>
        /// <param name="left">
        ///     The left edge of the rectangle to check.</param>
        /// <param name="top">
        ///     The top edge of the rectangle to check.</param>
        /// <returns>
        ///     Note that numerically, the <paramref name="width"/> and <paramref name="height"/> parameters are exclusive: a
        ///     coordinate at (5, 0) is outside of a rectangle of width 5 and left edge 0.</returns>
        public bool InRange(int width, int height, int left = 0, int top = 0) => X >= left && X - left < width && Y >= top && Y - top < height;

        /// <summary>
        ///     Returns the index that this coordinate would have in a rectangle of width <paramref name="width"/> in which
        ///     the cells are numbered from 0 in reading order.</summary>
        /// <param name="width">
        ///     The width of the rectangle.</param>
        /// <returns>
        ///     This method does not check if <see cref="X"/> is within range. If it is not, the returned result is
        ///     meaningless.</returns>
        public int GetIndex(int width) => X + width * Y;

        /// <summary>
        ///     Calculates the Manhattan distance between this coordinate and <paramref name="other"/>. This is the number of
        ///     orthogonal steps required to reach one from the other.</summary>
        public int ManhattanDistance(Coord other) => Math.Abs(other.X - X) + Math.Abs(other.Y - Y);

        /// <summary>Describes a 2D grid of square cells.</summary>
        public class Grid : Structure<Coord>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> links = null, Func<Coord, IEnumerable<Coord>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a rectilinear grid that is <paramref name="width"/> cells wide and <paramref name="height"/>
            ///     cells tall.</summary>
            /// <param name="width">
            ///     Width of the grid.</param>
            /// <param name="height">
            ///     Height of the grid.</param>
            /// <param name="toroidalX">
            ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
            /// <param name="toroidalY">
            ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
            public Grid(int width, int height, bool toroidalX = false, bool toroidalY = false)
                : base(Rectangle(width, height), getNeighbors: getNeighborsGetter(width, height, toroidalX, toroidalY))
            {
                _width = width;
                _height = height;
                _toroidalX = toroidalX;
                _toroidalY = toroidalY;
            }

            private Grid(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> links, int width, int height, bool toroidalX, bool toroidalY)
                : base(cells, links, null)
            {
                _width = width;
                _height = height;
                _toroidalX = toroidalX;
                _toroidalY = toroidalY;
            }

            private static Func<Coord, IEnumerable<Coord>> getNeighborsGetter(int width, int height, bool toroidalX, bool toroidalY)
            {
                return get;
                IEnumerable<Coord> get(Coord c)
                {
                    foreach (var neighbor in c.GetNeighbors())
                        if (neighbor.X >= 0 && neighbor.X < width && neighbor.Y >= 0 && neighbor.Y <= height)
                            yield return neighbor;
                    if (toroidalX && c.X == 0)
                        yield return new Coord(width - 1, c.Y);
                    if (toroidalX && c.X == width - 1)
                        yield return new Coord(0, c.Y);
                    if (toroidalY && c.Y == 0)
                        yield return new Coord(c.X, height - 1);
                    if (toroidalY && c.Y == height - 1)
                        yield return new Coord(c.X, 0);
                }
            }

            private readonly int _width;
            private readonly int _height;
            private readonly bool _toroidalX;
            private readonly bool _toroidalY;

            /// <inheritdoc/>
            protected override Structure<Coord> makeModifiedStructure(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> traversible) => new Grid(cells, traversible, _width, _height, _toroidalX, _toroidalY);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);

            /// <inheritdoc/>
            protected override EdgeInfo<Coord> svgEdgeType(Link<Coordinates.Vertex> edge, List<Coord> cells)
            {
                if (cells.Count == 1)
                {
                    var c = cells[0];
                    if (_toroidalX && c.X == 0 && edge.All(v => v is Vertex cv && cv.Cell.X == 0))
                        return new EdgeInfo<Coord> { EdgeType = _links.Contains(new Link<Coord>(c, c.MoveX(_width - 1))) ? EdgeType.Passage : EdgeType.Wall, Cell1 = c, Cell2 = c.MoveX(_width - 1) };
                    else if (_toroidalX && c.X == _width - 1 && edge.All(v => v is Vertex cv && cv.Cell.X == _width))
                        return new EdgeInfo<Coord> { EdgeType = _links.Contains(new Link<Coord>(c, c.MoveX(-_width + 1))) ? EdgeType.Passage : EdgeType.Wall, Cell1 = c, Cell2 = c.MoveX(-_width + 1) };
                    else if (_toroidalY && c.Y == 0 && edge.All(v => v is Vertex cv && cv.Cell.Y == 0))
                        return new EdgeInfo<Coord> { EdgeType = _links.Contains(new Link<Coord>(c, c.MoveY(_height - 1))) ? EdgeType.Passage : EdgeType.Wall, Cell1 = c, Cell2 = c.MoveY(_height - 1) };
                    else if (_toroidalY && c.Y == _height - 1 && edge.All(v => v is Vertex cv && cv.Cell.Y == _height))
                        return new EdgeInfo<Coord> { EdgeType = _links.Contains(new Link<Coord>(c, c.MoveY(-_height + 1))) ? EdgeType.Passage : EdgeType.Wall, Cell1 = c, Cell2 = c.MoveY(-_height + 1) };
                }
                return base.svgEdgeType(edge, cells);
            }

            /// <inheritdoc/>
            protected override bool drawBridge(Link<Coord> link)
            {
                var c = link.Apart(out var d);
                return !(_toroidalX && Math.Abs(c.X - d.X) + 1 == _width || _toroidalY && Math.Abs(c.Y - d.Y) + 1 == _height);
            }
        }

        /// <summary>Describes a vertex (gridline intersection) in a rectilinear grid (<see cref="Grid"/>).</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>Returns the grid cell whose top-left corner is this coordinate.</summary>
            public Coord Cell { get; private set; }

            /// <summary>
            ///     Constructor.</summary>
            /// <param name="x">
            ///     The x-coordinate of the cell that has this vertex as its top-left corner.</param>
            /// <param name="y">
            ///     The y-coordinate of the cell that has this vertex as its top-left corner.</param>
            public Vertex(int x, int y)
            {
                Cell = new Coord(x, y);
            }

            /// <summary>Constructor.</summary>
            public Vertex(Coord cell)
            {
                Cell = cell;
            }

            /// <inheritdoc/>
            public override PointD Point => new PointD(Cell.X, Cell.Y);

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex cv && cv.Cell.Equals(Cell);
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex cv && cv.Cell.Equals(Cell);
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Cell.GetHashCode() + 347);
        }

        /// <summary>Identifies a direction within a 2D rectilinear grid.</summary>
        public enum Direction
        {
            /// <summary>Up (dx = 0, dy = -1).</summary>
            Up,
            /// <summary>Up and right (dx = 1, dy = -1).</summary>
            UpRight,
            /// <summary>Right (dx = 1, dy = 0).</summary>
            Right,
            /// <summary>Down and right (dx = 1, dy = 1).</summary>
            DownRight,
            /// <summary>Down (dx = 0, dy = 1).</summary>
            Down,
            /// <summary>Down and left (dx = -1, dy = 1).</summary>
            DownLeft,
            /// <summary>Left (dx = -1, dy = 0).</summary>
            Left,
            /// <summary>Up and left (dx = -1, dy = -1).</summary>
            UpLeft
        }

        /// <summary>Provides a collection of all orthogonal directions.</summary>
        public static readonly IEnumerable<Direction> OrthogonalDirections = new[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        /// <summary>Provides a collection of all diagonal directions.</summary>
        public static readonly IEnumerable<Direction> DiagonalDirections = new[] { Direction.UpRight, Direction.DownRight, Direction.DownLeft, Direction.UpLeft };
        /// <summary>Provides a collection of all directions.</summary>
        public static readonly IEnumerable<Direction> AllDirections = (Direction[]) Enum.GetValues(typeof(Direction));

        /// <summary>Addition operator.</summary>
        public static Coord operator +(Coord one, Coord two) => new Coord(one.X + two.X, one.Y + two.Y);
        /// <summary>Subtraction operator.</summary>
        public static Coord operator -(Coord one, Coord two) => new Coord(one.X - two.X, one.Y - two.Y);
    }
}
