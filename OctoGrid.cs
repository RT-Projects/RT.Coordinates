using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes a 2D grid of octagonal cells with (diagonal) squares filling the gaps.</summary>
    public class OctoGrid : Structure<OctoCell>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public OctoGrid(IEnumerable<OctoCell> cells, IEnumerable<Link<OctoCell>> links = null, Func<OctoCell, IEnumerable<OctoCell>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs an <see cref="OctoGrid"/> that forms a rectangle. Along the perimeter all the cells will be
        ///     octagons. Only squares internal to these octagons are included.</summary>
        /// <param name="width">
        ///     The number of octagons in the x direction.</param>
        /// <param name="height">
        ///     The number of octagons in the y direction.</param>
        public OctoGrid(int width, int height)
            : base(OctoCell.LargeRectangle(width, height))
        {
        }

        /// <inheritdoc/>
        protected override Structure<OctoCell> makeModifiedStructure(IEnumerable<OctoCell> cells, IEnumerable<Link<OctoCell>> traversible) => new OctoGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new OctoGrid GenerateMaze(Random rnd = null) => (OctoGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new OctoGrid GenerateMaze(Func<int, int, int> rndNext) => (OctoGrid) base.GenerateMaze(rndNext);
    }
}
