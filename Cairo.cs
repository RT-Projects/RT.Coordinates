using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents a cell in a <see cref="Grid"/>. The gridlines form an interlocking pattern of perpendicular stretched
    ///     hexagons. Each cairo is an irregular pentagon, one vertex of which can be thought of as the center of a square,
    ///     while the vertex two clockwise from that is a vertex of the same square. The remaining vertices are off from the
    ///     square’s edge but in such a way that 4 cairos make a flower-like shape which tiles the plane in a rectilinear
    ///     pattern. The inner angles of each pentagon are 120°, 90°, 120°, 120°, and 90°.</summary>
    public struct Cairo : IEquatable<Cairo>, INeighbor<Cairo>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>Identifies a square. Each cairo forms one quarter of this square.</summary>
        public Coord Cell { get; private set; }
        /// <summary>Identifies which cairo within <see cref="Cell"/> this is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public Cairo(Coord cell, Position pos)
        {
            Cell = cell;
            Pos = pos;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="x">
        ///     X-coordinate of the underlying square.</param>
        /// <param name="y">
        ///     Y-coordinate of the underlying square.</param>
        /// <param name="pos">
        ///     Position of the <see cref="Cairo"/> within the square.</param>
        public Cairo(int x, int y, Position pos)
        {
            Cell = new Coord(x, y);
            Pos = pos;
        }

        /// <summary>Identifies one of the <see cref="Cairo"/> cells that make up a square.</summary>
        public enum Position
        {
            /// <summary>The top-left <see cref="Cairo"/> within a square.</summary>
            TopLeft,
            /// <summary>The top-right <see cref="Cairo"/> within a square.</summary>
            TopRight,
            /// <summary>The bottom-right <see cref="Cairo"/> within a square.</summary>
            BottomRight,
            /// <summary>The bottom-left <see cref="Cairo"/> within a square.</summary>
            BottomLeft
        }

        /// <summary>
        ///     Constructs a grid of the specified <paramref name="width"/> and <paramref name="height"/> and divides each
        ///     square into four <see cref="Cairo"/> cells.</summary>
        public static IEnumerable<Cairo> Rectangle(int width, int height) => Coord.Rectangle(width, height).SelectMany(cell => _cairoPositions.Select(pos => new Cairo(cell, pos)));
        private static readonly Position[] _cairoPositions = (Position[]) Enum.GetValues(typeof(Position));

        /// <inheritdoc/>
        public bool Equals(Cairo other) => other.Cell.Equals(Cell) && other.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Cairo other && other.Cell.Equals(Cell) && other.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Cell.GetHashCode() * 4 + (int) Pos;
        /// <summary>Equality operator.</summary>
        public static bool operator ==(Cairo one, Cairo two) => one.Equals(two);
        /// <summary>Inequality operator.</summary>
        public static bool operator !=(Cairo one, Cairo two) => !one.Equals(two);

        /// <inheritdoc/>
        public IEnumerable<Cairo> Neighbors
        {
            get
            {
                yield return new Cairo(Cell, (Position) (((int) Pos + 1) % 4));
                yield return new Cairo(Cell, (Position) (((int) Pos + 3) % 4));
                yield return new Cairo(Cell.Move((Coord.Direction) ((2 * (int) Pos + 6) % 8)), (Position) (((int) Pos + 1) % 4));
                yield return new Cairo(Cell.Move((Coord.Direction) (2 * (int) Pos)), (Position) (((int) Pos + 3) % 4));
                yield return new Cairo(Cell.Move((Coord.Direction) (2 * (int) Pos)), (Position) (((int) Pos + 2) % 4));
            }
        }

        IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Cairo"/>, going clockwise from the vertex at the
        ///     center of <see cref="Cell"/>.</summary>
        public Coordinates.Vertex[] Vertices => Pos switch
        {
            Position.TopLeft => new Coordinates.Vertex[] {
                new Vertex(Cell, Vertex.Position.Center),
                new Vertex(Cell.Move(Coord.Direction.Left), Vertex.Position.TopRightPlus1),
                new Vertex(Cell.Move(Coord.Direction.Left), Vertex.Position.TopRight),
                new Vertex(Cell, Vertex.Position.TopLeftPlus1),
                new Vertex(Cell, Vertex.Position.TopRightMinus1)
            },
            Position.TopRight => new Coordinates.Vertex[] {
                new Vertex(Cell, Vertex.Position.Center),
                new Vertex(Cell, Vertex.Position.TopRightMinus1),
                new Vertex(Cell, Vertex.Position.TopRight),
                new Vertex(Cell, Vertex.Position.TopRightPlus1),
                new Vertex(Cell, Vertex.Position.BottomRightMinus1)
            },
            Position.BottomRight => new Coordinates.Vertex[] {
                new Vertex(Cell, Vertex.Position.Center),
                new Vertex(Cell, Vertex.Position.BottomRightMinus1),
                new Vertex(Cell.Move(Coord.Direction.Down), Vertex.Position.TopRight),
                new Vertex(Cell.Move(Coord.Direction.Down), Vertex.Position.TopRightMinus1),
                new Vertex(Cell.Move(Coord.Direction.Down), Vertex.Position.TopLeftPlus1)
            },
            Position.BottomLeft => new Coordinates.Vertex[] {
                new Vertex(Cell, Vertex.Position.Center),
                new Vertex(Cell.Move(Coord.Direction.Down), Vertex.Position.TopLeftPlus1),
                new Vertex(Cell.Move(Coord.Direction.DownLeft), Vertex.Position.TopRight),
                new Vertex(Cell.Move(Coord.Direction.Left), Vertex.Position.BottomRightMinus1),
                new Vertex(Cell.Move(Coord.Direction.Left), Vertex.Position.TopRightPlus1)
            },
            _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.")
        };

        /// <inheritdoc/>
        public PointD Center => Cell.Center;

        /// <inheritdoc/>
        public override string ToString() => $"{Cell};{(int) Pos}";

        /// <summary>
        ///     Describes a grid structure consisting of <see cref="Cairo"/> cells that join up in groups of 4 to form
        ///     (horizontally or vertically stretched) hexagons, which in turn tile the plane.</summary>
        public class Grid : Structure<Cairo>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Cairo> cells, IEnumerable<Link<Cairo>> links = null, Func<Cairo, IEnumerable<Cairo>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> consisting of a rectangular grid of the specified <paramref name="width"/>
            ///     and <paramref name="height"/>.</summary>
            public Grid(int width, int height)
                : base(Rectangle(width, height))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Cairo> makeModifiedStructure(IEnumerable<Cairo> cells, IEnumerable<Link<Cairo>> traversible) => new Grid(cells, traversible);

            /// <summary>
            ///     Generates a maze on this structure.</summary>
            /// <param name="rnd">
            ///     A random number generator.</param>
            /// <exception cref="InvalidOperationException">
            ///     The current structure is disjointed (consists of more than one piece).</exception>
            public new Grid GenerateMaze(Random rnd = null) => (Grid) base.GenerateMaze(rnd);

            /// <summary>
            ///     Generates a maze on this structure.</summary>
            /// <param name="rndNext">
            ///     A delegate that can provide random numbers.</param>
            /// <exception cref="InvalidOperationException">
            ///     The current structure is disjointed (consists of more than one piece).</exception>
            public new Grid GenerateMaze(Func<int, int, int> rndNext) => (Grid) base.GenerateMaze(rndNext);
        }

        /// <summary>Describes one of the vertices of a <see cref="Cairo"/>.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>The <see cref="Cell"/> tile that this <see cref="Vertex"/> is within.</summary>
            public Coord Cell { get; private set; }
            /// <summary>Which position within the <see cref="Cell"/> this vertex is.</summary>
            public Position Pos { get; private set; }

            /// <summary>Constructor.</summary>
            public Vertex(Coord cell, Position pos)
            {
                Cell = cell;
                Pos = pos;
            }

            /// <summary>
            ///     Describes the position of a <see cref="Vertex"/> in relation to the vertices of its containing <see
            ///     cref="Cell"/>.</summary>
            public enum Position
            {
                /// <summary>The vertex one clockwise from the top-left vertex of the referenced <see cref="Cell"/>.</summary>
                TopLeftPlus1,
                /// <summary>The vertex one counter-clockwise from <see cref="TopRight"/>.</summary>
                TopRightMinus1,
                /// <summary>The top-right vertex of the referenced <see cref="Cell"/>.</summary>
                TopRight,
                /// <summary>The vertex one clockwise from <see cref="TopRight"/>.</summary>
                TopRightPlus1,
                /// <summary>
                ///     The vertex one counter-clockwise from the bottom-right vertex of the referenced <see cref="Cell"/>.</summary>
                BottomRightMinus1,
                /// <summary>The vertex at the center of the referenced <see cref="Cell"/>.</summary>
                Center
            }

            private const double x = .35714285714285714286; // = 5/14
            private const double y = .12371791482634837811; // = √(3)/14
            private static readonly double[] xs = { x, 1 - x, 1, 1 + y, 1 - y, .5 };
            private static readonly double[] ys = { -y, y, 0, x, 1 - x, .5 };

            /// <inheritdoc/>
            public override double X => Cell.X + xs[(int) Pos];
            /// <inheritdoc/>
            public override double Y => Cell.Y + ys[(int) Pos];

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex cv && cv.Cell.Equals(Cell) && cv.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex cv && cv.Cell.Equals(Cell) && cv.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Cell.GetHashCode() * 7 + (int) Pos);
        }
    }
}
