using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RT.Coordinates
{
    /// <summary>Represents a cell in a <see cref="CairoGrid"/>.</summary>
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
                        yield return new Cairo(Hex.Move(HexDirection.Up), Position.Bottom);
                        yield return new Cairo(Hex.Move(HexDirection.UpRight), Position.Left);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex.Move(HexDirection.UpLeft), Position.Right);
                        break;

                    case Position.Right:
                        yield return new Cairo(Hex, Position.Bottom);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex, Position.Top);
                        yield return new Cairo(Hex.Move(HexDirection.UpRight), Position.Bottom);
                        yield return new Cairo(Hex.Move(HexDirection.DownRight), Position.Top);
                        break;

                    case Position.Bottom:
                        yield return new Cairo(Hex.Move(HexDirection.DownRight), Position.Left);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Left);
                        yield return new Cairo(Hex.Move(HexDirection.DownLeft), Position.Right);
                        yield return new Cairo(Hex.Move(HexDirection.Down), Position.Top);
                        break;

                    default:    // Position.Left
                        yield return new Cairo(Hex, Position.Top);
                        yield return new Cairo(Hex, Position.Right);
                        yield return new Cairo(Hex, Position.Bottom);
                        yield return new Cairo(Hex.Move(HexDirection.DownLeft), Position.Top);
                        yield return new Cairo(Hex.Move(HexDirection.UpLeft), Position.Bottom);
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Cairo"/>, going clockwise from the “tip” of the
        ///     pentagon (the vertex opposite the horizontal or vertical edge).</summary>
        public Vertex[] Vertices => Pos switch
        {
            Position.Top => new Vertex[] {
                new CairoVertex(Hex, CairoVertex.Position.CenterTop),
                new CairoVertex(Hex, CairoVertex.Position.MidTopLeft),
                new CairoVertex(Hex, CairoVertex.Position.TopLeft),
                new CairoVertex(Hex.Move(HexDirection.UpRight), CairoVertex.Position.Left),
                new CairoVertex(Hex.Move(HexDirection.UpRight), CairoVertex.Position.MidBottomLeft)
            },
            Position.Right => new Vertex[] {
                new CairoVertex(Hex.Move(HexDirection.DownRight), CairoVertex.Position.TopLeft),
                new CairoVertex(Hex.Move(HexDirection.DownRight), CairoVertex.Position.MidTopLeft),
                new CairoVertex(Hex, CairoVertex.Position.CenterBottom),
                new CairoVertex(Hex, CairoVertex.Position.CenterTop),
                new CairoVertex(Hex.Move(HexDirection.UpRight), CairoVertex.Position.MidBottomLeft)
            },
            Position.Bottom => new Vertex[] {
                new CairoVertex(Hex, CairoVertex.Position.CenterBottom),
                new CairoVertex(Hex.Move(HexDirection.DownRight), CairoVertex.Position.MidTopLeft),
                new CairoVertex(Hex.Move(HexDirection.DownRight), CairoVertex.Position.Left),
                new CairoVertex(Hex.Move(HexDirection.Down), CairoVertex.Position.TopLeft),
                new CairoVertex(Hex, CairoVertex.Position.MidBottomLeft)
            },
            // Position.Left
            _ => new Vertex[] {
                new CairoVertex(Hex, CairoVertex.Position.Left),
                new CairoVertex(Hex, CairoVertex.Position.MidTopLeft),
                new CairoVertex(Hex, CairoVertex.Position.CenterTop),
                new CairoVertex(Hex, CairoVertex.Position.CenterBottom),
                new CairoVertex(Hex, CairoVertex.Position.MidBottomLeft)
            },
        };

        private const double sqrt7 = 2.6457513110645905905016157536392604257102591830825;
        private static readonly double[] xs = { 0, sqrt7 / 4, 0, -sqrt7 / 4 };
        private static readonly double[] ys = { -sqrt7 / 4 - .5, 0, sqrt7 / 4 + .5, 0 };

        /// <inheritdoc/>
        public PointD Center => new PointD(Hex.Q * (sqrt7 + 1) / 2 + xs[(int) Pos], Hex.Q * (sqrt7 + 1) / 2 + Hex.R * (sqrt7 + 1) + ys[(int) Pos]);

        /// <inheritdoc/>
        public override string ToString() => $"{Hex};{(int) Pos}";
    }
}
