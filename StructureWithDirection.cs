using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a structure of connected cells, such as a grid, in which there is a consistent set of directions one can
    ///     move from each cell.</summary>
    /// <typeparam name="TCell">
    ///     The type of cells in the structure; for example, <see cref="Coord"/> or <see cref="Hex"/>.</typeparam>
    /// <typeparam name="TDirection">
    ///     The type (usually an enum) identifying a direction; for example, <see cref="GridDirection"/> or <see
    ///     cref="HexDirection"/>.</typeparam>
    public class StructureWithDirection<TCell, TDirection> : Structure<TCell> where TCell : IHasDirection<TCell, TDirection>
    {
        /// <summary>
        ///     Constructs a structure with the specified cells and links between them.</summary>
        /// <remarks>
        ///     Please refer to <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}},
        ///     Func{TCell, IEnumerable{TCell}})"/> for restrictions on the parameter values.</remarks>
        public StructureWithDirection(IEnumerable<TCell> cells, IEnumerable<Link<TCell>> links = null, Func<TCell, IEnumerable<TCell>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        /// <summary>
        ///     Determines whether it is possible within the current structure to move the specified number of steps in the
        ///     specified direction.</summary>
        /// <param name="cell">
        ///     Starting cell.</param>
        /// <param name="direction">
        ///     Direction to attempt to move in.</param>
        /// <param name="amount">
        ///     Number of steps to move.</param>
        public bool CanMove(TCell cell, TDirection direction, int amount = 1)
        {
            var c = cell;
            for (var i = 0; i < amount; i++)
            {
                var newC = c.Move(direction, amount);
                if (!_cells.Contains(c) || !_links.Contains(new Link<TCell>(c, newC)))
                    return false;
                c = newC;
            }
            return true;
        }
    }
}
