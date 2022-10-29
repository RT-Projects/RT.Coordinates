using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents a cell in a <see cref="Grid"/>. Each cairo is a pentagon. Four cairos form a hexagon that is either
    ///     horizontally or vertically stretched.</summary>
    public struct Cairo : IEquatable<Cairo>, INeighbor<Cairo>, IHasSvgGeometry
    {
        /// <summary>Identifies a hexagon. Each Cairo is one quarter of a hexagon.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Identifies which Cairo within <see cref="Hex"/> this is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public Cairo(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="q">
        ///     Q-coordinate of the underlying hexagon.</param>
        /// <param name="r">
        ///     R-coordinate of the underlying hexagon.</param>
        /// <param name="pos">
        ///     Position of the <see cref="Cairo"/> within the hexagon.</param>
        public Cairo(int q, int r, Position pos)
        {
            Hex = new Hex(q, r);
            Pos = pos;
        }

        /// <summary>Identifies one of the <see cref="Cairo"/> cells that make up a hexagon.</summary>
        public enum Position
        {
            /// <summary>The top <see cref="Cairo"/> within a hexagon.</summary>
            Top,
            /// <summary>The right <see cref="Cairo"/> within a hexagon.</summary>
            Right,
            /// <summary>The bottom <see cref="Cairo"/> within a hexagon.</summary>
            Bottom,
            /// <summary>The left <see cref="Cairo"/> within a hexagon.</summary>
            Left
        }

        /// <summary>
        ///     Constructs a hexagonal grid of the specified <paramref name="sideLength"/> and divides each hexagon into four
        ///     <see cref="Cairo"/> cells.</summary>
        public static IEnumerable<Cairo> LargeHexagon(int sideLength) => Hex.LargeHexagon(sideLength).SelectMany(hex => _cairoPositions.Select(pos => new Cairo(hex, pos)));
        private static readonly Position[] _cairoPositions = (Position[]) Enum.GetValues(typeof(Position));

        /// <inheritdoc/>
        public bool Equals(Cairo other) => other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Cairo other && other.Hex.Equals(Hex) && other.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 4 + (int) Pos;
        /// <summary>Equality operator.</summary>
        public static bool operator ==(Cairo one, Cairo two) => one.Equals(two);
        /// <summary>Inequality operator.</summary>
        public static bool operator !=(Cairo one, Cairo two) => !one.Equals(two);

        /// <inheritdoc/>
        public IEnumerable<Cairo> Neighbors
        {
            get
            {
                switch (Pos)
                {
                    case Position.Top:
                        yield return new Cairo(Hex.Move(Hex.Direction.Up), Position.Bottom);
                        yield return new Cairo(Hex.Move(Hex.Direction.UpRight), Position.Left);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex.Move(Hex.Direction.UpLeft), Position.Right);
                        break;

                    case Position.Right:
                        yield return new Cairo(Hex, Position.Bottom);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex, Position.Top);
                        yield return new Cairo(Hex.Move(Hex.Direction.UpRight), Position.Bottom);
                        yield return new Cairo(Hex.Move(Hex.Direction.DownRight), Position.Top);
                        break;

                    case Position.Bottom:
                        yield return new Cairo(Hex.Move(Hex.Direction.DownRight), Position.Left);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex.Move(Hex.Direction.DownLeft), Position.Right);
                        yield return new Cairo(Hex.Move(Hex.Direction.Down), Position.Top);
                        break;

                    case Position.Left:
                        yield return new Cairo(Hex, Position.Top);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Bottom);
                        yield return new Cairo(Hex.Move(Hex.Direction.DownLeft), Position.Top);
                        yield return new Cairo(Hex.Move(Hex.Direction.UpLeft), Position.Bottom);
                        break;

                    default:
                        throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.");
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Cairo"/>, going clockwise from the “tip” of the
        ///     pentagon (the vertex opposite the horizontal or vertical edge).</summary>
        public Coordinates.Vertex[] Vertices => Pos switch
        {
            Position.Top => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.CenterTop),
                new Vertex(Hex, Vertex.Position.MidTopLeft),
                new Vertex(Hex, Vertex.Position.TopLeft),
                new Vertex(Hex.Move(Hex.Direction.UpRight), Vertex.Position.Left),
                new Vertex(Hex.Move(Hex.Direction.UpRight), Vertex.Position.MidBottomLeft)
            },
            Position.Right => new Coordinates.Vertex[] {
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.TopLeft),
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.MidTopLeft),
                new Vertex(Hex, Vertex.Position.CenterBottom),
                new Vertex(Hex, Vertex.Position.CenterTop),
                new Vertex(Hex.Move(Hex.Direction.UpRight), Vertex.Position.MidBottomLeft)
            },
            Position.Bottom => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.CenterBottom),
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.MidTopLeft),
                new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.Left),
                new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopLeft),
                new Vertex(Hex, Vertex.Position.MidBottomLeft)
            },
            Position.Left => new Coordinates.Vertex[] {
                new Vertex(Hex, Vertex.Position.Left),
                new Vertex(Hex, Vertex.Position.MidTopLeft),
                new Vertex(Hex, Vertex.Position.CenterTop),
                new Vertex(Hex, Vertex.Position.CenterBottom),
                new Vertex(Hex, Vertex.Position.MidBottomLeft)
            },
            _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.")
        };

        private const double sqrt7 = 2.6457513110645905905016157536392604257102591830825;
        private static readonly double[] xs = { 0, sqrt7 / 4, 0, -sqrt7 / 4 };
        private static readonly double[] ys = { -sqrt7 / 4 - .5, 0, sqrt7 / 4 + .5, 0 };

        /// <inheritdoc/>
        public PointD Center => new PointD(Hex.Q * (sqrt7 + 1) / 2 + xs[(int) Pos], Hex.Q * (sqrt7 + 1) / 2 + Hex.R * (sqrt7 + 1) + ys[(int) Pos]);

        /// <inheritdoc/>
        public override string ToString() => $"{Hex};{(int) Pos}";

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
            ///     Constructs a <see cref="Grid"/> consisting of a hexagonal grid of the specified <paramref
            ///     name="sideLength"/>.</summary>
            public Grid(int sideLength)
                : base(Cairo.LargeHexagon(sideLength))
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
                /// <summary>Midpoint of the lower-left edge of the hex.</summary>
                MidBottomLeft,
                /// <summary>Left vertex of the hex.</summary>
                Left,
                /// <summary>Midpoint of the upper-left edge of the hex.</summary>
                MidTopLeft,
                /// <summary>Top-left vertex of the hex.</summary>
                TopLeft,
                /// <summary>Top of the two Cairo vertices that are inside of the hexagon.</summary>
                CenterTop,
                /// <summary>Bottom of the two Cairo vertices that are inside of the hexagon.</summary>
                CenterBottom
            }

            private const double sqrt7 = 2.6457513110645905905016157536392604257102591830825;
            private const double h = (sqrt7 + 1) / 2;
            private static readonly double[] xs = { -(sqrt7 + 1) / 4, -sqrt7 / 2, -(sqrt7 + 1) / 4, -.5, 0, 0 };
            private static readonly double[] ys = { h / 2, 0, -h / 2, -h, -.5, .5 };

            /// <inheritdoc/>
            public override double X => Hex.Q * h + xs[(int) Pos];
            /// <inheritdoc/>
            public override double Y => Hex.Q * h + Hex.R * (sqrt7 + 1) + ys[(int) Pos];

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex cv && cv.Hex.Equals(Hex) && cv.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex cv && cv.Hex.Equals(Hex) && cv.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => Hex.GetHashCode() * 11 + (int) Pos;
        }
    }
}
