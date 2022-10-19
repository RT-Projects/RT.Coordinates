using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes a 2D grid of flat-topped hexagonal cells.</summary>
    public class HexGrid : StructureWithDirection<Hex, HexDirection>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public HexGrid(IEnumerable<Hex> cells, IEnumerable<Link<Hex>> links = null, Func<Hex, IEnumerable<Hex>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Constructs a hexagonal grid of the specified <paramref name="sideLength"/>.</summary>
        /// <param name="sideLength">
        ///     Size of the grid.</param>
        public HexGrid(int sideLength)
            : base(Hex.LargeHexagon(sideLength))
        {
        }

        /// <inheritdoc/>
        protected override Structure<Hex> makeModifiedStructure(IEnumerable<Hex> cells, IEnumerable<Link<Hex>> traversible) => new HexGrid(cells, traversible);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new HexGrid GenerateMaze(Random rnd = null) => (HexGrid) base.GenerateMaze(rnd);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public new HexGrid GenerateMaze(Func<int, int, int> rndNext) => (HexGrid) base.GenerateMaze(rndNext);
    }
}
