using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a <see cref="RhombGrid"/>. Three cells of this kind form a hexagon, which in turn tiles the
    ///     plane.</summary>
    public struct Rhomb : IEquatable<Rhomb>, INeighbor<Rhomb>, IHasSvgGeometry
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
        public bool Equals(Rhomb rhomb) => Hex.Equals(rhomb.Hex) && Pos == rhomb.Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Rhomb rhomb && Hex.Equals(rhomb.Hex) && Pos == rhomb.Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 3 + (int) Pos;

        /// <inheritdoc/>
        public IEnumerable<Rhomb> Neighbors
        {
            get
            {
                switch (Pos)
                {
                    case Position.TopRight:
                        yield return new Rhomb(Hex.Move(HexDirection.Up), Position.BottomRight);
                        yield return new Rhomb(Hex.Move(HexDirection.UpRight), Position.Left);
                        yield return new Rhomb(Hex, Position.BottomRight);
                        yield return new Rhomb(Hex, Position.Left);
                        break;

                    case Position.BottomRight:
                        yield return new Rhomb(Hex, Position.TopRight);
                        yield return new Rhomb(Hex.Move(HexDirection.DownRight), Position.Left);
                        yield return new Rhomb(Hex.Move(HexDirection.Down), Position.TopRight);
                        yield return new Rhomb(Hex, Position.Left);
                        break;

                    case Position.Left:
                        yield return new Rhomb(Hex, Position.TopRight);
                        yield return new Rhomb(Hex, Position.BottomRight);
                        yield return new Rhomb(Hex.Move(HexDirection.DownLeft), Position.TopRight);
                        yield return new Rhomb(Hex.Move(HexDirection.UpLeft), Position.BottomRight);
                        break;

                    default:
                        throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.");
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Rhomb"/>, going clockwise from the vertex at the
        ///     center of <see cref="Hex"/>.</summary>
        public Vertex[] Vertices => Pos switch
        {
            Position.TopRight => new Vertex[] {
                new RhombVertex(Hex, RhombVertex.Position.Center),
                new RhombVertex(Hex, RhombVertex.Position.TopLeft),
                new RhombVertex(Hex, RhombVertex.Position.TopRight),
                new RhombVertex(Hex.Move(HexDirection.DownRight), RhombVertex.Position.TopLeft)
            },
            Position.BottomRight => new Vertex[] {
                new RhombVertex(Hex, RhombVertex.Position.Center),
                new RhombVertex(Hex.Move(HexDirection.DownRight), RhombVertex.Position.TopLeft),
                new RhombVertex(Hex.Move(HexDirection.Down), RhombVertex.Position.TopRight),
                new RhombVertex(Hex.Move(HexDirection.Down), RhombVertex.Position.TopLeft)
            },
            Position.Left => new Vertex[] {
                new RhombVertex(Hex, RhombVertex.Position.Center),
                new RhombVertex(Hex.Move(HexDirection.Down), RhombVertex.Position.TopLeft),
                new RhombVertex(Hex.Move(HexDirection.DownLeft), RhombVertex.Position.TopRight),
                new RhombVertex(Hex, RhombVertex.Position.TopLeft)
            },
            _ => throw new InvalidOperationException($"{nameof(Pos)} has an invalid value of {Pos}.")
        };

        /// <inheritdoc/>
        public PointD Center => Hex.Center * 1.5;
    }
}
