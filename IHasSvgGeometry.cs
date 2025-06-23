using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Exposes properties that allow a cell to be rendered in SVG.</summary>
    public interface IHasSvgGeometry
    {
        /// <summary>Returns a sequence of edges (line segments) describing the perimeter of the cell.</summary>
        IEnumerable<Link<Vertex>> Edges { get; }

        /// <summary>Returns the center-point of the cell.</summary>
        PointD Center { get; }
    }
}
