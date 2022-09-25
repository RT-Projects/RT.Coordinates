using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Contains instructions to control the behavior of <see cref="Structure{TCell}.Svg(SvgInstructions{TCell})"/>.</summary>
    public class SvgInstructions<TCell>
    {
        /// <summary>Adds extra SVG code for every cell. <c>TCell</c> must implement <see cref="IHasSvgGeometry{TCell}"/>.</summary>
        public Func<TCell, string> PerCell = null;

        /// <summary>
        ///     Instructs <see cref="Structure{TCell}.Svg(SvgInstructions{TCell})"/> to paint the specified cells with a
        ///     background color.</summary>
        public IEnumerable<TCell> HighlightCells = null;

        /// <summary>Specifies the color used when highlighting cells specified by <see cref="HighlightCells"/>.</summary>
        public string HighlightColor = "hsl(284, 83%, 85%)";
    }
}
