using System;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in a triangular grid (<see cref="TriGrid"/>).</summary>
    public class TriVertex : Vertex
    {
        /// <summary>
        ///     Constructs a <see cref="TriVertex"/> representing the top vertex of an up-pointing triangle.</summary>
        /// <param name="tri">
        ///     The triangle representing this vertex.</param>
        public TriVertex(Tri tri)
        {
            if (!tri.IsUpPointing)
                throw new ArgumentException("The tri must be an up-pointing tri.", nameof(tri));
            Tri = tri;
        }

        /// <summary>Returns the tri whose top vertex is this vertex.</summary>
        public Tri Tri { get; private set; }

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is TriVertex tv && tv.Tri.Equals(Tri);
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is TriVertex tv && tv.Tri.Equals(Tri);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(Tri.GetHashCode() + 47);

        /// <inheritdoc/>
        public override double X => Tri.X / 2d;
        /// <inheritdoc/>
        public override double Y => Tri.Y * 0.86602540378443864676372317075294; // sin 60°
    }
}
