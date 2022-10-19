using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Contains instructions to control the behavior of <see cref="Structure{TCell}.Svg(SvgInstructions{TCell})"/>.</summary>
    public class SvgInstructions<TCell>
    {
        /// <summary>Adds extra SVG code for every cell.</summary>
        public Func<TCell, string> PerCell = null;

        /// <summary>
        ///     Instructs <see cref="Structure{TCell}.Svg(SvgInstructions{TCell})"/> to paint the specified cells with a
        ///     background color.</summary>
        public IEnumerable<CellWithColor<TCell>> HighlightCells = null;

        /// <summary>Specifies the color used when highlighting cells specified by <see cref="HighlightCells"/>.</summary>
        public string HighlightColor = "hsl(284, 83%, 85%)";

        /// <summary>
        ///     Returns a collection of edges (line segments) that make up the perimeter of the specified cell. Only required
        ///     for cells that don’t implement <see cref="IHasSvgGeometry"/>.</summary>
        public Func<TCell, IEnumerable<Link<Vertex>>> GetEdges;

        /// <summary>
        ///     Returns the center point for the specified cell. Only required for cells that don’t implement <see
        ///     cref="IHasSvgGeometry"/>.</summary>
        public Func<TCell, PointD?> GetCenter;

        /// <summary>Provides some additional SVG code to add at the start of the file.</summary>
        public string ExtraSvg1;

        /// <summary>
        ///     Provides some additional SVG code to add into the file after the cell highlights (<see
        ///     cref="HighlightCells"/>) but before the main grid.</summary>
        public string ExtraSvg2;

        /// <summary>
        ///     Provides some additional SVG code to add into the file after the main grid but before <see cref="PerCell"/>.</summary>
        public string ExtraSvg3;

        /// <summary>Provides some additional SVG code to add at the end of the file.</summary>
        public string ExtraSvg4;

        /// <summary>
        ///     Generates the SVG path object for passable edges between cells. The function parameter is the path’s <c>d</c>
        ///     attribute.</summary>
        public Func<string, string> PassagesPath = d => $"<path d='{d}' fill='none' stroke-width='.02' stroke='#ccc' stroke-dasharray='.1' />";

        /// <summary>
        ///     Generates the SVG path object for impassable edges between cells (walls). The function parameter is the path’s
        ///     <c>d</c> attribute.</summary>
        public Func<string, string> WallsPath = d => $"<path d='{d}' fill='none' stroke-width='.05' stroke='black' />";

        /// <summary>
        ///     Generates the SVG path object for the outline of the structure. The function parameter is the path’s <c>d</c>
        ///     attribute.</summary>
        public Func<string, string> OutlinePath = d => $"<path d='{d}' fill='none' stroke-width='.1' stroke='black' />";
    }
}
