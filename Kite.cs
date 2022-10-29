using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a kite-shaped cell in a <see cref="Grid"/>. The “spikier” vertex of the kite is the center of a hexagon;
    ///     the sides adjacent are spokes connecting with the midpoints of the hexagon’s outline; and the rest form the
    ///     outline.</summary>
    public struct Kite : IEquatable<Kite>, INeighbor<Kite>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>The underlying hex tile. This kite forms one sixth of that hexagon.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which of the kites within the hexagon this is.</summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="hex">
        ///     A hex tile to construct a kite from.</param>
        /// <param name="pos">
        ///     Which of the kites within the hexagon to construct.</param>
        public Kite(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="q">
        ///     The Q coordinate of the hex tile to construct a kite from.</param>
        /// <param name="r">
        ///     The R coordinate of the hex tile to construct a kite from.</param>
        /// <param name="pos">
        ///     Which of the kites within the hexagon to construct.</param>
        public Kite(int q, int r, Position pos)
        {
            Hex = new Hex(q, r);
            Pos = pos;
        }

        /// <inheritdoc/>
        public bool Equals(Kite other) => other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Kite other && other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(Hex.GetHashCode() * 7 + (int) Pos);

        /// <summary>Equality operator.</summary>
        public static bool operator ==(Kite one, Kite two) => one.Equals(two);
        /// <summary>Inequality operator.</summary>
        public static bool operator !=(Kite one, Kite two) => !one.Equals(two);

        /// <inheritdoc/>
        public IEnumerable<Kite> Neighbors
        {
            get
            {
                yield return new Kite(Hex, (Position) (((int) Pos + 1) % 6));
                yield return new Kite(Hex, (Position) (((int) Pos + 5) % 6));
                yield return new Kite(Hex.Move((Hex.Direction) (((int) Pos + 1) % 6)), (Position) (((int) Pos + 2) % 6));
                yield return new Kite(Hex.Move((Hex.Direction) (((int) Pos + 2) % 6)), (Position) (((int) Pos + 4) % 6));
            }
        }

        IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Kite"/>, going clockwise from the “spiky” vertex
        ///     (center of <see cref="Hex"/>).</summary>
        public Coordinates.Vertex[] Vertices => new Coordinates.Vertex[]
        {
            new Vertex(Hex, Vertex.Position.Center),
            new Vertex(Hex, fullPos: 2 * (int) Pos),
            new Vertex(Hex, fullPos: 2 * (int) Pos + 1),
            new Vertex(Hex, fullPos: 2 * (int) Pos + 2)
        };

        /// <inheritdoc/>
        public PointD Center => Hex.Center * 2 + new PointD(0, -.6).Rotate(((int) Pos + .5) * Math.PI / 3);

        /// <summary>Identifies one of the <see cref="Kite"/> cells that make up a hexagon.</summary>
        public enum Position
        {
            /// <summary>The upper-right (1 o’clock) kite.</summary>
            TopRight,
            /// <summary>The right (3 o’clock) kite.</summary>
            Right,
            /// <summary>The lower-right (5 o’clock) kite.</summary>
            BottomRight,
            /// <summary>The lower-left (7 o’clock) kite.</summary>
            BottomLeft,
            /// <summary>The left (9 o’clock) kite.</summary>
            Left,
            /// <summary>The upper-left (11 o’clock) kite.</summary>
            TopLeft
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Hex}/{(int) Pos}";

        /// <summary>
        ///     Describes a grid structure consisting of <see cref="Kite"/> cells that join up in groups of 6 to form
        ///     hexagons, which in turn tile the plane.</summary>
        public class Grid : Structure<Kite>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Kite> cells, IEnumerable<Link<Kite>> links = null, Func<Kite, IEnumerable<Kite>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> consisting of a hexagonal grid of the specified <paramref
            ///     name="sideLength"/>.</summary>
            public Grid(int sideLength)
                : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _kitePositions.Select(pos => new Kite(hex, pos))))
            {
            }

            private static readonly Position[] _kitePositions = (Position[]) Enum.GetValues(typeof(Position));

            /// <inheritdoc/>
            protected override Structure<Kite> makeModifiedStructure(IEnumerable<Kite> cells, IEnumerable<Link<Kite>> traversible) => new Grid(cells, traversible);

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

        /// <summary>Describes one of the vertices of a <see cref="Kite"/>.</summary>
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
            ///     Position of the vertex counting clockwise from the vertex at the midpoint of the hexagon’s top edge. Note
            ///     that even numbers yield midpoints, odd numbers yield vertices of the hexagon.</param>
            /// <remarks>
            ///     Note that this constructor cannot construct a vertex that is at the center of a hexagon. Use <see
            ///     cref="Vertex(Hex, Position)"/> for that.</remarks>
            public Vertex(Hex hex, int fullPos)
            {
                switch ((fullPos % 12 + 12) % 12)
                {
                    case 5: Hex = hex.Move(Hex.Direction.Down); Pos = Position.UpRight; break;
                    case 6: Hex = hex.Move(Hex.Direction.Down); Pos = Position.MidUp; break;
                    case 7: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.Right; break;
                    case 8: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.MidUpRight; break;
                    case 9: Hex = hex.Move(Hex.Direction.DownLeft); Pos = Position.UpRight; break;
                    case 10: Hex = hex.Move(Hex.Direction.UpLeft); Pos = Position.MidDownRight; break;
                    case 11: Hex = hex.Move(Hex.Direction.UpLeft); Pos = Position.Right; break;

                    default:    // <= 4
                        Pos = (Position) ((fullPos % 12 + 12) % 12);
                        Hex = hex;
                        break;
                }
            }

            /// <summary>
            ///     Describes the position of a <see cref="Vertex"/> in relation to the vertices of its containing <see
            ///     cref="Hex"/>.</summary>
            public enum Position
            {
                /// <summary>Midpoint of the upper edge of the hex.</summary>
                MidUp,
                /// <summary>Upper-right vertex of the hex.</summary>
                UpRight,
                /// <summary>Midpoint of the upper-right edge of the hex.</summary>
                MidUpRight,
                /// <summary>Right vertex of the hex.</summary>
                Right,
                /// <summary>Midpoint of the lower-right edge of the hex.</summary>
                MidDownRight,
                /// <summary>Centerpoint of the hex.</summary>
                Center
            }

            private static readonly double[] xs = { 0, .25, .375, .5, .375, 0 };
            private static readonly double[] ys = { -.5, -.5, -.25, 0, .25, 0 };

            /// <inheritdoc/>
            public override double X => (Hex.Q * .75 + xs[(int) Pos]) * 2;
            /// <inheritdoc/>
            public override double Y => (Hex.Q * .5 + Hex.R + ys[(int) Pos]) * Hex.WidthToHeight * 2;

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => Hex.GetHashCode() * 6 + (int) Pos;
        }
    }
}
