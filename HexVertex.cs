namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in a hexagonal grid (<see cref="HexGrid"/>).</summary>
    public class HexVertex : Vertex
    {
        /// <summary>Returns the hex just below this vertex.</summary>
        public Hex Hex { get; private set; }

        /// <summary>If <c>true</c>, this vertex is the top-right vertex of <see cref="Hex"/>; otherwise, the top-left.</summary>
        public bool Right { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="hex">
        ///     The hex just below this vertex.</param>
        /// <param name="right">
        ///     If <c>true</c>, identifies the top-right vertex of <paramref name="hex"/>; otherwise, the top-left.</param>
        public HexVertex(Hex hex, bool right)
        {
            Hex = hex;
            Right = right;
        }

        /// <inheritdoc/>
        public override double X => Hex.Q * .75 + (Right ? .25 : -.25);
        /// <inheritdoc/>
        public override double Y => (Hex.Q * .5 + Hex.R - .5) * Hex.WidthToHeight;

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is HexVertex hv && hv.Hex == Hex && hv.Right == Right;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is HexVertex hv && Equals(hv);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(Hex.GetHashCode() * (Right ? 1048609 : 1048601));
    }
}
