using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Contains some helpers relating to <see cref="GridDirection"/> enum values.</summary>
    public static class GridDirections
    {
        /// <summary>Provides a collection of all orthogonal directions.</summary>
        public static readonly IEnumerable<GridDirection> Orthogonal = new[] { GridDirection.Up, GridDirection.Right, GridDirection.Down, GridDirection.Left };
        /// <summary>Provides a collection of all diagonal directions.</summary>
        public static readonly IEnumerable<GridDirection> Diagonal = new[] { GridDirection.UpRight, GridDirection.DownRight, GridDirection.DownLeft, GridDirection.UpLeft };
        /// <summary>Provides a collection of all directions.</summary>
        public static readonly IEnumerable<GridDirection> All = (GridDirection[]) Enum.GetValues(typeof(GridDirection));

        /// <summary>
        ///     Returns a new <see cref="GridDirection"/> which is the specified multiple of 45° clockwise from the current
        ///     direction.</summary>
        /// <param name="dir">
        ///     Original starting direction.</param>
        /// <param name="amount">
        ///     Number of 45° turns to perform. Use negative numbers to go counter-clockwise.</param>
        public static GridDirection Clockwise(this GridDirection dir, int amount = 1) => (GridDirection) ((((int) dir + amount) % 8 + 8) % 8);
    }
}
