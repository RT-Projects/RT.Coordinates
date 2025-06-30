using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a cell in a <see cref="Grid"/>. Each cell may be a square or a triangle, which are all slightly
    ///         tilted.</para></summary>
    /// <image type="raw">
    ///     &lt;svg width="10" height="10" version="1.1" viewBox="-.368 -.1 10 10"
    ///     xmlns="http://www.w3.org/2000/svg"&gt;&lt;path d="m1.3-.6996-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679
    ///     1-1-.2679.7321-.7321 1-.2679-.7321-.7321-.2679 1-1-.2679m4.268-1-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321-.2679 1-1-.2679m4.268-1-.2679 1 .2679 1-1
    ///     .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321-.2679 1-1-.2679m4.268-1-.2679 1 .2679
    ///     1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321-.2679 1-1-.2679m4.268-1-.2679 1
    ///     .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321-.2679 1-1-.2679m3.268
    ///     1.268-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321.2679-1-.2679 1-1-.2679m-7.732 1-.2679 1 .2679 1-1
    ///     .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m2 2-.2679 1-1-.2679m1-1.732.2679
    ///     1-1-.2679.7321-.7321.2679-1m-9 1.732-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m2 2-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321.2679-1m-9 1.732-.2679 1 .2679 1-1
    ///     .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679
    ///     1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321 1-.2679-.7321-.7321m2 2-.2679 1-1-.2679m1-1.732.2679
    ///     1-1-.2679.7321-.7321.2679-1m-9 1.732-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m3-.2679-.2679 1 .2679 1-1 .2679-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321
    ///     1-.2679-.7321-.7321m2 2-.2679 1-1-.2679m1-1.732.2679 1-1-.2679.7321-.7321.2679-1m-10 2 .7321.7321-1 .2679m2.268-1
    ///     .7321.7321-1 .2679-1-.2679.2679-1m3 .2679.7321.7321-1 .2679-1-.2679.2679-1m3 .2679.7321.7321-1
    ///     .2679-1-.2679.2679-1m3 .2679.7321.7321-1 .2679-1-.2679.2679-1m3 .2679-.2679 1-1-.2679.2679-1m-10.27 1
    ///     .268-1-.268-1 .268-1-.268-1 .268-1-.268-1 .268-1-.268-1 .268-1-.268-1 .268-1 1 .2679 1-.2679 1 .2679 1-.2679 1
    ///     .2679 1-.2679 1 .2679 1-.2679 1 .2679 1-.2679 1 .2679" fill="none" stroke="black"
    ///     stroke-width=".05"/&gt;&lt;/svg&gt;</image>
    public struct Snubquad(int x, int y, Snubquad.Tile subtile) : IEquatable<Snubquad>, INeighbor<Snubquad>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>X-coordinate of the cell.</summary>
        public int X { get; private set; } = x;
        /// <summary>Y-coordinate of the cell.</summary>
        public int Y { get; private set; } = y;
        /// <summary>Specifies which of the six subtiles this cell is.</summary>
        public Tile Subtile { get; private set; } = subtile;

        /// <summary>Identifies one of the three types of tiles in a <see cref="Snubquad"/> grid.</summary>
        public enum Tile
        {
            /// <summary>A square tilted clockwise, located in the top-left of the tile.</summary>
            SquareTL,
            /// <summary>The tri to the right of <see cref="SquareTL"/> and above <see cref="SquareBR"/>.</summary>
            TriTM,
            /// <summary>The tri to the right of <see cref="TriTM"/>.</summary>
            TriTR,
            /// <summary>The tri to the left of <see cref="TriBM"/> and below <see cref="SquareTL"/>.</summary>
            TriBL,
            /// <summary>The tri to the left of <see cref="SquareBR"/>.</summary>
            TriBM,
            /// <summary>A square tilted counter-clockwise, located in the bottom-right of the tile.</summary>
            SquareBR,
        }

        /// <summary>
        ///     Returns a set of <see cref="Snubquad"/> cells that form a rectangle consisting of <paramref
        ///     name="width"/>×<paramref name="height"/>×6 cells.</summary>
        /// <param name="width">
        ///     The number of tile in the x direction.</param>
        /// <param name="height">
        ///     The number of tile in the y direction.</param>
        public static IEnumerable<Snubquad> Rectangle(int width, int height)
        {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    for (var sub = 0; sub < 6; sub++)
                        yield return new Snubquad(x, y, (Tile) sub);
        }

        /// <inheritdoc/>
        public readonly bool Equals(Snubquad other) => other.X == X && other.Y == Y && other.Subtile == Subtile;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Snubquad oc && Equals(oc);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => X * 1073741833 + Y * 479 + (int) Subtile;

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Snubquad"/>, going clockwise from the top-left
        ///     (square or horizontal hex) or top (vertical hex).</summary>
        public readonly Coordinates.Vertex[] Vertices => Subtile switch
        {
            Tile.SquareTL => new[] { new Vertex(X, Y, Vertex.VertexPos.TL), new Vertex(X, Y, Vertex.VertexPos.TM), new Vertex(X, Y, Vertex.VertexPos.MC), new Vertex(X, Y, Vertex.VertexPos.ML) },
            Tile.TriTM => [new Vertex(X, Y, Vertex.VertexPos.TM), new Vertex(X + 1, Y, Vertex.VertexPos.ML), new Vertex(X, Y, Vertex.VertexPos.MC)],
            Tile.TriTR => [new Vertex(X, Y, Vertex.VertexPos.TM), new Vertex(X + 1, Y, Vertex.VertexPos.TL), new Vertex(X + 1, Y, Vertex.VertexPos.ML)],
            Tile.TriBL => [new Vertex(X, Y, Vertex.VertexPos.ML), new Vertex(X, Y, Vertex.VertexPos.MC), new Vertex(X, Y + 1, Vertex.VertexPos.TL)],
            Tile.TriBM => [new Vertex(X, Y, Vertex.VertexPos.MC), new Vertex(X, Y + 1, Vertex.VertexPos.TM), new Vertex(X, Y + 1, Vertex.VertexPos.TL)],
            Tile.SquareBR => [new Vertex(X, Y, Vertex.VertexPos.MC), new Vertex(X + 1, Y, Vertex.VertexPos.ML), new Vertex(X + 1, Y + 1, Vertex.VertexPos.TL), new Vertex(X, Y + 1, Vertex.VertexPos.TM)],
            _ => throw new InvalidOperationException("‘Subtile’ has an unexpected value.")
        };

        /// <inheritdoc/>
        public readonly PointD Center => Subtile switch
        {
            Tile.SquareTL => new PointD(2 * X + 0.3660254037844387, 2 * Y + 0.6339745962155615),
            Tile.TriTM => new PointD(2 * X + 1.1547005383792517, 2 * Y + 0.8452994616207488),
            Tile.TriTR => new PointD(2 * X + 1.5773502691896257, 2 * Y + 0.4226497308103742),
            Tile.TriBL => new PointD(2 * X + 0.15470053837925146, 2 * Y + 1.4226497308103743),
            Tile.TriBM => new PointD(2 * X + 0.5773502691896257, 2 * Y + 1.8452994616207483),
            Tile.SquareBR => new PointD(2 * X + 1.3660254037844386, 2 * Y + 1.6339745962155616),
            _ => throw new InvalidOperationException("‘Subtile’ has an unexpected value.")
        };

        /// <inheritdoc/>
        public readonly IEnumerable<Snubquad> Neighbors
        {
            get
            {
                switch (Subtile)
                {
                    case Tile.SquareTL:
                        yield return new(X, Y - 1, Tile.TriBM);
                        yield return new(X, Y, Tile.TriTM);
                        yield return new(X, Y, Tile.TriBL);
                        yield return new(X - 1, Y, Tile.TriTR);
                        break;

                    case Tile.TriTM:
                        yield return new(X, Y, Tile.TriTR);
                        yield return new(X, Y, Tile.SquareBR);
                        yield return new(X, Y, Tile.SquareTL);
                        break;

                    case Tile.TriTR:
                        yield return new(X, Y - 1, Tile.SquareBR);
                        yield return new(X + 1, Y, Tile.SquareTL);
                        yield return new(X, Y, Tile.TriTM);
                        break;

                    case Tile.TriBL:
                        yield return new(X, Y, Tile.SquareTL);
                        yield return new(X, Y, Tile.TriBM);
                        yield return new(X - 1, Y, Tile.SquareBR);
                        break;

                    case Tile.TriBM:
                        yield return new(X, Y, Tile.SquareBR);
                        yield return new(X, Y + 1, Tile.SquareTL);
                        yield return new(X, Y, Tile.TriBL);
                        break;

                    case Tile.SquareBR:
                        yield return new(X, Y, Tile.TriTM);
                        yield return new(X + 1, Y, Tile.TriBL);
                        yield return new(X, Y + 1, Tile.TriTR);
                        yield return new(X, Y, Tile.TriBM);
                        break;

                    default:
                        throw new InvalidOperationException("‘Subtile’ has an unexpected value.");
                }
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public override readonly string ToString() => $"S({X},{Y})/{(int) Subtile}";

        /// <summary>Describes a 2D grid of square cells surrounded by hexagonal cells. Looks a bit like a 7-segment display.</summary>
        public class Grid : Structure<Snubquad>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Snubquad> cells, IEnumerable<Link<Snubquad>> links = null, Func<Snubquad, IEnumerable<Snubquad>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> that forms a rectangle consisting of <paramref name="width"/>×<paramref
            ///     name="height"/>×6 cells.</summary>
            /// <param name="width">
            ///     The number of squares in the x direction.</param>
            /// <param name="height">
            ///     The number of squares in the y direction.</param>
            public Grid(int width, int height)
                : base(Rectangle(width, height))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Snubquad> makeModifiedStructure(IEnumerable<Snubquad> cells, IEnumerable<Link<Snubquad>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }

        /// <summary>
        ///     Describes a vertex (gridline intersection) in a <see cref="Grid"/>.</summary>
        /// <remarks>
        ///     Constructor.</remarks>
        public class Vertex(int x, int y, Vertex.VertexPos pos) : Coordinates.Vertex
        {
            /// <summary>Specifies the x-coordinate of a square <see cref="Snubquad"/>.</summary>
            public int CellX { get; private set; } = x;
            /// <summary>Specifies the y-coordinate of a square <see cref="Snubquad"/>.</summary>
            public int CellY { get; private set; } = y;
            /// <summary>Specifies which of the vertices within a <see cref="Snubquad"/> tile this is.</summary>
            public VertexPos Pos { get; private set; } = pos;

            /// <summary>Defines one of the vertices that make up a <see cref="Snubquad"/> lattice.</summary>
            public enum VertexPos
            {
                /// <summary>Top-left vertex (0, 0).</summary>
                TL,
                /// <summary>Top-middle vertex (½, 1−½√3).</summary>
                TM,
                /// <summary>Middle-left vertex (½√3−1, ½).</summary>
                ML,
                /// <summary>Middle-center vertex ((√3−1)/2, (3−√3)/2).</summary>
                MC
            }

            private const double s = .2679491924311227065;     // = 2 − √3

            /// <inheritdoc/>
            public override PointD Point => Pos switch
            {
                VertexPos.TL => new(2 * CellX, 2 * CellY),
                VertexPos.TM => new(2 * CellX + 1, 2 * CellY + s),
                VertexPos.ML => new(2 * CellX - s, 2 * CellY + 1),
                VertexPos.MC => new(2 * CellX + 1 - s, 2 * CellY + 1 + s),
                _ => throw new InvalidOperationException("‘Pos’ has an unexpected value.")
            };

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => CellX * 1073741833 + CellY * 479 + (int) Pos;

            /// <inheritdoc/>
            public override string ToString() => $"Sn({CellX}, {CellY})/{(int) Pos}";
        }
    }
}
