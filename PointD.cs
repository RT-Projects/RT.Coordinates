using System;

namespace RT.Coordinates
{
    /// <summary>Represents a point in 2D space.</summary>
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        ///     Constructor.</summary>
        /// <param name="x">
        ///     X-coordinate.</param>
        /// <param name="y">
        ///     Y-coordinate.</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>X-coordinate.</summary>
        public double X { get; private set; }
        /// <summary>Y-coordinate.</summary>
        public double Y { get; private set; }

        /// <summary>Multiplies a point by a specified scalar factor.</summary>
        public static PointD operator *(PointD p, double amount) => new PointD(p.X * amount, p.Y * amount);
        /// <summary>Multiplies a point by a specified scalar factor.</summary>
        public static PointD operator *(double amount, PointD p) => new PointD(p.X * amount, p.Y * amount);
        /// <summary>Divides a point by a specified scalar factor.</summary>
        public static PointD operator /(PointD p, double amount) => new PointD(p.X / amount, p.Y / amount);
        /// <summary>Adds two points (treating them as vectors).</summary>
        public static PointD operator +(PointD p1, PointD p2) => new PointD(p1.X + p2.X, p1.Y + p2.Y);
        /// <summary>Subtracts a point from another (treating them as vectors).</summary>
        public static PointD operator -(PointD p1, PointD p2) => new PointD(p1.X - p2.X, p1.Y - p2.Y);

        /// <summary>Calculates the distance of this point from the origin.</summary>
        public double Distance => Math.Sqrt(X * X + Y * Y);

        /// <summary>Implements <see cref="IEquatable{T}"/>.</summary>
        public bool Equals(PointD other) => X == other.X && Y == other.Y;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PointD p && Equals(p);
        /// <inheritdoc/>
        public override int GetHashCode() => X.GetHashCode() * 1073741827 + Y.GetHashCode();

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the origin.</summary>
        /// <param name="angle">
        ///     The angle in radians.</param>
        /// <returns>
        ///     The rotated point.</returns>
        public PointD Rotate(double angle)
        {
            var sina = Math.Sin(angle);
            var cosa = Math.Cos(angle);
            return new PointD(X * cosa + Y * sina, Y * cosa - X * sina);
        }

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the origin.</summary>
        /// <param name="angle">
        ///     The angle in degrees.</param>
        /// <returns>
        ///     The rotated point.</returns>
        public PointD RotateDeg(double angle) => Rotate(angle * Math.PI / 180);
    }
}
