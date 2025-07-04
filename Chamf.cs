using System;
using System.Collections.Generic;
using System.Linq;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a cell in a <see cref="Grid"/>. Each cell may be a square or an elongated hexagon.</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='.5 .5 10.5 10.5' font-size='.2'
    ///     text-anchor='middle'&gt;&lt;path d='M0 0L1.5 0L1.5 1.5L0 1.5zM2.5 0L2 -0.5L1.5 0M0 0L-0.5 -0.5M0 1.5L-0.5 2L0
    ///     2.5M2.5 0L4 0L4 1.5L2.5 1.5zM5 0L4.5 -0.5L4 0M2.5 1.5L2 2L1.5 1.5M5 0L6.5 0L6.5 1.5L5 1.5zM7.5 0L7 -0.5L6.5 0M5
    ///     1.5L4.5 2L4 1.5M7.5 0L9 0L9 1.5L7.5 1.5zM10 0L9.5 -0.5L9 0M7.5 1.5L7 2L6.5 1.5M10 0L11.5 0L11.5 1.5L10 1.5zM12.5
    ///     0L12 -0.5L11.5 0M10 1.5L9.5 2L9 1.5M12.5 0L14 0L14 1.5L12.5 1.5zM14.5 -0.5L14 0M14 2.5L14.5 2L14 1.5M12.5 1.5L12
    ///     2L11.5 1.5M0 2.5L1.5 2.5L1.5 4L0 4zM2.5 2.5L2 2L1.5 2.5M0 4L-0.5 4.5L0 5M2.5 2.5L4 2.5L4 4L2.5 4zM5 2.5L4.5 2L4
    ///     2.5M2.5 4L2 4.5L1.5 4M5 2.5L6.5 2.5L6.5 4L5 4zM7.5 2.5L7 2L6.5 2.5M5 4L4.5 4.5L4 4M7.5 2.5L9 2.5L9 4L7.5 4zM10
    ///     2.5L9.5 2L9 2.5M7.5 4L7 4.5L6.5 4M10 2.5L11.5 2.5L11.5 4L10 4zM12.5 2.5L12 2L11.5 2.5M10 4L9.5 4.5L9 4M12.5 2.5L14
    ///     2.5L14 4L12.5 4zM14 5L14.5 4.5L14 4M12.5 4L12 4.5L11.5 4M0 5L1.5 5L1.5 6.5L0 6.5zM2.5 5L2 4.5L1.5 5M0 6.5L-0.5 7L0
    ///     7.5M2.5 5L4 5L4 6.5L2.5 6.5zM5 5L4.5 4.5L4 5M2.5 6.5L2 7L1.5 6.5M5 5L6.5 5L6.5 6.5L5 6.5zM7.5 5L7 4.5L6.5 5M5
    ///     6.5L4.5 7L4 6.5M7.5 5L9 5L9 6.5L7.5 6.5zM10 5L9.5 4.5L9 5M7.5 6.5L7 7L6.5 6.5M10 5L11.5 5L11.5 6.5L10 6.5zM12.5
    ///     5L12 4.5L11.5 5M10 6.5L9.5 7L9 6.5M12.5 5L14 5L14 6.5L12.5 6.5zM14 7.5L14.5 7L14 6.5M12.5 6.5L12 7L11.5 6.5M0
    ///     7.5L1.5 7.5L1.5 9L0 9zM2.5 7.5L2 7L1.5 7.5M0 9L-0.5 9.5L0 10M2.5 7.5L4 7.5L4 9L2.5 9zM5 7.5L4.5 7L4 7.5M2.5 9L2
    ///     9.5L1.5 9M5 7.5L6.5 7.5L6.5 9L5 9zM7.5 7.5L7 7L6.5 7.5M5 9L4.5 9.5L4 9M7.5 7.5L9 7.5L9 9L7.5 9zM10 7.5L9.5 7L9
    ///     7.5M7.5 9L7 9.5L6.5 9M10 7.5L11.5 7.5L11.5 9L10 9zM12.5 7.5L12 7L11.5 7.5M10 9L9.5 9.5L9 9M12.5 7.5L14 7.5L14
    ///     9L12.5 9zM14 10L14.5 9.5L14 9M12.5 9L12 9.5L11.5 9M0 10L1.5 10L1.5 11.5L0 11.5zM2.5 10L2 9.5L1.5 10M1.5 11.5L2
    ///     12L2.5 11.5M-0.5 12L0 11.5M2.5 10L4 10L4 11.5L2.5 11.5zM5 10L4.5 9.5L4 10M4 11.5L4.5 12L5 11.5M5 10L6.5 10L6.5
    ///     11.5L5 11.5zM7.5 10L7 9.5L6.5 10M6.5 11.5L7 12L7.5 11.5M7.5 10L9 10L9 11.5L7.5 11.5zM10 10L9.5 9.5L9 10M9 11.5L9.5
    ///     12L10 11.5M10 10L11.5 10L11.5 11.5L10 11.5zM12.5 10L12 9.5L11.5 10M11.5 11.5L12 12L12.5 11.5M12.5 10L14 10L14
    ///     11.5L12.5 11.5zM14.5 12L14 11.5M12 12L11.5 12.5L10 12.5L9.5 12L9 12.5L7.5 12.5L7 12L6.5 12.5L5 12.5L4.5 12L4
    ///     12.5L2.5 12.5L2 12L1.5 12.5L0 12.5L-0.5 12L-1 11.5L-1 10L-0.5 9.5L-1 9L-1 7.5L-0.5 7L-1 6.5L-1 5L-0.5 4.5L-1 4L-1
    ///     2.5L-0.5 2L-1 1.5L-1 0L-0.5 -0.5L0 -1L1.5 -1L2 -0.5L2.5 -1L4 -1L4.5 -0.5L5 -1L6.5 -1L7 -0.5L7.5 -1L9 -1L9.5
    ///     -0.5L10 -1L11.5 -1L12 -0.5L12.5 -1L14 -1L14.5 -0.5L15 0L15 1.5L14.5 2L15 2.5L15 4L14.5 4.5L15 5L15 6.5L14.5 7L15
    ///     7.5L15 9L14.5 9.5L15 10L15 11.5L14.5 12L14 12.5L12.5 12.5z' fill='none' stroke-width='.05' stroke='black'
    ///     /&gt;&lt;/svg&gt;</image>
    /// <remarks>Constructor.</remarks>
    public struct Chamf(int x, int y, Chamf.Tile subtile) : IEquatable<Chamf>, INeighbor<Chamf>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>X-coordinate of the cell.</summary>
        public int X { get; private set; } = x;
        /// <summary>Y-coordinate of the cell.</summary>
        public int Y { get; private set; } = y;
        /// <summary>
        ///     Specifies whether this cell is a square, a horizontal hexagon, or a vertical hexagon. The hexagons are above
        ///     and to the left of the square with the same coordinates.</summary>
        public Tile Subtile { get; private set; } = subtile;

        /// <summary>Identifies one of the three types of tiles in a <see cref="Chamf"/> grid.</summary>
        public enum Tile
        {
            /// <summary>A square, surrounded by hexagons.</summary>
            Square,
            /// <summary>A horizontally-elongated hexagon, located above the <see cref="Square"/> with the same coordinates.</summary>
            HorizHex,
            /// <summary>A vertically-elongated hexagon, located left of the <see cref="Square"/> with the same coordinates.</summary>
            VertHex
        }

        /// <summary>
        ///     Returns a set of <see cref="Chamf"/> cells that form a rectangle. Along the perimeter all the cells will be
        ///     hexagons. Only squares internal to these hexagons are included.</summary>
        /// <param name="width">
        ///     The number of squares in the x direction.</param>
        /// <param name="height">
        ///     The number of squares in the y direction.</param>
        public static IEnumerable<Chamf> Rectangle(int width, int height)
        {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    yield return new Chamf(x, y, Tile.Square);
                    yield return new Chamf(x, y, Tile.HorizHex);
                    if (x == width - 1)
                        yield return new Chamf(x + 1, y, Tile.VertHex);
                    if (y == height - 1)
                        yield return new Chamf(x, y + 1, Tile.HorizHex);
                    yield return new Chamf(x, y, Tile.VertHex);
                }
        }

        /// <inheritdoc/>
        public readonly bool Equals(Chamf other) => other.X == X && other.Y == Y && other.Subtile == Subtile;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Chamf oc && Equals(oc);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => X * 1073741831 + Y * 347 + (int) Subtile;

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Chamf"/>, going clockwise from the top-left
        ///     (square or horizontal hex) or top (vertical hex).</summary>
        public readonly Coordinates.Vertex[] Vertices => Subtile switch
        {
            Tile.Square => new[] { new Vertex(X, Y, 0), new Vertex(X, Y, 1), new Vertex(X, Y, 2), new Vertex(X, Y, 3) },
            Tile.HorizHex => [new Vertex(X, Y - 1, 3), new Vertex(X, Y - 1, 2), new Vertex(X, Y - 1, 4), new Vertex(X, Y, 1), new Vertex(X, Y, 0), new Vertex(X - 1, Y - 1, 4)],
            Tile.VertHex => [new Vertex(X - 1, Y - 1, 4), new Vertex(X, Y, 0), new Vertex(X, Y, 3), new Vertex(X - 1, Y, 4), new Vertex(X - 1, Y, 2), new Vertex(X - 1, Y, 1)],
            _ => throw new InvalidOperationException("‘Subtile’ has an unexpected value.")
        };

        /// <inheritdoc/>
        public readonly PointD Center => Subtile switch
        {
            Tile.Square => new PointD(2.5 * X + .75, 2.5 * Y + .75),
            Tile.HorizHex => new PointD(2.5 * X + .75, 2.5 * Y - .5),
            Tile.VertHex => new PointD(2.5 * X - .5, 2.5 * Y + .75),
            _ => throw new InvalidOperationException("‘Subtile’ has an unexpected value.")
        };

        /// <inheritdoc/>
        public readonly IEnumerable<Chamf> Neighbors
        {
            get
            {
                switch (Subtile)
                {
                    case Tile.Square:
                        yield return new Chamf(X, Y, Tile.HorizHex);
                        yield return new Chamf(X + 1, Y, Tile.VertHex);
                        yield return new Chamf(X, Y + 1, Tile.HorizHex);
                        yield return new Chamf(X, Y, Tile.VertHex);
                        break;

                    case Tile.HorizHex:
                        yield return new Chamf(X, Y - 1, Tile.Square);
                        yield return new Chamf(X + 1, Y - 1, Tile.VertHex);
                        yield return new Chamf(X + 1, Y, Tile.VertHex);
                        yield return new Chamf(X, Y, Tile.Square);
                        yield return new Chamf(X, Y, Tile.VertHex);
                        yield return new Chamf(X, Y - 1, Tile.VertHex);
                        break;

                    case Tile.VertHex:
                        yield return new Chamf(X, Y, Tile.HorizHex);
                        yield return new Chamf(X, Y, Tile.Square);
                        yield return new Chamf(X, Y + 1, Tile.HorizHex);
                        yield return new Chamf(X - 1, Y + 1, Tile.HorizHex);
                        yield return new Chamf(X - 1, Y, Tile.Square);
                        yield return new Chamf(X - 1, Y, Tile.HorizHex);
                        break;

                    default:
                        throw new InvalidOperationException("‘Subtile’ has an unexpected value.");
                }
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public override readonly string ToString() => string.Format("H({0},{1})/{2}", X, Y, (int) Subtile);

        /// <summary>Describes a 2D grid of square cells surrounded by hexagonal cells. Looks a bit like a 7-segment display.</summary>
        public class Grid : Structure<Chamf>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Chamf> cells, IEnumerable<Link<Chamf>> links = null, Func<Chamf, IEnumerable<Chamf>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> that forms a rectangle. Along the perimeter all the cells will be
            ///     hexagons. Only squares internal to these hexagons are included.</summary>
            /// <param name="width">
            ///     The number of squares in the x direction.</param>
            /// <param name="height">
            ///     The number of squares in the y direction.</param>
            public Grid(int width, int height)
                : base(Rectangle(width, height))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Chamf> makeModifiedStructure(IEnumerable<Chamf> cells, IEnumerable<Link<Chamf>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }

        /// <summary>Describes a vertex (gridline intersection) in a <see cref="Grid"/>.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>Specifies the x-coordinate of a square <see cref="Chamf"/>.</summary>
            public int CellX { get; private set; }
            /// <summary>Specifies the y-coordinate of a square <see cref="Chamf"/>.</summary>
            public int CellY { get; private set; }
            /// <summary>
            ///     Specifies which of the five vertices pertaining to a square <see cref="Chamf"/> this is: 0–3 describe the
            ///     square (clockwise); 4 is the additional vertex below and to the right of the square.</summary>
            public int Pos { get; private set; }

            /// <summary>Constructor.</summary>
            public Vertex(int x, int y, int pos)
            {
                if (pos < 0 || pos > 4)
                    throw new ArgumentException($"‘{pos}’ is not a valid value for ‘{nameof(pos)}’ (0–4 expected).", nameof(pos));
                CellX = x;
                CellY = y;
                Pos = pos;
            }

            /// <inheritdoc/>
            public override PointD Point => Pos switch
            {
                0 => new PointD(2.5 * CellX, 2.5 * CellY),
                1 => new PointD(2.5 * CellX + 1.5, 2.5 * CellY),
                2 => new PointD(2.5 * CellX + 1.5, 2.5 * CellY + 1.5),
                3 => new PointD(2.5 * CellX, 2.5 * CellY + 1.5),
                4 => new PointD(2.5 * CellX + 2, 2.5 * CellY + 2),
                _ => throw new InvalidOperationException("‘Pos’ has an unexpected value.")
            };

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => CellX * 1073741831 + CellY * 347 + Pos;

            /// <inheritdoc/>
            public override string ToString() => $"Ch({CellX}, {CellY})/{Pos}";
        }
    }
}
