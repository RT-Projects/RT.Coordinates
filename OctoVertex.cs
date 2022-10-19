using System;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in an <see cref="OctoGrid"/>.</summary>
    public class OctoVertex : Vertex
    {
        /// <summary>Specifies the x-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
        public int CellX { get; private set; }
        /// <summary>Specifies the y-coordinate of the <see cref="OctoCell"/> below this vertex.</summary>
        public int CellY { get; private set; }
        /// <summary>Specifies which of the four vertices along the left and top edge of an octagon this is (0–3 clockwise).</summary>
        public int Pos { get; private set; }

        /// <summary>Constructor.</summary>
        public OctoVertex(int x, int y, int topPos)
        {
            if (topPos < 0 || topPos > 3)
                throw new ArgumentException($"‘{topPos}’ is not a valid value for ‘{nameof(topPos)}’ (0–3 expected).", nameof(topPos));
            CellX = x;
            CellY = y;
            Pos = topPos;
        }

        private const double D1 = .29289321881345247559915563789515096071516406231155;   // 1 - 1/√2
        private const double D2 = .70710678118654752440084436210484903928483593768845;   // 1/√2
        private static readonly double[] xs = { 0, 0, D1, D2 };
        private static readonly double[] ys = { D2, D1, 0, 0 };

        /// <inheritdoc/>
        public override double X => CellX + xs[Pos];
        /// <inheritdoc/>
        public override double Y => CellY + ys[Pos];

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is OctoVertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is OctoVertex ov && ov.CellX == CellX && ov.CellY == CellY && ov.Pos == Pos;
        /// <inheritdoc/>
        public override int GetHashCode() => CellX * 1073741827 + CellY * 47 + Pos;
    }
}
