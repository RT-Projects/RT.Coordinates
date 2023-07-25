using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes an SVG color with optional opacity information.</summary>
    public struct SvgColor : IEquatable<SvgColor>
    {
        /// <summary>Determines the SVG color, or <c>null</c> to use a default color.</summary>
        public string SvgFillColor { get; private set; }
        /// <summary>Determines the SVG fill opacity, or <c>null</c> to omit the attribute.</summary>
        public string SvgFillOpacity { get; private set; }

        /// <summary>Constructor.</summary>
        public SvgColor(string color = null, string opacity = null)
        {
            SvgFillColor = color;
            SvgFillOpacity = opacity;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SvgColor other && SvgFillColor == other.SvgFillColor && SvgFillOpacity == other.SvgFillOpacity;
        /// <inheritdoc/>
        public bool Equals(SvgColor other) => SvgFillColor == other.SvgFillColor && SvgFillOpacity == other.SvgFillOpacity;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1413938657;
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillColor));
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillOpacity));
            return hashCode;
        }

        /// <summary>Deconstructor.</summary>
        public void Deconstruct(out string color, out string opacity)
        {
            color = SvgFillColor;
            opacity = SvgFillOpacity;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{SvgFillColor}{(SvgFillOpacity == null ? "" : "/")}{SvgFillOpacity}";

        /// <summary>Implicitly converts a string (containing an SVG color) to an opaque <see cref="SvgColor"/>.</summary>
        public static implicit operator SvgColor(string color) => new SvgColor(color: color);
    }
}
