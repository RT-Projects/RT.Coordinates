using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a kite-shaped cell in a <see cref="KiteGrid"/>. The “spikier” vertex of the kite is the center of a
    ///     hexagon; the sides adjacent are spokes connecting with the midpoints of the hexagon’s outline; and the rest form
    ///     the outline.</summary>
    public struct Kite : IEquatable<Kite>, IHasSvgGeometry, INeighbor<Kite>
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
                yield return new Kite(Hex.Move((HexDirection) (((int) Pos + 1) % 6)), (Position) (((int) Pos + 2) % 6));
                yield return new Kite(Hex.Move((HexDirection) (((int) Pos + 2) % 6)), (Position) (((int) Pos + 4) % 6));
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Kite"/>, going clockwise from the “spiky” vertex
        ///     (center of <see cref="Hex"/>).</summary>
        public Vertex[] Vertices => new Vertex[]
        {
            new KiteVertex(Hex, KiteVertex.Position.Center),
            new KiteVertex(Hex, fullPos: 2 * (int) Pos),
            new KiteVertex(Hex, fullPos: 2 * (int) Pos + 1),
            new KiteVertex(Hex, fullPos: 2 * (int) Pos + 2)
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
    }
}
