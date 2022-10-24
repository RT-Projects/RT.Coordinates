using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a grid structure consisting of <see cref="Floret"/> cells that join up in groups of 6 to form hexagons,
    ///     which in turn tile the plane.</summary>
    public class FloretGrid : Structure<Floret>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public FloretGrid(IEnumerable<Floret> cells, IEnumerable<Link<Floret>> links = null, Func<Floret, IEnumerable<Floret>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs a <see cref="FloretGrid"/> consisting of a hexagonal grid of the specified <paramref
        ///     name="sideLength"/>.</summary>
        public FloretGrid(int sideLength)
            : base(Hex.LargeHexagon(sideLength).SelectMany(hex => _floretPositions.Select(pos => new Floret(hex, pos))))
        {
        }

        private static readonly Floret.Position[] _floretPositions = (Floret.Position[]) Enum.GetValues(typeof(Floret.Position));

        /// <inheritdoc/>
        protected override Structure<Floret> makeModifiedStructure(IEnumerable<Floret> cells, IEnumerable<Link<Floret>> traversible) => new FloretGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new FloretGrid GenerateMaze(Random rnd = null) => (FloretGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new FloretGrid GenerateMaze(Func<int, int, int> rndNext) => (FloretGrid) base.GenerateMaze(rndNext);
    }
}
