using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes an SVG color with optional opacity information.</summary>
    public struct SvgColor(string color = null, string opacity = null) : IEquatable<SvgColor>
    {
        /// <summary>Determines the SVG color, or <c>null</c> to use a default color.</summary>
        public string SvgFillColor { get; private set; } = color;
        /// <summary>Determines the SVG fill opacity, or <c>null</c> to omit the attribute.</summary>
        public string SvgFillOpacity { get; private set; } = opacity;

        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is SvgColor other && SvgFillColor == other.SvgFillColor && SvgFillOpacity == other.SvgFillOpacity;
        /// <inheritdoc/>
        public readonly bool Equals(SvgColor other) => SvgFillColor == other.SvgFillColor && SvgFillOpacity == other.SvgFillOpacity;

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            var hashCode = 1413938657;
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillColor));
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillOpacity));
            return hashCode;
        }

        /// <summary>Deconstructor.</summary>
        public readonly void Deconstruct(out string color, out string opacity)
        {
            color = SvgFillColor;
            opacity = SvgFillOpacity;
        }

        /// <inheritdoc/>
        public override readonly string ToString() => $"{SvgFillColor}{(SvgFillOpacity == null ? "" : "/")}{SvgFillOpacity}";

        /// <summary>Implicitly converts a string (containing an SVG color) to an opaque <see cref="SvgColor"/>.</summary>
        public static implicit operator SvgColor(string color) => new(color: color);
    }
}
