using System;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a vertex in a pentagonal grid using a system of four base vectors, which are unit vectors 36° from one
    ///     another. Used by <see cref="Penrose"/> and <see cref="PentaCell"/>.</summary>
    public struct Pentavector(int a, int b, int c, int d) : IEquatable<Pentavector>
    {
        /// <summary>
        ///     The A component of this vector, which extends outward from the origin at a 54° angle counter-clockwise (SVG)
        ///     or clockwise (geometry) from the x-axis.</summary>
        public int A { get; private set; } = a;
        /// <summary>
        ///     The B component of this vector, which extends outward from the origin at an 18° angle counter-clockwise (SVG)
        ///     or clockwise (geometry) from the x-axis.</summary>
        public int B { get; private set; } = b;
        /// <summary>
        ///     The C component of this vector, which extends outward from the origin at an 18° angle clockwise (SVG) or
        ///     counter-clockwise (geometry) from the x-axis.</summary>
        public int C { get; private set; } = c;
        /// <summary>
        ///     The D component of this vector, which extends outward from the origin at a 54° angle clockwise (SVG) or
        ///     counter-clockwise (geometry) from the x-axis.</summary>
        public int D { get; private set; } = d;

        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector at a 54° angle counter-clockwise (SVG) or
        ///     clockwise (geometry) from the x-axis.</summary>
        public static readonly Pentavector BaseA = new(1, 0, 0, 0);
        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector at a 18° angle counter-clockwise (SVG) or
        ///     clockwise (geometry) from the x-axis.</summary>
        public static readonly Pentavector BaseB = new(0, 1, 0, 0);
        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector at a 18° angle clockwise (SVG) or
        ///     counter-clockwise (geometry) from the x-axis.</summary>
        public static readonly Pentavector BaseC = new(0, 0, 1, 0);
        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector at a 54° angle clockwise (SVG) or
        ///     counter-clockwise (geometry) from the x-axis.</summary>
        public static readonly Pentavector BaseD = new(0, 0, 0, 1);
        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector pointing straight down (SVG) or up (geometry).</summary>
        public static readonly Pentavector BaseE = new(-1, 1, -1, 1);

        /// <summary>
        ///     Returns a <see cref="Pentavector"/> describing a unit vector at the specified <paramref name="angle"/>.</summary>
        /// <param name="angle">
        ///     The angle, specified as a multiple of 36° going clockwise (SVG) or counter-clockwise (geometry) from
        ///     vertically straight up (SVG) or down (geometry).</param>
        /// <remarks>
        ///     Note that if <paramref name="angle"/> is <c>0</c>, the vector returned is the negative of <see cref="BaseE"/>.</remarks>
        public static Pentavector Base(int angle) => ((angle % 10 + 10) % 10) switch
        {
            0 => -BaseE,
            1 => BaseA,
            2 => BaseB,
            3 => BaseC,
            4 => BaseD,
            5 => BaseE,
            6 => -BaseA,
            7 => -BaseB,
            8 => -BaseC,
            9 => -BaseD,
            _ => throw new InvalidOperationException($"Invalid angle value ‘{angle}’.")
        };

        /// <summary>
        ///     Rotates the vector the specified <paramref name="clockwiseAmount"/> times 36°.</summary>
        /// <returns>
        ///     The rotation proceeds clockwise in SVG and counter-clockwise in geometry.</returns>
        public readonly Pentavector Rotate(int clockwiseAmount = 1)
        {
            clockwiseAmount = (clockwiseAmount % 10 + 10) % 10;
            var v = this;
            while (clockwiseAmount-- > 0)
                v = new Pentavector(-v.D, v.D + v.A, -v.D + v.B, v.D + v.C);
            return v;
        }

        /// <summary>
        ///     Returns a new <see cref="Pentavector"/> whose direction is the same as this vector, but with a length
        ///     multiplied by 1/φ. φ is the golden ratio ((√(5)+1)/2).</summary>
        public readonly Pentavector DivideByPhi => new(B - D, C + D - B, A + B - C, C - A);
        /// <summary>
        ///     Returns a new <see cref="Pentavector"/> whose direction is the same as this vector, but with a length
        ///     multiplied by φ. φ is the golden ratio ((√(5)+1)/2).</summary>
        public readonly Pentavector MultiplyByPhi => new(A + B - D, C + D, A + B, C + D - A);

        /// <inheritdoc/>
        public readonly bool Equals(Pentavector other) => other.A == A && other.B == B && other.C == C && other.D == D;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Pentavector other && other.A == A && other.B == B && other.C == C && other.D == D;
        /// <inheritdoc/>
        public override readonly int GetHashCode() => unchecked(148139063 * A + 336220337 * B + 672753941 * C + 797808397 * D);

        /// <summary>Addition operator.</summary>
        public static Pentavector operator +(Pentavector one, Pentavector two) => new(one.A + two.A, one.B + two.B, one.C + two.C, one.D + two.D);
        /// <summary>Subtraction operator.</summary>
        public static Pentavector operator -(Pentavector one, Pentavector two) => new(one.A - two.A, one.B - two.B, one.C - two.C, one.D - two.D);
        /// <summary>Multiplication operator.</summary>
        public static Pentavector operator *(Pentavector one, int two) => new(one.A * two, one.B * two, one.C * two, one.D * two);
        /// <summary>Multiplication operator.</summary>
        public static Pentavector operator *(int one, Pentavector two) => new(two.A * one, two.B * one, two.C * one, two.D * one);
        /// <summary>Unary negation operator.</summary>
        public static Pentavector operator -(Pentavector op) => new(-op.A, -op.B, -op.C, -op.D);

        /// <summary>Equality comparison.</summary>
        public static bool operator ==(Pentavector one, Pentavector two) => one.Equals(two);
        /// <summary>Inequality comparison.</summary>
        public static bool operator !=(Pentavector one, Pentavector two) => !one.Equals(two);

        private const double sin18 = .30901699437494742410;
        private const double cos18 = .95105651629515357210;
        private const double sin54 = .80901699437494742410;
        private const double cos54 = .58778525229247312918;

        /// <summary>Returns the 2D point represented by this vector.</summary>
        public readonly PointD Point => new(cos54 * (A + D) + cos18 * (B + C), sin54 * (D - A) + sin18 * (C - B));

        /// <summary>Implicit operator converting <see cref="Pentavector"/> to <see cref="Coordinates.Vertex"/>.</summary>
        public static implicit operator Vertex(Pentavector v) => new(v);
        /// <inheritdoc/>
        public override readonly string ToString() => $"({A},{B},{C},{D})";

        /// <summary>
        ///     Describes a vertex in a grid constructed from <see cref="Pentavector"/> values.</summary>
        /// <remarks>
        ///     This is merely a thin wrapper around <see cref="Pentavector"/> but for the purpose of implementing <see
        ///     cref="Coordinates.Vertex"/>.</remarks>
        public class Vertex(Pentavector vector) : Coordinates.Vertex
        {
            /// <summary>The underlying <see cref="Vector"/>.</summary>
            public Pentavector Vector { get; private set; } = vector;

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex v && v.Vector.Equals(Vector);
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex v && v.Vector.Equals(Vector);
            /// <inheritdoc/>
            public override int GetHashCode() => Vector.GetHashCode();
            /// <inheritdoc/>
            public override PointD Point => Vector.Point;
        }
    }
}
