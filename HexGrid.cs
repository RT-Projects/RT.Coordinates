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
    }
}
