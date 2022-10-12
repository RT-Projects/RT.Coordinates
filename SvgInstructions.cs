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
        ///     Returns a collection of vertices for the specified cell. Only required for cells that don’t implement <see
        ///     cref="IHasSvgGeometry"/>.</summary>
        public Func<TCell, IEnumerable<Vertex>> GetVertices;

        /// <summary>
        ///     Returns the center point for the specified cell. Only required for cells that don’t implement <see
        ///     cref="IHasSvgGeometry"/>.</summary>
        public Func<TCell, PointD?> GetCenter;
    }
}
