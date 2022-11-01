using System;
using System.Collections;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Contains instructions to control the behavior of <see cref="Structure{TCell}.Svg(SvgInstructions)"/>.</summary>
    public class SvgInstructions
    {
        /// <summary>Adds extra SVG code for every cell.</summary>
        public Func<object, string> PerCell = null;

        /// <summary>
        ///     Instructs <see cref="Structure{TCell}.Svg(SvgInstructions)"/> to paint some cells with a background color.</summary>
        /// <remarks>
        ///     <para>
        ///         Possible values for this field are:</para>
        ///     <list type="bullet">
        ///         <item><description>
        ///             Any <see cref="ICollection"/> (for example, a <see cref="HashSet{T}"/>) containing the cells. This
        ///             will color each cell in the color specified by <see cref="HighlightColor"/>. Cells that are not part
        ///             of this structure, or any other objects, are ignored.</description></item>
        ///         <item><description>
        ///             An <see cref="IDictionary"/> in which the keys are the cells. The values can be <see cref="SvgColor"/>
        ///             objects, or they are converted to strings and assumed to be SVG colors without opacity. Cells that are
        ///             not part of this structure, or any other keys, are ignored.</description></item>
        ///         <item><description>
        ///             A <c>Func&lt;object, <see cref="SvgColor"/>&gt;</c>, <c>Func&lt;object, string&gt;</c>, or
        ///             <c>Func&lt;object, object&gt;</c> which returns the color information for each cell. The function will
        ///             be invoked for every cell in the structure.</description></item></list></remarks>
        public object HighlightCells = null;

        /// <summary>Specifies a default color used when highlighting cells specified by <see cref="HighlightCells"/>.</summary>
        public SvgColor HighlightColor = new SvgColor("hsl(284, 83%, 85%)", null);

        /// <summary>
        ///     Returns a collection of edges (line segments) that make up the perimeter of the specified cell. If this is
        ///     absent or returns <c>null</c>, the cell must implement <see cref="IHasSvgGeometry"/>.</summary>
        public Func<object, IEnumerable<Link<Vertex>>> GetEdges;

        /// <summary>
        ///     Returns the center point for the specified cell. If this is absent or returns <c>null</c>, the cell must
        ///     implement <see cref="IHasSvgGeometry"/>.</summary>
        public Func<object, PointD?> GetCenter;

        /// <summary>Returns the 2D point for the specified vertex.</summary>
        public Func<Vertex, PointD> GetVertexPoint = v => v.Point;

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
