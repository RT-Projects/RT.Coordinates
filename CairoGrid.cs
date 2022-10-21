using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a grid structure consisting of <see cref="Cairo"/> cells that join up in groups of 4 to form (vertically
    ///     stretched) hexagons, which in turn tile the plane.</summary>
    public class CairoGrid : Structure<Cairo>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public CairoGrid(IEnumerable<Cairo> cells, IEnumerable<Link<Cairo>> links = null, Func<Cairo, IEnumerable<Cairo>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs a <see cref="CairoGrid"/> consisting of a hexagonal grid of the specified <paramref
        ///     name="sideLength"/>.</summary>
        public CairoGrid(int sideLength)
            : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _cairoPositions.Select(pos => new Cairo(hex, pos))))
        {
        }

        private static readonly Cairo.Position[] _cairoPositions = (Cairo.Position[]) Enum.GetValues(typeof(Cairo.Position));

        /// <inheritdoc/>
        protected override Structure<Cairo> makeModifiedStructure(IEnumerable<Cairo> cells, IEnumerable<Link<Cairo>> traversible) => new CairoGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new CairoGrid GenerateMaze(Random rnd = null) => (CairoGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new CairoGrid GenerateMaze(Func<int, int, int> rndNext) => (CairoGrid) base.GenerateMaze(rndNext);
    }
}
