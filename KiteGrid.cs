using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a grid structure consisting of <see cref="Kite"/> cells that join up in groups of 6 to form hexagons,
    ///     which in turn tile the plane.</summary>
    public class KiteGrid : Structure<Kite>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public KiteGrid(IEnumerable<Kite> cells, IEnumerable<Link<Kite>> links = null, Func<Kite, IEnumerable<Kite>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs a <see cref="KiteGrid"/> consisting of a hexagonal grid of the specified <paramref
        ///     name="sideLength"/>.</summary>
        public KiteGrid(int sideLength)
            : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _kitePositions.Select(pos => new Kite(hex, pos))))
        {
        }

        private static readonly Kite.Position[] _kitePositions = (Kite.Position[]) Enum.GetValues(typeof(Kite.Position));

        /// <inheritdoc/>
        protected override Structure<Kite> makeModifiedStructure(IEnumerable<Kite> cells, IEnumerable<Link<Kite>> traversible) => new KiteGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new KiteGrid GenerateMaze(Random rnd = null) => (KiteGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new KiteGrid GenerateMaze(Func<int, int, int> rndNext) => (KiteGrid) base.GenerateMaze(rndNext);
    }
}
