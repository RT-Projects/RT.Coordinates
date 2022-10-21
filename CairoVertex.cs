namespace RT.Coordinates
{
    /// <summary>Describes one of the vertices of a <see cref="Cairo"/>.</summary>
    public class CairoVertex : Vertex
    {
        /// <summary>The <see cref="Hex"/> tile that this <see cref="CairoVertex"/> is within.</summary>
        public Hex Hex { get; private set; }
        /// <summary>Which position within the <see cref="Hex"/> this vertex is.</summary>
        public Position Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public CairoVertex(Hex hex, Position pos)
        {
            Hex = hex;
            Pos = pos;
        }

        /// <summary>
        ///     Describes the position of a <see cref="CairoVertex"/> in relation to the vertices of its containing <see
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
        public override bool Equals(Vertex other) => other is CairoVertex cv && cv.Hex.Equals(Hex) && cv.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CairoVertex cv && cv.Hex.Equals(Hex) && cv.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => Hex.GetHashCode() * 11 + (int) Pos;
    }
}
