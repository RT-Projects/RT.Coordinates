using System;
using System.Collections;
using System.Collections.Generic;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>Contains instructions to control the behavior of <see cref="Structure{TCell}.Svg(SvgInstructions)"/>.</summary>
    public class SvgInstructions
    {
        /// <summary>Adds extra SVG code for every cell before the SVG for the outline and walls.</summary>
        public Func<object, string> PerCellBefore = null;

        /// <summary>Adds extra SVG code for every cell after the SVG for the outline and walls.</summary>
        public Func<object, string> PerCellAfter = null;

        /// <summary>
        ///     Paints some cells with a background color.</summary>
        /// <remarks>
        ///     <para>
        ///         Possible values for this field are:</para>
        ///     <list type="bullet">
        ///         <item><description>
        ///             Any <see cref="ICollection"/> (for example, a <see cref="HashSet{T}"/>) containing the cells. This
        ///             will color each cell in the color specified by <see cref="HighlightColor"/>. Cells that are not part
        ///             of this structure, or any other objects, are ignored.</description></item>
        ///         <item><description>
        ///             Any <see cref="IDictionary"/> in which the keys are the cells. The values can be <see
        ///             cref="SvgColor"/> objects, or they are converted to strings and assumed to be SVG colors without
        ///             opacity. Cells that are not part of this structure, or any other keys, are ignored.</description></item>
        ///         <item><description>
        ///             A <c>Func&lt;object, <see cref="SvgColor"/>&gt;</c>, <c>Func&lt;object, string&gt;</c>, or
        ///             <c>Func&lt;object, object&gt;</c> which returns the color information for each cell. The function will
        ///             be invoked for every cell in the structure.</description></item></list></remarks>
        public object HighlightCells = null;

        /// <summary>Specifies a default color used when highlighting cells specified by <see cref="HighlightCells"/>.</summary>
        public SvgColor HighlightColor = new("hsl(284, 83%, 85%)", null);

        /// <summary>
        ///     Returns a collection of edges (line segments) that make up the perimeter of the specified cell. If this is
        ///     absent or returns <c>null</c>, the cell must implement <see cref="IHasSvgGeometry"/>.</summary>
        public Func<object, IEnumerable<Edge>> GetEdges;

        /// <summary>
        ///     Returns the center point for the specified cell. If this is absent or returns <c>null</c>, the cell must
        ///     implement <see cref="IHasSvgGeometry"/>.</summary>
        public Func<object, PointD?> GetCenter;

        /// <summary>Returns the 2D point for the specified vertex.</summary>
        public Func<Vertex, PointD> GetVertexPoint = v => v.Point;

        /// <summary>
        ///     Returns the type of edge to draw in SVG for a particular pair of vertices. The second parameter receives a
        ///     list of the cells that share the edge; this will be of runtime type
        ///     <c>System.Collections.Generic.List&lt;TCell&gt;</c>, where <c>TCell</c> is the generic type argument used on
        ///     <see cref="Structure{TCell}"/>.</summary>
        /// <remarks>
        ///     In cases where this is specified and the <see cref="Structure{TCell}"/> derived class also overrides <see
        ///     cref="Structure{TCell}.svgEdgeType(Link{Vertex}, List{TCell})"/>, this delegate takes precedence.</remarks>
        public Func<Link<Vertex>, IList, EdgeInfo<object>> GetEdgeType = null;

        /// <summary>
        ///     Specifies the attributes on the main <c>&lt;svg&gt;</c> tag. Set this to <c>null</c> to omit the SVG tag
        ///     entirely.</summary>
        public string SvgAttributes = "xmlns='http://www.w3.org/2000/svg' viewBox='{0} {1} {2} {3}' font-size='.2' text-anchor='middle'";

        /// <summary>Provides some additional SVG code to add at the start of the file.</summary>
        public string ExtraSvg1;

        /// <summary>
        ///     Provides some additional SVG code to add into the file after the cell highlights (<see
        ///     cref="HighlightCells"/>) but before the main grid.</summary>
        public string ExtraSvg2;

        /// <summary>
        ///     Provides some additional SVG code to add into the file after the main grid but before <see
        ///     cref="PerCellAfter"/>.</summary>
        public string ExtraSvg3;

        /// <summary>Provides some additional SVG code to add at the end of the file.</summary>
        public string ExtraSvg4;

        /// <summary>
        ///     If <c>true</c>, <see cref="PassagesPaths"/> is used to render each passable edge separately. Otherwise, <see
        ///     cref="PassagesPath"/> is used to render all passable edges as a single SVG path.</summary>
        public bool PassagesSeparate = false;

        /// <summary>
        ///     Generates the SVG path object for passable edges between cells, assuming <see cref="PassagesSeparate"/> is
        ///     <c>false</c>. The function is invoked once only. The function parameter is the path’s <c>d</c> attribute,
        ///     describing the entirety of all passages in the structure.</summary>
        public Func<string, string> PassagesPath = d => $"<path d='{d}' fill='none' stroke-width='.02' stroke='#aaa' stroke-dasharray='.1' />";

        /// <summary>
        ///     Generates SVG path objects for passable edges between cells, assuming <see cref="PassagesSeparate"/> is
        ///     <c>true</c>. The function is invoked once for each separate passable edge. The first function parameter is the
        ///     path’s <c>d</c> attribute. The remaining parameters are the two cells connected by the passage, in no
        ///     particular order.</summary>
        public Func<string, object, object, string> PassagesPaths = (d, c1, c2) => $"<path d='{d}' fill='none' stroke-width='.02' stroke='#aaa' stroke-dasharray='.1' />";

        /// <summary>
        ///     If <c>true</c>, <see cref="WallsPaths"/> is used to render each impassable edge (wall) separately. Otherwise,
        ///     <see cref="WallsPath"/> is used to render all impassable edges (walls) as a single SVG path.</summary>
        public bool WallsSeparate = false;

        /// <summary>
        ///     Generates the SVG path object for impassable edges (walls) between cells, assuming <see cref="WallsSeparate"/>
        ///     is <c>false</c>. The function is invoked once only. The function parameter is the path’s <c>d</c> attribute,
        ///     describing the entirety of all walls in the structure.</summary>
        public Func<string, string> WallsPath = d => $"<path d='{d}' fill='none' stroke-width='.05' stroke='black' />";

        /// <summary>
        ///     Generates SVG path objects for impassable edges (walls) between cells, assuming <see cref="WallsSeparate"/> is
        ///     <c>true</c>. The function is invoked once for each separate impassable edge (wall). The first function
        ///     parameter is the path’s <c>d</c> attribute. The remaining parameters are the two cells on either side of the
        ///     wall, in no particular order.</summary>
        public Func<string, object, object, string> WallsPaths = (d, c1, c2) => $"<path d='{d}' fill='none' stroke-width='.05' stroke='black' />";

        /// <summary>
        ///     If <c>true</c>, <see cref="OutlinePaths"/> is used to render each edge of the outline (perimeter) separately.
        ///     Otherwise, <see cref="OutlinePath"/> is used to render the entire outline as a single SVG path.</summary>
        public bool OutlineSeparate = false;

        /// <summary>
        ///     Generates the SVG path object for the structure’s outline (perimeter), assuming <see cref="OutlineSeparate"/>
        ///     is <c>false</c>. The function is invoked once only. The function parameter is the path’s <c>d</c> attribute,
        ///     describing the entirety of the outline of the structure.</summary>
        public Func<string, string> OutlinePath = d => $"<path d='{d}' fill='none' stroke-width='.1' stroke='black' />";

        /// <summary>
        ///     Generates SVG path objects for each segment of the outline (perimeter), assuming <see cref="OutlineSeparate"/>
        ///     is <c>true</c>. The function is invoked once for each separate edge along the perimeter. The first function
        ///     parameter is the path’s <c>d</c> attribute. The second parameter is the cell the edge belongs to.</summary>
        public Func<string, object, string> OutlinePaths = (d, c) => $"<path d='{d}' fill='none' stroke-width='.1' stroke='black' />";

        /// <summary>
        ///     Generates SVG code for a bridge — a connection between cells that have no edges in common. The first two
        ///     parameters are the cells; the third is a default SVG path <c>d</c> attribute that can optionally be used.</summary>
        public Func<object, object, string, string> BridgeSvg = null;

        /// <summary>Specifies a number of decimal places to round every coordinate to when generating SVG paths.</summary>
        public int? Precision = null;

        /// <summary>Amount of space to leave on the left and right side of the grid (not including the stroke width).</summary>
        public double MarginX = .1;

        /// <summary>Amount of space to leave on the top and bottom of the grid (not including the stroke width).</summary>
        public double MarginY = .1;

        /// <summary>Provides a default implementation for <see cref="BridgeSvg"/>.</summary>
        public static string DrawBridge(PointD center1, PointD center2, Func<double, string> r)
        {
            var control1 = ((center1 * 2 + center2) / 3 - center1).RotateDeg(30) + center1;
            var control2 = ((center1 + center2 * 2) / 3 - center2).RotateDeg(-30) + center2;
            var d = $"M{r(center1.X)} {r(center1.Y)}C{r(control1.X)} {r(control1.Y)} {r(control2.X)} {r(control2.Y)} {r(center2.X)} {r(center2.Y)}";
            return $"<path d='{d}' fill='none' stroke-width='.3' stroke='black' /><path d='{d}' fill='none' stroke-width='.2' stroke='white' stroke-linecap='round' />";
        }

        /// <summary>
        ///     Rounds a floating-point value to a number of decimal places specified by <see cref="Precision"/>. Useful to
        ///     pass into methods such as <see cref="GridUtils.SvgEdgesPath(IEnumerable{Link{Vertex}}, Func{Vertex, PointD},
        ///     Func{double, string})"/>.</summary>
        /// <param name="value">
        ///     The floating-point value to be rounded.</param>
        public string Round(double value) => Precision == null ? value.ToString() : value.ToString($"0.{new string('0', Precision.Value)}");
    }
}
