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
        public static PointD operator *(PointD p, double amount) => new(p.X * amount, p.Y * amount);
        /// <summary>Multiplies a point by a specified scalar factor.</summary>
        public static PointD operator *(double amount, PointD p) => new(p.X * amount, p.Y * amount);
        /// <summary>Divides a point by a specified scalar factor.</summary>
        public static PointD operator /(PointD p, double amount) => new(p.X / amount, p.Y / amount);
        /// <summary>Adds two points (treating them as vectors).</summary>
        public static PointD operator +(PointD p1, PointD p2) => new(p1.X + p2.X, p1.Y + p2.Y);
        /// <summary>Subtracts a point from another (treating them as vectors).</summary>
        public static PointD operator -(PointD p1, PointD p2) => new(p1.X - p2.X, p1.Y - p2.Y);

        /// <summary>Calculates the distance of this point from the origin.</summary>
        public readonly double Distance => Math.Sqrt(X * X + Y * Y);

        /// <summary>Implements <see cref="IEquatable{T}"/>.</summary>
        public readonly bool Equals(PointD other) => X == other.X && Y == other.Y;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is PointD p && Equals(p);
        /// <inheritdoc/>
        public override readonly int GetHashCode() => X.GetHashCode() * 1073741827 + Y.GetHashCode();

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the origin.</summary>
        /// <param name="angle">
        ///     The angle in radians.</param>
        /// <returns>
        ///     The rotated point.</returns>
        /// <remarks>
        ///     If the point coordinates are interpreted as is common in geometry (positive Y-axis goes up), this rotation is
        ///     counter-clockwise. In SVG, where the positive Y-axis goes down, the rotation is clockwise.</remarks>
        public readonly PointD Rotate(double angle)
        {
            var sina = Math.Sin(angle);
            var cosa = Math.Cos(angle);
            return new PointD(X * cosa - Y * sina, X * sina + Y * cosa);
        }

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the specified point.</summary>
        /// <param name="angle">
        ///     The angle in radians.</param>
        /// <param name="about">
        ///     Point to rotate the current point about.</param>
        /// <returns>
        ///     The rotated point.</returns>
        /// <remarks>
        ///     If the point coordinates are interpreted as is common in geometry (positive Y-axis goes up), this rotation is
        ///     counter-clockwise. In SVG, where the positive Y-axis goes down, the rotation is clockwise.</remarks>
        public readonly PointD Rotate(double angle, PointD about) => (this - about).Rotate(angle) + about;

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the origin.</summary>
        /// <param name="angle">
        ///     The angle in degrees.</param>
        /// <returns>
        ///     The rotated point.</returns>
        /// <remarks>
        ///     If the point coordinates are interpreted as is common in geometry (positive Y-axis goes up), this rotation is
        ///     counter-clockwise. In SVG, where the positive Y-axis goes down, the rotation is clockwise.</remarks>
        public readonly PointD RotateDeg(double angle) => Rotate(angle * Math.PI / 180);

        /// <summary>
        ///     Rotates the current point by the specified <paramref name="angle"/> about the specified point.</summary>
        /// <param name="angle">
        ///     The angle in degrees.</param>
        /// <param name="about">
        ///     Point to rotate the current point about.</param>
        /// <returns>
        ///     The rotated point.</returns>
        /// <remarks>
        ///     If the point coordinates are interpreted as is common in geometry (positive Y-axis goes up), this rotation is
        ///     counter-clockwise. In SVG, where the positive Y-axis goes down, the rotation is clockwise.</remarks>
        public readonly PointD RotateDeg(double angle, PointD about) => Rotate(angle * Math.PI / 180, about);

        /// <inheritdoc/>
        public override readonly string ToString() => $"({X}, {Y})";

        /// <summary>Returns the theta (angle) of the vector represented by this <see cref="PointD"/>.</summary>
        public readonly double Theta => Math.Atan2(Y, X);

        /// <summary>Returns the unit vector in the same direction as this one.</summary>
        public readonly PointD Unit
        {
            get
            {
                var len = Math.Sqrt(X * X + Y * Y);
                return new PointD(X / len, Y / len);
            }
        }
    }
}
