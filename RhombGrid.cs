using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a grid structure consisting of <see cref="Rhomb"/> cells that join up in groups of 3 to form hexagons,
    ///     which in turn tile the plane.</summary>
    public class RhombGrid : Structure<Rhomb>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public RhombGrid(IEnumerable<Rhomb> cells, IEnumerable<Link<Rhomb>> links = null, Func<Rhomb, IEnumerable<Rhomb>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs a <see cref="RhombGrid"/> consisting of a hexagonal grid of the specified <paramref
        ///     name="sideLength"/>.</summary>
        public RhombGrid(int sideLength)
            : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _rhombPositions.Select(pos => new Rhomb(hex, pos))))
        {
        }

        private static readonly Rhomb.Position[] _rhombPositions = (Rhomb.Position[]) Enum.GetValues(typeof(Rhomb.Position));

        /// <inheritdoc/>
        protected override Structure<Rhomb> makeModifiedStructure(IEnumerable<Rhomb> cells, IEnumerable<Link<Rhomb>> traversible) => new RhombGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new RhombGrid GenerateMaze(Random rnd = null) => (RhombGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new RhombGrid GenerateMaze(Func<int, int, int> rndNext) => (RhombGrid) base.GenerateMaze(rndNext);
    }
}
