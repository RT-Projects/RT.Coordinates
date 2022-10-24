using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a <see cref="FloretGrid"/>. Each floret is a pentagon with one vertex “spikier” than the
    ///     others. That vertex can be thought of as the center of a hexagon, while the vertex two clockwise from that is a
    ///     vertex of the same hexagon. The remaining vertices are off from the hexagon’s edge but in such a way that 6
    ///     florets make a flower-like shape which tiles the plane in a hexagonal pattern.</summary>
    public struct Floret : IEquatable<Floret>, IHasSvgGeometry, INeighbor<Floret>
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
                yield return new Floret(Hex.Move((HexDirection) (((int) Pos + 1) % 6)), (Position) (((int) Pos + 2) % 6));
                yield return new Floret(Hex.Move((HexDirection) (((int) Pos + 2) % 6)), (Position) (((int) Pos + 4) % 6));
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Link<Vertex>> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Floret"/>, going clockwise from the “spiky” vertex
        ///     (center of <see cref="Hex"/>).</summary>
        public Vertex[] Vertices => new Vertex[]
        {
            new FloretVertex(Hex, FloretVertex.Position.Center),
            new FloretVertex(Hex, fullPos: 3 * (int) Pos + 1),
            new FloretVertex(Hex, fullPos: 3 * (int) Pos + 2),
            new FloretVertex(Hex, fullPos: 3 * (int) Pos + 3),
            new FloretVertex(Hex, fullPos: 3 * (int) Pos + 4)
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
    }
}
