using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Describes a 2D grid of triangular cells.</summary>
    public class TriGrid : Structure<Tri>
    {
        /// <summary>
        ///     Constructs a grid of the specified width and height in which the top-left triangle is an up-pointing one.</summary>
        /// <param name="width">
        ///     Number of triangles per row.</param>
        /// <param name="height">
        ///     Number of rows.</param>
        public TriGrid(int width, int height) : this(Enumerable.Range(0, width * height).Select(i => new Tri(i % width, i / width)))
        {
        }

        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public TriGrid(IEnumerable<Tri> cells, IEnumerable<Link<Tri>> links = null, Func<Tri, IEnumerable<Tri>> getNeighbors = null) : base(cells, links, getNeighbors)
        {
        }
    }
}
