using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a grid and an SVG color.</summary>
    /// <typeparam name="TCell">
    ///     Type of cell in the grid.</typeparam>
    /// <remarks>
    ///     This is used mainly by <see cref="SvgInstructions{TCell}.HighlightCells"/>.</remarks>
    public struct CellWithColor<TCell>
    {
        /// <summary>Identifies a cell in a grid.</summary>
        public TCell Cell { get; private set; }
        /// <summary>Determines the SVG fill color, or <c>null</c> to use a default color.</summary>
        public string SvgFillColor { get; private set; }
        /// <summary>Determines the SVG fill opacity, or <c>null</c> to omit the attribute.</summary>
        public string SvgFillOpacity { get; private set; }

        /// <summary>Constructor.</summary>
        public CellWithColor(TCell cell, string color = null, string opacity = null)
        {
            Cell = cell;
            SvgFillColor = color;
            SvgFillOpacity = opacity;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CellWithColor<TCell> other && EqualityComparer<TCell>.Default.Equals(Cell, other.Cell) && SvgFillColor == other.SvgFillColor && SvgFillOpacity == other.SvgFillOpacity;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1413938657;
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<TCell>.Default.GetHashCode(Cell));
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillColor));
            hashCode = unchecked(hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgFillOpacity));
            return hashCode;
        }

        /// <summary>Deconstructor.</summary>
        public void Deconstruct(out TCell cell, out string color, out string opacity)
        {
            cell = Cell;
            color = SvgFillColor;
            opacity = SvgFillOpacity;
        }

        /// <summary>Implicit conversion that associates the specified cell with the default highlight color.</summary>
        public static implicit operator CellWithColor<TCell>(TCell cell) => new CellWithColor<TCell>(cell, null, null);
    }
}
