using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Describes a cell in a <see cref="Grid"/>. Three cells of this kind form a hexagon, which in turn tiles the
    ///         plane.</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.25 -3.06698729810778 7 7'&gt;&lt;path d='M4
    ///     -3.46410161513775L3.5 -2.59807621135332L2.5 -2.59807621135332L3 -3.46410161513775M2 -3.46410161513775L2.5
    ///     -2.59807621135332L1.5 -2.59807621135332L1 -3.46410161513775L0.5 -2.59807621135332L-0.5 -2.59807621135332L0
    ///     -3.46410161513775M3.5 -2.59807621135332L4 -1.73205080756888L3 -1.73205080756888L2.5 -2.59807621135332L2
    ///     -1.73205080756888L1 -1.73205080756888L1.5 -2.59807621135332M0.5 -2.59807621135332L1 -1.73205080756888L0
    ///     -1.73205080756888L-0.5 -2.59807621135332L-1 -1.73205080756888L-2 -1.73205080756888L-1.5 -2.59807621135332M4
    ///     -1.73205080756888L3.5 -0.866025403784439L2.5 -0.866025403784439L3 -1.73205080756888M2 -1.73205080756888L2.5
    ///     -0.866025403784439L1.5 -0.866025403784439L1 -1.73205080756888L0.5 -0.866025403784439L-0.5 -0.866025403784439L0
    ///     -1.73205080756888M3.5 -0.866025403784439L4 0L3 0L2.5 -0.866025403784439L2 0L1 0L1.5 -0.866025403784439M-2.5
    ///     -2.59807621135332L-2 -1.73205080756888L-3 -1.73205080756888L-3.5 -2.59807621135332L-2.5 -2.59807621135332L-2
    ///     -3.46410161513775L-1.5 -2.59807621135332L-0.5 -2.59807621135332L-1 -3.46410161513775M-1 -1.73205080756888L-0.5
    ///     -0.866025403784439L-1.5 -0.866025403784439L-2 -1.73205080756888L-2.5 -0.866025403784439L-3.5 -0.866025403784439L-3
    ///     -1.73205080756888M0.5 -0.866025403784439L1 0L0 0L-0.5 -0.866025403784439L-1 0L-2 0L-1.5 -0.866025403784439M4 0L3.5
    ///     0.866025403784439L2.5 0.866025403784439L3 0M2 0L2.5 0.866025403784439L1.5 0.866025403784439L1 0L0.5
    ///     0.866025403784439L-0.5 0.866025403784439L0 0M3.5 0.866025403784439L4 1.73205080756888L3 1.73205080756888L2.5
    ///     0.866025403784439L2 1.73205080756888L1 1.73205080756888L1.5 0.866025403784439M-2.5 -0.866025403784439L-2 0L-3
    ///     0L-3.5 -0.866025403784439M-1 0L-0.5 0.866025403784439L-1.5 0.866025403784439L-2 0L-2.5 0.866025403784439L-3.5
    ///     0.866025403784439L-3 0M0.5 0.866025403784439L1 1.73205080756888L0 1.73205080756888L-0.5 0.866025403784439L-1
    ///     1.73205080756888L-2 1.73205080756888L-1.5 0.866025403784439M4 1.73205080756888L3.5 2.59807621135332L2.5
    ///     2.59807621135332L3 1.73205080756888M2 1.73205080756888L2.5 2.59807621135332L1.5 2.59807621135332L1
    ///     1.73205080756888L0.5 2.59807621135332L-0.5 2.59807621135332L0 1.73205080756888M3.5 2.59807621135332L4
    ///     3.46410161513775L3 3.46410161513775L2.5 2.59807621135332L2 3.46410161513775L1 3.46410161513775L1.5
    ///     2.59807621135332M-2.5 0.866025403784439L-2 1.73205080756888L-3 1.73205080756888L-3.5 0.866025403784439M-1
    ///     1.73205080756888L-0.5 2.59807621135332L-1.5 2.59807621135332L-2 1.73205080756888L-2.5 2.59807621135332L-3.5
    ///     2.59807621135332L-3 1.73205080756888M0.5 2.59807621135332L1 3.46410161513775L0 3.46410161513775L-0.5
    ///     2.59807621135332L-1 3.46410161513775L-2 3.46410161513775L-1.5 2.59807621135332M2 3.46410161513775L2.5
    ///     4.33012701892219L3 3.46410161513775M-2.5 2.59807621135332L-2 3.46410161513775L-3 3.46410161513775L-3.5
    ///     2.59807621135332M1.5 4.33012701892219L1 3.46410161513775L0.5 4.33012701892219M-1 3.46410161513775L-0.5
    ///     4.33012701892219L0 3.46410161513775M-1.5 4.33012701892219L-2 3.46410161513775L-2.5 4.33012701892219M-3.5
    ///     4.33012701892219L-3 3.46410161513775' fill='none' stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    public struct Rhomb : IEquatable<Rhomb>, INeighbor<Rhomb>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>The underlying hex tile. This rhomb forms one third of that hexagon.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which of the rhombs within the hexagon this is.</summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="hex">
        ///     A hex tile to construct a rhomb from.</param>
        /// <param name="pos">
        ///     Which of the rhombs within the hexagon to construct.</param>
        public Rhomb(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="q">
        ///     The Q coordinate of the hex tile to construct a rhomb from.</param>
        /// <param name="r">
        ///     The R coordinate of the hex tile to construct a rhomb from.</param>
        /// <param name="pos">
        ///     Which of the rhombs within the hexagon to construct.</param>
        public Rhomb(int q, int r, Position pos)
        {
            Hex = new Hex(q, r);
            Pos = pos;
        }

        /// <summary>Identifies one of the <see cref="Rhomb"/> cells that make up a hexagon.</summary>
        public enum Position
        {
            /// <summary>The rhomb in the top-right of the hexagon.</summary>
            TopRight,
            /// <summary>The rhomb in the bottom-right of the hexagon.</summary>
            BottomRight,
            /// <summary>The rhomb at the left of the hexagon.</summary>
            Left
        }

        /// <inheritdoc/>
        public readonly bool Equals(Rhomb rhomb) => Hex.Equals(rhomb.Hex) && Pos == rhomb.Pos;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Rhomb rhomb && Hex.Equals(rhomb.Hex) && Pos == rhomb.Pos;
        /// <inheritdoc/>
        public override readonly int GetHashCode() => Hex.GetHashCode() * 3 + (int) Pos;

        /// <inheritdoc/>
        public readonly IEnumerable<Rhomb> Neighbors
        {
            get
            {
                switch (Pos)
                {
                    case Position.TopRight:
                        yield return new Rhomb(Hex.Move(Hex.Direction.Up), Position.BottomRight);
                        yield return new Rhomb(Hex.Move(Hex.Direction.UpRight), Position.Left);
                        yield return new Rhomb(Hex, Position.BottomRight);
                        yield return new Rhomb(Hex, Position.Left);
                        break;

                    case Position.BottomRight:
                        yield return new Rhomb(Hex, Position.TopRight);
                        yield return new Rhomb(Hex.Move(Hex.Direction.DownRight), Position.Left);
                        yield return new Rhomb(Hex.Move(Hex.Direction.Down), Position.TopRight);
                        yield return new Rhomb(Hex, Position.Left);
                        break;

                    case Position.Left:
                        yield return new Rhomb(Hex, Position.TopRight);
                        yield return new Rhomb(Hex, Position.BottomRight);
                        yield return new Rhomb(Hex.Move(Hex.Direction.DownLeft), Position.TopRight);
                        yield return new Rhomb(Hex.Move(Hex.Direction.UpLeft), Position.BottomRight);
                        break;

                    default:
                        throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.");
                }
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public readonly IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Rhomb"/>, going clockwise from the vertex at the
        ///     center of <see cref="Hex"/>.</summary>
        public readonly Coordinates.Vertex[] Vertices => Pos switch
        {
            Position.TopRight => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.Center),
                new Vertex(Hex, Vertex.Position.TopLeft),
                new Vertex(Hex, Vertex.Position.TopRight),
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.TopLeft)
            },
            Position.BottomRight => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.Center),
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.TopLeft),
                new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopRight),
                new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopLeft)
            },
            Position.Left => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.Center),
                new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopLeft),
                new Vertex(Hex.Move(Hex.Direction.DownLeft), Vertex.Position.TopRight),
                new Vertex(Hex, Vertex.Position.TopLeft)
            },
            _ => throw new InvalidOperationException($"{nameof(Pos)} has an invalid value of {Pos}.")
        };

        private static readonly PointD[] _ps = { new(.25, -.433012701892220), new(.25, .433012701892220), new(-.5, 0) };

        /// <inheritdoc/>
        public readonly PointD Center => Hex.Center * 2 + _ps[(int) Pos];

        private static readonly Position[] _rhombPositions = (Position[]) Enum.GetValues(typeof(Position));

        /// <summary>
        ///     Returns a collection of <see cref="Rhomb"/> tiles that form a larger hexagonal structure.</summary>
        /// <param name="sideLength">
        ///     Side length of the hexagon structure to produce.</param>
        public static IEnumerable<Rhomb> LargeHexagon(int sideLength) => Hex.LargeHexagon(sideLength).SelectMany(hex => _rhombPositions.Select(pos => new Rhomb(hex, pos)));

        /// <inheritdoc/>
        public override readonly string ToString() => $"R({Hex.Q},{Hex.R})/{(int) Pos}";

        /// <summary>
        ///     Describes a grid structure consisting of <see cref="Rhomb"/> cells that join up in groups of 3 to form
        ///     hexagons, which in turn tile the plane.</summary>
        public class Grid : Structure<Rhomb>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Rhomb> cells, IEnumerable<Link<Rhomb>> links = null, Func<Rhomb, IEnumerable<Rhomb>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> consisting of a hexagonal grid of the specified <paramref
            ///     name="sideLength"/>.</summary>
            public Grid(int sideLength) : base(LargeHexagon(sideLength))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Rhomb> makeModifiedStructure(IEnumerable<Rhomb> cells, IEnumerable<Link<Rhomb>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }

        /// <summary>Describes one of the vertices of a <see cref="Rhomb"/>.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>The <see cref="Hex"/> tile that this <see cref="Vertex"/> is within.</summary>
            public Hex Hex { get; private set; }
            /// <summary>Which position within the <see cref="Hex"/> this vertex is.</summary>
            public Position Pos { get; private set; }

            /// <summary>Constructor.</summary>
            public Vertex(Hex hex, Position pos)
            {
                Hex = hex;
                Pos = pos;
            }

            /// <summary>
            ///     Describes the position of a <see cref="Vertex"/> in relation to the vertices of its containing <see
            ///     cref="Hex"/>.</summary>
            public enum Position
            {
                /// <summary>Top-left vertex of the hex.</summary>
                TopLeft,
                /// <summary>Top-right vertex of the hex.</summary>
                TopRight,
                /// <summary>Centerpoint of the hex.</summary>
                Center
            }

            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex rv && Hex.Equals(rv.Hex) && Pos == rv.Pos;
            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex vertex) => vertex is Vertex rv && Hex.Equals(rv.Hex) && Pos == rv.Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => Hex.GetHashCode() * 13 + (int) Pos;

            /// <inheritdoc/>
            public override PointD Point => new(
                Hex.Q * 1.5 + (Pos switch { Position.TopLeft => -.5, Position.TopRight => .5, Position.Center => 0, _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.") }),
                Hex.WidthToHeight * (Hex.Q + Hex.R * 2 + (Pos switch { Position.TopLeft => -1, Position.TopRight => -1, Position.Center => 0, _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.") })));
        }
    }
}
