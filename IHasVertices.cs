using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Exposes a sequence of vertices that allow a cell to be rendered in SVG.</summary>
    public interface IHasVertices<TCell> where TCell : IEquatable<TCell>
    {
        /// <summary>Returns a sequence of vertices in the order in which they must be rendered.</summary>
        public IEnumerable<Vertex<TCell>> Vertices { get; }
    }
}
