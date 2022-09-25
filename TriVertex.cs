using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in a triangular grid (<see cref="TriGrid"/>).</summary>
    public class TriVertex : Vertex<Tri>
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
        public override bool Equals(Vertex<Tri> other) => other is TriVertex tv && tv.Tri.Equals(Tri);
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is TriVertex tv && tv.Tri.Equals(Tri);
        /// <inheritdoc/>
        public override int GetHashCode() => Tri.GetHashCode();

        /// <inheritdoc/>
        public override IEnumerable<Tri> Cells
        {
            get
            {
                yield return new Tri(Tri.X - 1, Tri.Y - 1);
                yield return new Tri(Tri.X, Tri.Y - 1);
                yield return new Tri(Tri.X + 1, Tri.Y - 1);
                yield return new Tri(Tri.X + 1, Tri.Y);
                yield return Tri;
                yield return new Tri(Tri.X - 1, Tri.Y);
            }
        }

        /// <inheritdoc/>
        public override double X => Tri.X / 2d;
        /// <inheritdoc/>
        public override double Y => Tri.Y * 0.86602540378443864676372317075294; // sin 60°
    }
}
