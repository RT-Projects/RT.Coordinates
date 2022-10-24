namespace RT.Coordinates
{
    /// <summary>Describes one of the vertices of a <see cref="Floret"/>.</summary>
    public class FloretVertex : Vertex
    {
        /// <summary>The <see cref="Hex"/> tile that this <see cref="FloretVertex"/> is within.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which position within the <see cref="Hex"/> this vertex is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public FloretVertex(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Constructs a <see cref="FloretVertex"/> from any position along the perimeter of <paramref name="hex"/>.</summary>
        /// <param name="hex">
        ///     Hexagon to compute vertex from.</param>
        /// <param name="fullPos">
        ///     Position of the vertex counting clockwise from the vertex at the midpoint of the hexagon�s top edge. Note that
        ///     even numbers yield midpoints, odd numbers yield vertices of the hexagon.</param>
        /// <remarks>
        ///     Note that this constructor cannot construct a vertex that is at the center of a hexagon. Use <see
        ///     cref="FloretVertex(Hex, Position)"/> for that.</remarks>
        public FloretVertex(Hex hex, int fullPos)
        {
            switch ((fullPos % 18 + 18) % 18)
            {
                case 8: Hex = hex.Move(HexDirection.Down); Pos = Position.TopRight; break;
                case 9: Hex = hex.Move(HexDirection.Down); Pos = Position.TopRightMinus1; break;
                case 10: Hex = hex.Move(HexDirection.Down); Pos = Position.TopLeftPlus1; break;
                case 11: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.Right; break;
                case 12: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.RightMinus1; break;
                case 13: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.TopRightPlus1; break;
                case 14: Hex = hex.Move(HexDirection.DownLeft); Pos = Position.TopRight; break;
                case 15: Hex = hex.Move(HexDirection.UpLeft); Pos = Position.BottomRightMinus1; break;
                case 16: Hex = hex.Move(HexDirection.UpLeft); Pos = Position.RightPlus1; break;
                case 17: Hex = hex.Move(HexDirection.UpLeft); Pos = Position.Right; break;

                default:    // <= 7
                    Pos = (Position) ((fullPos % 18 + 18) % 18);
                    Hex = hex;
                    break;
            }
        }

        /// <summary>
        ///     Describes the position of a <see cref="FloretVertex"/> in relation to the vertices of its referenced <see
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
            /// <summary>The vertex one counter-clockwise from the bottom-right vertex of the referenced <see cref="Hex"/>.</summary>
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
        public override bool Equals(Vertex other) => other is FloretVertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FloretVertex kv && kv.Hex.Equals(Hex) && kv.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 6 + (int) Pos;
    }
}
