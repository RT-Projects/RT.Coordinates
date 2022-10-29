using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a <see cref="Grid"/>. Each floret is a pentagon with one vertex “spikier” than the others.
    ///     That vertex can be thought of as the center of a hexagon, while the vertex two clockwise from that is a vertex of
    ///     the same hexagon. The remaining vertices are off from the hexagon’s edge but in such a way that 6 florets make a
    ///     flower-like shape which tiles the plane in a hexagonal pattern. The flatter part of each pentagon has internal
    ///     angles of 120°.</summary>
    public struct Floret : IEquatable<Floret>, INeighbor<Floret>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>The underlying hex tile. This floret forms one sixth of that hexagon.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which of the florets within the hexagon this is.</summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="hex">
        ///     A hex tile to construct a floret from.</param>
        /// <param name="pos">
        ///     Which of the florets within the hexagon to construct.</param>
        public Floret(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="q">
        ///     The Q coordinate of the hex tile to construct a floret from.</param>
        /// <param name="r">
        ///     The R coordinate of the hex tile to construct a floret from.</param>
        /// <param name="pos">
        ///     Which of the florets within the hexagon to construct.</param>
        public Floret(int q, int r, Position pos)
        {
            Hex = new Hex(q, r);
            Pos = pos;
        }

        /// <inheritdoc/>
        public bool Equals(Floret other) => other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Floret other && other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(Hex.GetHashCode() * 7 + (int) Pos);

        /// <summary>Equality operator.</summary>
        public static bool operator ==(Floret one, Floret two) => one.Equals(two);
        /// <summary>Inequality operator.</summary>
        public static bool operator !=(Floret one, Floret two) => !one.Equals(two);

        /// <inheritdoc/>
        public IEnumerable<Floret> Neighbors
        {
            get
            {
                yield return new Floret(Hex, (Position) (((int) Pos + 1) % 6));
                yield return new Floret(Hex, (Position) (((int) Pos + 5) % 6));
                yield return new Floret(Hex.Move((Hex.Direction) (((int) Pos + 1) % 6)), (Position) (((int) Pos + 2) % 6));
                yield return new Floret(Hex.Move((Hex.Direction) (((int) Pos + 2) % 6)), (Position) (((int) Pos + 4) % 6));
                yield return new Floret(Hex.Move((Hex.Direction) (((int) Pos + 2) % 6)), (Position) (((int) Pos + 3) % 6));
            }
        }

        IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Floret"/>, going clockwise from the “spiky” vertex
        ///     (center of <see cref="Hex"/>).</summary>
        public Coordinates.Vertex[] Vertices => new Coordinates.Vertex[]
        {
            new Vertex(Hex, Vertex.Position.Center),
            new Vertex(Hex, fullPos: 3 * (int) Pos + 1),
            new Vertex(Hex, fullPos: 3 * (int) Pos + 2),
            new Vertex(Hex, fullPos: 3 * (int) Pos + 3),
            new Vertex(Hex, fullPos: 3 * (int) Pos + 4)
        };

        /// <inheritdoc/>
        public PointD Center => Hex.Center * 3 + new PointD(0, -.9).Rotate(((int) Pos + 2d / 3) * Math.PI / 3);

        /// <summary>Identifies one of the <see cref="Floret"/> cells that make up a hexagon.</summary>
        public enum Position
        {
            /// <summary>The upper-right (1 o’clock) floret.</summary>
            TopRight,
            /// <summary>The right (3 o’clock) floret.</summary>
            Right,
            /// <summary>The lower-right (5 o’clock) floret.</summary>
            BottomRight,
            /// <summary>The lower-left (7 o’clock) floret.</summary>
            BottomLeft,
            /// <summary>The left (9 o’clock) floret.</summary>
            Left,
            /// <summary>The upper-left (11 o’clock) floret.</summary>
            TopLeft
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Hex}/{(int) Pos}";

        /// <summary>
        ///     Describes a grid structure consisting of <see cref="Floret"/> cells that join up in groups of 6 to form
        ///     hexagons, which in turn tile the plane.</summary>
        public class Grid : Structure<Floret>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Floret> cells, IEnumerable<Link<Floret>> links = null, Func<Floret, IEnumerable<Floret>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> consisting of a hexagonal grid of the specified <paramref
            ///     name="sideLength"/>.</summary>
            public Grid(int sideLength)
                : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _floretPositions.Select(pos => new Floret(hex, pos))))
            {
            }

            private static readonly Position[] _floretPositions = (Position[]) Enum.GetValues(typeof(Position));

            /// <inheritdoc/>
            protected override Structure<Floret> makeModifiedStructure(IEnumerable<Floret> cells, IEnumerable<Link<Floret>> traversible) => new Grid(cells, traversible);

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

        /// <summary>Describes one of the vertices of a <see cref="Floret"/>.</summary>
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
            ///     Constructs a <see cref="Vertex"/> from any position along the perimeter of <paramref name="hex"/>.</summary>
            /// <param name="hex">
            ///     Hexagon to compute vertex from.</param>
            /// <param name="fullPos">
            ///     Position of the vertex counting clockwise from the vertex that is above the hexagon’s top edge. Note that
            ///     numbers that are 1 modulo 3 yield vertices of the hexagon.</param>
            /// <remarks>
            ///     Note that this constructor cannot construct a vertex that is at the center of a hexagon. Use <see
            ///     cref="Vertex(Hex, Position)"/> for that.</remarks>
            public Vertex(Hex hex, int fullPos)
            {
                switch ((fullPos % 18 + 18) % 18)
                {
                    case 8: Hex = hex.Move(Hex.Direction.Down); Pos = Position.TopRight; break;
                    case 9: Hex = hex.Move(Hex.Direction.Down); Pos = Position.TopRightMinus1; break;
                    case 10: Hex = hex.Move(Hex.Direction.Down); Pos = Position.TopLeftPlus1; break;
                    case 11: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.Right; break;
                    case 12: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.RightMinus1; break;
                    case 13: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.TopRightPlus1; break;
                    case 14: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.TopRight; break;
                    case 15: Hex = hex.Move(Hex.Direction.UpLeft); Pos = Position.BottomRightMinus1; break;
                    case 16: Hex = hex.Move(Hex.Direction.UpLeft); Pos = Position.RightPlus1; break;
                    case 17: Hex = hex.Move(Hex.Direction.UpLeft); Pos = Position.Right; break;

                    default:    // <= 7
                        Pos = (Position) ((fullPos % 18 + 18) % 18);
                        Hex = hex;
                        break;
                }
            }

            /// <summary>
            ///     Describes the position of a <see cref="Vertex"/> in relation to the vertices of its referenced <see
            ///     cref="Hex"/>.</summary>
            public enum Position
            {
                /// <summary>The vertex one clockwise from the top-left vertex of the referenced <see cref="Hex"/>.</summary>
                TopLeftPlus1,
                /// <summary>The vertex one counter-clockwise from <see cref="TopRight"/>.</summary>
                TopRightMinus1,
                /// <summary>The top-right vertex of the referenced <see cref="Hex"/>.</summary>
                TopRight,
                /// <summary>The vertex one clockwise from <see cref="TopRight"/>.</summary>
                TopRightPlus1,
                /// <summary>The vertex one counter-clockwise from <see cref="Right"/>.</summary>
                RightMinus1,
                /// <summary>The right vertex of the referenced <see cref="Hex"/>.</summary>
                Right,
                /// <summary>The vertex one clockwise from <see cref="Right"/>.</summary>
                RightPlus1,
                /// <summary>
                ///     The vertex one counter-clockwise from the bottom-right vertex of the referenced <see cref="Hex"/>.</summary>
                BottomRightMinus1,
                /// <summary>The vertex at the center of the referenced <see cref="Hex"/>.</summary>
                Center
            }

            private static readonly double[] xs = { -0.0714285714285714, 0.0714285714285714, 0.25, 0.392857142857143, 0.357142857142857, 0.5, 0.464285714285714, 0.285714285714286, 0 };
            private static readonly double[] ys = { -0.494871659305394, -0.371153744479045, -0.433012701892219, -0.309294787065871, -0.123717914826348, 0, 0.185576872239522, 0.247435829652697, 0 };

            /// <inheritdoc/>
            public override double X => (Hex.Q * .75 + xs[(int) Pos]) * 3;
            /// <inheritdoc/>
            public override double Y => ((Hex.Q * .5 + Hex.R) * Hex.WidthToHeight + ys[(int) Pos]) * 3;

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => Hex.GetHashCode() * 6 + (int) Pos;
        }
    }
}
