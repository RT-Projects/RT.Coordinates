namespace RT.Coordinates
{
    /// <summary>Describes one of the vertices of a <see cref="Kite"/>.</summary>
    public class KiteVertex : Vertex
    {
        /// <summary>The <see cref="Hex"/> tile that this <see cref="KiteVertex"/> is within.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which position within the <see cref="Hex"/> this vertex is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public KiteVertex(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructs a <see cref="KiteVertex"/> from any position along the perimeter of <paramref name="hex"/>.</summary>
        /// <param name="hex">
        ///     Hexagon to compute vertex from.</param>
        /// <param name="fullPos">
        ///     Position of the vertex counting clockwise from the vertex at the midpoint of the hexagon�s top edge. Note that
        ///     even numbers yield midpoints, odd numbers yield vertices of the hexagon.</param>
        /// <remarks>
        ///     Note that this constructor cannot construct a vertex that is at the center of a hexagon. Use <see
        ///     cref="KiteVertex(Hex, Position)"/> for that.</remarks>
        public KiteVertex(Hex hex, int fullPos)
        {
            switch ((fullPos % 12 + 12) % 12)
            {
                case 5: Hex = hex.Move(HexDirection.Down); Pos = Position.UpRight; break;
                case 6: Hex = hex.Move(HexDirection.Down); Pos = Position.MidUp; break;
                case 7: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.Right; break;
                case 8: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.MidUpRight; break;
                case 9: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.UpRight; break;
                case 10: Hex = hex.Move(HexDirection.UpLeft); Pos = Position.MidDownRight; break;
                case 11: Hex = hex.Move(HexDirection.UpLeft); Pos = Position.Right; break;

                default:    // <= 4
                    Pos = (Position) ((fullPos % 12 + 12) % 12);
                    Hex = hex;
                    break;
            }
        }

        /// <summary>
        ///     Describes the position of a <see cref="KiteVertex"/> in relation to the vertices of its containing <see
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
        public override bool Equals(Vertex other) => other is KiteVertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is KiteVertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 6 + (int) Pos;
    }
}
