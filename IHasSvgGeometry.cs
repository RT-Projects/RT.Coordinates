using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Exposes a sequence of vertices that allow a cell to be rendered in SVG.</summary>
    public interface IHasSvgGeometry
    {
        /// <summary>Returns a sequence of vertices in the order in which they must be rendered.</summary>
        public IEnumerable<Vertex> Vertices { get; }

        /// <summary>Returns the center-point of the cell.</summary>
        public PointD Center { get; }
    }
}
