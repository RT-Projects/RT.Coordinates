using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Contains some extension methods.</summary>
    public static class Extensions
    {
        /// <summary>
        ///     Given the output from <see cref="Structure{TCell}.FindPath(TCell, TCell)"/>, reconstructs a path from the
        ///     origin cell to the specified <paramref name="destination"/> cell.</summary>
        /// <typeparam name="TCell">
        ///     Type of cell (e.g., <see cref="Coord"/>, <see cref="Hex"/> or <see cref="Tri"/>).</typeparam>
        /// <param name="paths">
        ///     The output from <see cref="Structure{TCell}.FindPath(TCell, TCell)"/>.</param>
        /// <param name="destination">
        ///     The destination cell to reconstruct the path to.</param>
        /// <returns>
        ///     A collection containing every cell along the path, including the origin cell and <paramref
        ///     name="destination"/>.</returns>
        /// <exception cref="InvalidOperationException">
        ///     The specified dictionary does not contain the required information to reconstruct the path.</exception>
        public static IEnumerable<TCell> GetPathTo<TCell>(this Dictionary<TCell, CellWithDistance<TCell>> paths, TCell destination)
        {
            var result = new List<TCell> { destination };
            var cell = destination;
            while (true)
            {
                if (!paths.TryGetValue(cell, out var cwd))
                    throw new InvalidOperationException($"The path leads to the cell {cell} which is not in the provided dictionary.");
                if (cwd.Distance == 0)
                {
                    result.Reverse();
                    return result;
                }
                result.Add(cell = cwd.Parent);
            }
        }

        /// <summary>
        ///     Returns a new structure in which the specified set of cells is combined (merged) into a single cell.</summary>
        /// <param name="structure">
        ///     Structure to operate upon.</param>
        /// <param name="cells">
        ///     Set of cells to combine into one.</param>
        /// <remarks>
        ///     See <see cref="Structure{TCell}.CombineCells(TCell[])"/> for a code example.</remarks>
        /// <exception cref="InvalidOperationException">
        ///     One of the cells in <paramref name="cells"/> has already been combined with other cells within this structure.</exception>
        public static Structure<CombinedCell<TCell>> CombineCells<TCell>(this Structure<CombinedCell<TCell>> structure, params TCell[] cells) => CombineCells(structure, (IEnumerable<TCell>) cells);

        /// <summary>
        ///     Returns a new structure in which the specified set of cells is combined (merged) into a single cell.</summary>
        /// <param name="structure">
        ///     Structure to operate upon.</param>
        /// <param name="cells">
        ///     Set of cells to combine into one.</param>
        /// <remarks>
        ///     See <see cref="Structure{TCell}.CombineCells(TCell[])"/> for a code example.</remarks>
        /// <exception cref="InvalidOperationException">
        ///     One of the cells in <paramref name="cells"/> has already been combined with other cells within this structure.</exception>
        public static Structure<CombinedCell<TCell>> CombineCells<TCell>(this Structure<CombinedCell<TCell>> structure, IEnumerable<TCell> cells)
        {
            var cellsArr = (cells as IList<TCell>) ?? cells.ToArray();
            var already = cellsArr.IndexOf(c => structure.Cells.Any(c2 => c2.Count > 1 && c2.Contains(c)));
            if (already != -1)
                throw new InvalidOperationException($"Cell {cellsArr[already]} is already in a combination with other cells.");
            var combo = new CombinedCell<TCell>(cellsArr);
            return new Structure<CombinedCell<TCell>>(
                structure.Cells.Where(c => c.Count > 1 || !combo.Contains(c.First())).Concat(new CombinedCell<TCell>[] { combo }),
                structure.Links.Select(link =>
                {
                    var cc1 = link.Cells.First();
                    var cc2 = link.Other(cc1);
                    var c1 = cc1.First();
                    var c2 = cc2.First();
                    if (cc1.Count == 1 && combo.Contains(c1))
                        return cc2.Count == 1 && combo.Contains(c2) ? null : new Link<CombinedCell<TCell>>(combo, cc2).Nullable();
                    if (cc2.Count == 1 && combo.Contains(c2))
                        return new Link<CombinedCell<TCell>>(cc1, combo).Nullable();
                    return link.Nullable();
                }).WhereNotNull());
        }

        /// <summary>
        ///     Generates a set of edges from a collection of vertices in which the vertices are in the correct order (either
        ///     clockwise or counter-clockwise).</summary>
        public static IEnumerable<Link<Vertex>> MakeEdges(this IEnumerable<Vertex> vertices) => vertices.SelectConsecutivePairs(true, (v1, v2) => new Link<Vertex>(v1, v2));
    }
}
