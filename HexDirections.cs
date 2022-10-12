using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Contains some helpers relating to <see cref="HexDirection"/> enum values.</summary>
    public static class HexDirections
    {
        /// <summary>Provides a collection of all hexagonal directions.</summary>
        public static readonly IEnumerable<HexDirection> All = (HexDirection[]) Enum.GetValues(typeof(HexDirection));

        /// <summary>
        ///     Returns a new <see cref="HexDirection"/> which is the specified multiple of 60° clockwise from the current
        ///     direction.</summary>
        /// <param name="dir">
        ///     Original starting direction.</param>
        /// <param name="amount">
        ///     Number of 60° turns to perform. Use negative numbers to go counter-clockwise.</param>
        public static HexDirection Clockwise(this HexDirection dir, int amount = 1) => (HexDirection) ((((int) dir + amount) % 6 + 6) % 6);
    }
}
