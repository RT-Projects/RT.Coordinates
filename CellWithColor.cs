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
        /// <summary>Determines an SVG color, or <c>null</c> to use a default color.</summary>
        public string SvgColor { get; private set; }

        /// <summary>Constructor.</summary>
        public CellWithColor(TCell cell, string svgColor)
        {
            Cell = cell;
            SvgColor = svgColor;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CellWithColor<TCell> other && EqualityComparer<TCell>.Default.Equals(Cell, other.Cell) && SvgColor == other.SvgColor;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1413938657;
            hashCode = hashCode * -1521134295 + EqualityComparer<TCell>.Default.GetHashCode(Cell);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SvgColor);
            return hashCode;
        }

        /// <summary>Deconstructor.</summary>
        public void Deconstruct(out TCell cell, out string svgColor)
        {
            cell = Cell;
            svgColor = SvgColor;
        }

        /// <summary>Implicit conversion that associates the specified cell with the default highlight color.</summary>
        public static implicit operator CellWithColor<TCell>(TCell cell) => new CellWithColor<TCell>(cell, null);
    }
}
