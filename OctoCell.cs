using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a cell in an <see cref="Grid"/>. Each cell may be an octagon or a square.</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='0.5 0.5 7 7'&gt;&lt;path d='M1 0.292893218813452L1
    ///     0.707106781186548L0.707106781186548 1L0.292893218813452 1M1 0.707106781186548L1.29289321881345 1L1
    ///     1.29289321881345L0.707106781186548 1M1 1.29289321881345L1 1.70710678118655L0.707106781186548 2L0.292893218813452
    ///     2M1 1.70710678118655L1.29289321881345 2L1 2.29289321881345L0.707106781186548 2M1 2.29289321881345L1
    ///     2.70710678118655L0.707106781186548 3L0.292893218813452 3M1 2.70710678118655L1.29289321881345 3L1
    ///     3.29289321881345L0.707106781186548 3M1 3.29289321881345L1 3.70710678118655L0.707106781186548 4L0.292893218813452
    ///     4M1 3.70710678118655L1.29289321881345 4L1 4.29289321881345L0.707106781186548 4M1 4.29289321881345L1
    ///     4.70710678118655L0.707106781186548 5L0.292893218813452 5M1 4.70710678118655L1.29289321881345 5L1
    ///     5.29289321881345L0.707106781186548 5M1 5.29289321881345L1 5.70710678118655L0.707106781186548 6L0.292893218813452
    ///     6M1 5.70710678118655L1.29289321881345 6L1 6.29289321881345L0.707106781186548 6M1 6.29289321881345L1
    ///     6.70710678118655L0.707106781186548 7L0.292893218813452 7M1 6.70710678118655L1.29289321881345 7L1
    ///     7.29289321881345L0.707106781186548 7M1 7.29289321881345L1 7.70710678118655M2 0.292893218813452L2
    ///     0.707106781186548L1.70710678118655 1L1.29289321881345 1M2 0.707106781186548L2.29289321881345 1L2
    ///     1.29289321881345L1.70710678118655 1M2 1.29289321881345L2 1.70710678118655L1.70710678118655 2L1.29289321881345 2M2
    ///     1.70710678118655L2.29289321881345 2L2 2.29289321881345L1.70710678118655 2M2 2.29289321881345L2
    ///     2.70710678118655L1.70710678118655 3L1.29289321881345 3M2 2.70710678118655L2.29289321881345 3L2
    ///     3.29289321881345L1.70710678118655 3M2 3.29289321881345L2 3.70710678118655L1.70710678118655 4L1.29289321881345 4M2
    ///     3.70710678118655L2.29289321881345 4L2 4.29289321881345L1.70710678118655 4M2 4.29289321881345L2
    ///     4.70710678118655L1.70710678118655 5L1.29289321881345 5M2 4.70710678118655L2.29289321881345 5L2
    ///     5.29289321881345L1.70710678118655 5M2 5.29289321881345L2 5.70710678118655L1.70710678118655 6L1.29289321881345 6M2
    ///     5.70710678118655L2.29289321881345 6L2 6.29289321881345L1.70710678118655 6M2 6.29289321881345L2
    ///     6.70710678118655L1.70710678118655 7L1.29289321881345 7M2 6.70710678118655L2.29289321881345 7L2
    ///     7.29289321881345L1.70710678118655 7M2 7.29289321881345L2 7.70710678118655M3 0.292893218813452L3
    ///     0.707106781186548L2.70710678118655 1L2.29289321881345 1M3 0.707106781186548L3.29289321881345 1L3
    ///     1.29289321881345L2.70710678118655 1M3 1.29289321881345L3 1.70710678118655L2.70710678118655 2L2.29289321881345 2M3
    ///     1.70710678118655L3.29289321881345 2L3 2.29289321881345L2.70710678118655 2M3 2.29289321881345L3
    ///     2.70710678118655L2.70710678118655 3L2.29289321881345 3M3 2.70710678118655L3.29289321881345 3L3
    ///     3.29289321881345L2.70710678118655 3M3 3.29289321881345L3 3.70710678118655L2.70710678118655 4L2.29289321881345 4M3
    ///     3.70710678118655L3.29289321881345 4L3 4.29289321881345L2.70710678118655 4M3 4.29289321881345L3
    ///     4.70710678118655L2.70710678118655 5L2.29289321881345 5M3 4.70710678118655L3.29289321881345 5L3
    ///     5.29289321881345L2.70710678118655 5M3 5.29289321881345L3 5.70710678118655L2.70710678118655 6L2.29289321881345 6M3
    ///     5.70710678118655L3.29289321881345 6L3 6.29289321881345L2.70710678118655 6M3 6.29289321881345L3
    ///     6.70710678118655L2.70710678118655 7L2.29289321881345 7M3 6.70710678118655L3.29289321881345 7L3
    ///     7.29289321881345L2.70710678118655 7M3 7.29289321881345L3 7.70710678118655M4 0.292893218813452L4
    ///     0.707106781186548L3.70710678118655 1L3.29289321881345 1M4 0.707106781186548L4.29289321881345 1L4
    ///     1.29289321881345L3.70710678118655 1M4 1.29289321881345L4 1.70710678118655L3.70710678118655 2L3.29289321881345 2M4
    ///     1.70710678118655L4.29289321881345 2L4 2.29289321881345L3.70710678118655 2M4 2.29289321881345L4
    ///     2.70710678118655L3.70710678118655 3L3.29289321881345 3M4 2.70710678118655L4.29289321881345 3L4
    ///     3.29289321881345L3.70710678118655 3M4 3.29289321881345L4 3.70710678118655L3.70710678118655 4L3.29289321881345 4M4
    ///     3.70710678118655L4.29289321881345 4L4 4.29289321881345L3.70710678118655 4M4 4.29289321881345L4
    ///     4.70710678118655L3.70710678118655 5L3.29289321881345 5M4 4.70710678118655L4.29289321881345 5L4
    ///     5.29289321881345L3.70710678118655 5M4 5.29289321881345L4 5.70710678118655L3.70710678118655 6L3.29289321881345 6M4
    ///     5.70710678118655L4.29289321881345 6L4 6.29289321881345L3.70710678118655 6M4 6.29289321881345L4
    ///     6.70710678118655L3.70710678118655 7L3.29289321881345 7M4 6.70710678118655L4.29289321881345 7L4
    ///     7.29289321881345L3.70710678118655 7M4 7.29289321881345L4 7.70710678118655M5 0.292893218813452L5
    ///     0.707106781186548L4.70710678118655 1L4.29289321881345 1M5 0.707106781186548L5.29289321881345 1L5
    ///     1.29289321881345L4.70710678118655 1M5 1.29289321881345L5 1.70710678118655L4.70710678118655 2L4.29289321881345 2M5
    ///     1.70710678118655L5.29289321881345 2L5 2.29289321881345L4.70710678118655 2M5 2.29289321881345L5
    ///     2.70710678118655L4.70710678118655 3L4.29289321881345 3M5 2.70710678118655L5.29289321881345 3L5
    ///     3.29289321881345L4.70710678118655 3M5 3.29289321881345L5 3.70710678118655L4.70710678118655 4L4.29289321881345 4M5
    ///     3.70710678118655L5.29289321881345 4L5 4.29289321881345L4.70710678118655 4M5 4.29289321881345L5
    ///     4.70710678118655L4.70710678118655 5L4.29289321881345 5M5 4.70710678118655L5.29289321881345 5L5
    ///     5.29289321881345L4.70710678118655 5M5 5.29289321881345L5 5.70710678118655L4.70710678118655 6L4.29289321881345 6M5
    ///     5.70710678118655L5.29289321881345 6L5 6.29289321881345L4.70710678118655 6M5 6.29289321881345L5
    ///     6.70710678118655L4.70710678118655 7L4.29289321881345 7M5 6.70710678118655L5.29289321881345 7L5
    ///     7.29289321881345L4.70710678118655 7M5 7.29289321881345L5 7.70710678118655M6 0.292893218813452L6
    ///     0.707106781186548L5.70710678118655 1L5.29289321881345 1M6 0.707106781186548L6.29289321881345 1L6
    ///     1.29289321881345L5.70710678118655 1M6 1.29289321881345L6 1.70710678118655L5.70710678118655 2L5.29289321881345 2M6
    ///     1.70710678118655L6.29289321881345 2L6 2.29289321881345L5.70710678118655 2M6 2.29289321881345L6
    ///     2.70710678118655L5.70710678118655 3L5.29289321881345 3M6 2.70710678118655L6.29289321881345 3L6
    ///     3.29289321881345L5.70710678118655 3M6 3.29289321881345L6 3.70710678118655L5.70710678118655 4L5.29289321881345 4M6
    ///     3.70710678118655L6.29289321881345 4L6 4.29289321881345L5.70710678118655 4M6 4.29289321881345L6
    ///     4.70710678118655L5.70710678118655 5L5.29289321881345 5M6 4.70710678118655L6.29289321881345 5L6
    ///     5.29289321881345L5.70710678118655 5M6 5.29289321881345L6 5.70710678118655L5.70710678118655 6L5.29289321881345 6M6
    ///     5.70710678118655L6.29289321881345 6L6 6.29289321881345L5.70710678118655 6M6 6.29289321881345L6
    ///     6.70710678118655L5.70710678118655 7L5.29289321881345 7M6 6.70710678118655L6.29289321881345 7L6
    ///     7.29289321881345L5.70710678118655 7M6 7.29289321881345L6 7.70710678118655M7 0.292893218813452L7
    ///     0.707106781186548L6.70710678118655 1L6.29289321881345 1M7 0.707106781186548L7.29289321881345 1L7
    ///     1.29289321881345L6.70710678118655 1M7 1.29289321881345L7 1.70710678118655L6.70710678118655 2L6.29289321881345 2M7
    ///     1.70710678118655L7.29289321881345 2L7 2.29289321881345L6.70710678118655 2M7 2.29289321881345L7
    ///     2.70710678118655L6.70710678118655 3L6.29289321881345 3M7 2.70710678118655L7.29289321881345 3L7
    ///     3.29289321881345L6.70710678118655 3M7 3.29289321881345L7 3.70710678118655L6.70710678118655 4L6.29289321881345 4M7
    ///     3.70710678118655L7.29289321881345 4L7 4.29289321881345L6.70710678118655 4M7 4.29289321881345L7
    ///     4.70710678118655L6.70710678118655 5L6.29289321881345 5M7 4.70710678118655L7.29289321881345 5L7
    ///     5.29289321881345L6.70710678118655 5M7 5.29289321881345L7 5.70710678118655L6.70710678118655 6L6.29289321881345 6M7
    ///     5.70710678118655L7.29289321881345 6L7 6.29289321881345L6.70710678118655 6M7 6.29289321881345L7
    ///     6.70710678118655L6.70710678118655 7L6.29289321881345 7M7 6.70710678118655L7.29289321881345 7L7
    ///     7.29289321881345L6.70710678118655 7M7 7.29289321881345L7 7.70710678118655M7.70710678118655 1L7.29289321881345
    ///     1M7.70710678118655 2L7.29289321881345 2M7.70710678118655 3L7.29289321881345 3M7.70710678118655 4L7.29289321881345
    ///     4M7.70710678118655 5L7.29289321881345 5M7.70710678118655 6L7.29289321881345 6M7.70710678118655 7L7.29289321881345
    ///     7' fill='none' stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    public struct OctoCell : IEquatable<OctoCell>, INeighbor<OctoCell>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>X-coordinate of the cell.</summary>
        public int X { get; private set; }
        /// <summary>Y-coordinate of the cell.</summary>
        public int Y { get; private set; }
        /// <summary>
        ///     Specifies whether this cell is a square or an octagon. If it is a square, it is south-east of the octagon with
        ///     the same coordinates.</summary>
        public bool IsSquare { get; private set; }

        /// <summary>Constructor.</summary>
        public OctoCell(int x, int y, bool isSquare)
        {
            X = x;
            Y = y;
            IsSquare = isSquare;
        }

        /// <summary>
        ///     Returns a set of <see cref="OctoCell"/> cells that form a rectangle. Along the perimeter all the cells will be
        ///     octagons. Only squares internal to these octagons are included.</summary>
        /// <param name="width">
        ///     The number of octagons in the x direction.</param>
        /// <param name="height">
        ///     The number of octagons in the y direction.</param>
        public static IEnumerable<OctoCell> Rectangle(int width, int height)
        {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    yield return new OctoCell(x, y, false);
                    if (x < width - 1 && y < height - 1)
                        yield return new OctoCell(x, y, true);
                }
        }

        /// <inheritdoc/>
        public readonly bool Equals(OctoCell other) => other.X == X && other.Y == Y && other.IsSquare == IsSquare;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is OctoCell oc && Equals(oc);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => X * 1073741827 + Y * 47 + (IsSquare ? 1 : 0);

        /// <inheritdoc/>
        public readonly IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="OctoCell"/>, going clockwise from the top-left
        ///     (octagon) or top (square).</summary>
        public readonly Coordinates.Vertex[] Vertices => IsSquare
            ? new Coordinates.Vertex[] { new Vertex(X + 1, Y, 0), new Vertex(X + 1, Y + 1, 2), new Vertex(X + 1, Y + 1, 1), new Vertex(X, Y + 1, 3) }
            : new Coordinates.Vertex[] { new Vertex(X, Y, 2), new Vertex(X, Y, 3), new Vertex(X + 1, Y, 1), new Vertex(X + 1, Y, 0), new Vertex(X, Y + 1, 3), new Vertex(X, Y + 1, 2), new Vertex(X, Y, 0), new Vertex(X, Y, 1) };

        /// <inheritdoc/>
        public readonly PointD Center => IsSquare ? new PointD(X + 1, Y + 1) : new PointD(X + .5, Y + .5);

        /// <inheritdoc/>
        public readonly IEnumerable<OctoCell> Neighbors
        {
            get
            {
                if (IsSquare)
                {
                    yield return new OctoCell(X, Y, false);
                    yield return new OctoCell(X + 1, Y, false);
                    yield return new OctoCell(X + 1, Y + 1, false);
                    yield return new OctoCell(X, Y + 1, false);
                }
                else
                {
                    yield return new OctoCell(X, Y - 1, true);
                    yield return new OctoCell(X, Y - 1, true);
                    yield return new OctoCell(X + 1, Y, false);
                    yield return new OctoCell(X, Y, true);
                    yield return new OctoCell(X, Y + 1, false);
                    yield return new OctoCell(X - 1, Y, true);
                    yield return new OctoCell(X - 1, Y, false);
                }
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public override readonly string ToString() => string.Format("{0}({1},{2})", IsSquare ? "o" : "O", X, Y);

        /// <summary>Describes a 2D grid of octagonal cells with (diagonal) squares filling the gaps.</summary>
        public class Grid : Structure<OctoCell>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<OctoCell> cells, IEnumerable<Link<OctoCell>> links = null, Func<OctoCell, IEnumerable<OctoCell>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs an <see cref="Grid"/> that forms a rectangle. Along the perimeter all the cells will be
            ///     octagons. Only squares internal to these octagons are included.</summary>
            /// <param name="width">
            ///     The number of octagons in the x direction.</param>
            /// <param name="height">
            ///     The number of octagons in the y direction.</param>
            public Grid(int width, int height)
                : base(Rectangle(width, height))
            {
            }

            /// <inheritdoc/>
            protected override Structure<OctoCell> makeModifiedStructure(IEnumerable<OctoCell> cells, IEnumerable<Link<OctoCell>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }

        /// <summary>Describes a vertex (gridline intersection) in an <see cref="Grid"/>.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>Specifies the x-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
            public int CellX { get; private set; }
            /// <summary>Specifies the y-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
            public int CellY { get; private set; }
            /// <summary>
            ///     Specifies which of the four vertices along the left and top edge of an octagon this is (0–3 clockwise).</summary>
            public int Pos { get; private set; }

            /// <summary>Constructor.</summary>
            public Vertex(int x, int y, int topPos)
            {
                if (topPos < 0 || topPos > 3)
                    throw new ArgumentException($"‘{topPos}’ is not a valid value for ‘{nameof(topPos)}’ (0–3 expected).", nameof(topPos));
                CellX = x;
                CellY = y;
                Pos = topPos;
            }

            private const double D1 = .29289321881345247559915563789515096071516406231155;   // 1 - 1/√2
            private const double D2 = .70710678118654752440084436210484903928483593768845;   // 1/√2
            private static readonly double[] xs = { 0, 0, D1, D2 };
            private static readonly double[] ys = { D2, D1, 0, 0 };

            /// <inheritdoc/>
            public override PointD Point => new(CellX + xs[Pos], CellY + ys[Pos]);

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => CellX * 1073741827 + CellY * 47 + Pos;
        }
    }
}
