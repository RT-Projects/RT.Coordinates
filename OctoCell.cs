using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Represents a cell in an <see cref="Grid"/>. Each cell may be an octagon or a square.</summary>
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
        public static IEnumerable<OctoCell> LargeRectangle(int width, int height)
        {
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    yield return new OctoCell(x, y, false);
                    if (x < width - 1 && y < height - 1)
                        yield return new OctoCell(x, y, true);
                }
        }

        /// <inheritdoc/>
        public bool Equals(OctoCell other) => other.X == X && other.Y == Y && other.IsSquare == IsSquare;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is OctoCell oc && Equals(oc);
        /// <inheritdoc/>
        public override int GetHashCode() => X * 1073741827 + Y * 47 + (IsSquare ? 1 : 0);

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="OctoCell"/>, going clockwise from the top-left
        ///     (octagon) or top (square).</summary>
        public Coordinates.Vertex[] Vertices => IsSquare
            ? new Coordinates.Vertex[] { new Vertex(X + 1, Y, 0), new Vertex(X + 1, Y + 1, 2), new Vertex(X + 1, Y + 1, 1), new Vertex(X, Y + 1, 3) }
            : new Coordinates.Vertex[] { new Vertex(X, Y, 2), new Vertex(X, Y, 3), new Vertex(X + 1, Y, 1), new Vertex(X + 1, Y, 0), new Vertex(X, Y + 1, 3), new Vertex(X, Y + 1, 2), new Vertex(X, Y, 0), new Vertex(X, Y, 1) };

        /// <inheritdoc/>
        public PointD Center => IsSquare ? new PointD(X + 1, Y + 1) : new PointD(X + .5, Y + .5);

        /// <inheritdoc/>
        public IEnumerable<OctoCell> Neighbors
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

        IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public override string ToString() => string.Format(IsSquare ? "[{0},{1}]" : "({0},{1})", X, Y);

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
                : base(LargeRectangle(width, height))
            {
            }

            /// <inheritdoc/>
            protected override Structure<OctoCell> makeModifiedStructure(IEnumerable<OctoCell> cells, IEnumerable<Link<OctoCell>> traversible) => new Grid(cells, traversible);

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

        /// <summary>Describes a vertex (gridline intersection) in an <see cref="Grid"/>.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>Specifies the x-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
            public int CellX { get; private set; }
            /// <summary>Specifies the y-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
            public int CellY { get; private set; }
            /// <summary>Specifies which of the four vertices along the left and top edge of an octagon this is (0–3 clockwise).</summary>
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
            public override double X => CellX + xs[Pos];
            /// <inheritdoc/>
            public override double Y => CellY + ys[Pos];

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => CellX * 1073741827 + CellY * 47 + Pos;
        }
    }
}
