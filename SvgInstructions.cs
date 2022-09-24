using System;

namespace RT.Coordinates
{
    /// <summary>Contains instructions to control the behavior of <see cref="Structure{TCell}.Svg(SvgInstructions{TCell})"/>.</summary>
    public class SvgInstructions<TCell>
    {
        /// <summary>
        ///     Adds extra SVG code for every cell. <c>TCell</c> must implement <see cref="IHasSvgGeometry{TCell}"/>.</summary>
        public Func<TCell, string> PerCell = null;
    }
}
