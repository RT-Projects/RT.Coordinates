using System;

namespace RT.Coordinates
{
    /// <summary>Describes one of the vertices of a <see cref="Rhomb"/>.</summary>
    public class RhombVertex : Vertex, IEquatable<Vertex>
    {
        /// <summary>The <see cref="Hex"/> tile that this <see cref="RhombVertex"/> is within.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which position within the <see cref="Hex"/> this vertex is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public RhombVertex(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Describes the position of a <see cref="RhombVertex"/> in relation to the vertices of its containing <see
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
        public override bool Equals(object obj) => obj is RhombVertex rv && Hex.Equals(rv.Hex) && Pos == rv.Pos;
        /// <inheritdoc/>
        public override bool Equals(Vertex vertex) => vertex is RhombVertex rv && Hex.Equals(rv.Hex) && Pos == rv.Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 13 + (int) Pos;

        /// <inheritdoc/>
        public override double X => Hex.Q * 1.125 + (Pos switch { Position.TopLeft => -.375, Position.TopRight => .375, Position.Center => 0, _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.") });
        /// <inheritdoc/>
        public override double Y => Hex.WidthToHeight * (Hex.Q * .75 + Hex.R * 1.5 + (Pos switch { Position.TopLeft => -.75, Position.TopRight => -.75, Position.Center => 0, _ => throw new InvalidOperationException($"{nameof(Pos)} has invalid value {Pos}.") }));
    }
}
