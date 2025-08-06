using System;

namespace RT.Coordinates;

/// <summary>Encapsulates a rational number between 0 and 1 that represents a fraction of a circle.</summary>
public struct CircleFraction : IEquatable<CircleFraction>, IComparable<CircleFraction>
{
    /// <summary>Numerator.</summary>
    public int Numerator { get; private set; }
    /// <summary>Denominator.</summary>
    public int Denominator { get; private set; }

    /// <summary>Constructor.</summary>
    public CircleFraction(int numerator, int denominator)
    {
        if (numerator % denominator == 0)
        {
            Numerator = 0;
            Denominator = 1;
        }
        else
        {
            var divisor = gcd(Math.Abs(numerator), Math.Abs(denominator));
            Denominator = denominator / divisor;
            var num = numerator / divisor;
            Numerator = (num % Denominator + Denominator) % Denominator;
        }
    }

    /// <summary>Represents the value 0 (zero).</summary>
    public static readonly CircleFraction Zero = new(0, 1);

    private static int gcd(int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    /// <summary>Addition operator.</summary>
    public static CircleFraction operator +(CircleFraction a, CircleFraction b) => new(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    /// <summary>Subtraction operator.</summary>
    public static CircleFraction operator -(CircleFraction a, CircleFraction b) => new(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    /// <summary>Multiplication operator.</summary>
    public static CircleFraction operator *(CircleFraction a, CircleFraction b) => new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    /// <summary>Division operator.</summary>
    public static CircleFraction operator /(CircleFraction a, CircleFraction b) => new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    /// <summary>Less-than comparison operator.</summary>
    public static bool operator <(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator < b.Numerator * a.Denominator;
    /// <summary>Greater-than comparison operator.</summary>
    public static bool operator >(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator > b.Numerator * a.Denominator;
    /// <summary>Less-than-or-equal-to comparison operator.</summary>
    public static bool operator <=(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator <= b.Numerator * a.Denominator;
    /// <summary>Greater-than-or-equal-to comparison operator.</summary>
    public static bool operator >=(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator >= b.Numerator * a.Denominator;
    /// <summary>Equality comparison operator.</summary>
    public static bool operator ==(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator == b.Numerator * a.Denominator;
    /// <summary>Inequality comparison operator.</summary>
    public static bool operator !=(CircleFraction a, CircleFraction b) => a.Numerator * b.Denominator != b.Numerator * a.Denominator;
    /// <summary>
    ///     Determines whether the current value is strictly between <paramref name="min"/> and <paramref name="max"/>
    ///     (exclusive).</summary>
    public readonly bool IsStrictlyBetween(CircleFraction min, CircleFraction max) => max > min ? (this > min && this < max) : (this > min || this < max);
    /// <summary>
    ///     Determines whether the current value is between <paramref name="min"/> and <paramref name="max"/> (inclusive).</summary>
    public readonly bool IsBetweenInclusive(CircleFraction min, CircleFraction max) => max > min ? (this >= min && this <= max) : (this >= min || this <= max);

    /// <summary>Addition operator.</summary>
    public static double operator +(CircleFraction a, double b) => b + ((double) a.Numerator / a.Denominator);
    /// <summary>Addition operator.</summary>
    public static double operator +(double b, CircleFraction a) => b + ((double) a.Numerator / a.Denominator);
    /// <summary>Subtraction operator.</summary>
    public static double operator -(CircleFraction a, double b) => ((double) a.Numerator / a.Denominator) - b;
    /// <summary>Subtraction operator.</summary>
    public static double operator -(double b, CircleFraction a) => b - ((double) a.Numerator / a.Denominator);
    /// <summary>Multiplication operator.</summary>
    public static double operator *(CircleFraction a, double b) => b * a.Numerator / a.Denominator;
    /// <summary>Multiplication operator.</summary>
    public static double operator *(double b, CircleFraction a) => b * a.Numerator / a.Denominator;
    /// <summary>Division operator.</summary>
    public static double operator /(CircleFraction a, double b) => (double) a.Numerator / a.Denominator / b;
    /// <summary>Division operator.</summary>
    public static double operator /(double b, CircleFraction a) => b * a.Denominator / a.Numerator;

    /// <inheritdoc/>
    public readonly int CompareTo(CircleFraction other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);
    /// <inheritdoc/>
    public readonly bool Equals(CircleFraction other) => other.Numerator == Numerator && other.Denominator == Denominator;
    /// <inheritdoc/>
    public override readonly bool Equals(object obj) => obj is CircleFraction other && other.Numerator == Numerator && other.Denominator == Denominator;
    /// <inheritdoc/>
    public override readonly int GetHashCode() => unchecked(Numerator * 28935701 + Denominator);
    /// <inheritdoc/>
    public override readonly string ToString() => $"{Numerator}/{Denominator}";
}
