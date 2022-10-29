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

        /// <summary>
        ///     Determines whether it is possible within <paramref name="structure"/> to move the specified <paramref
        ///     name="amount"/> of steps in the specified <paramref name="direction"/> and sets <paramref name="newCell"/> to
        ///     the cell landed on.</summary>
        /// <param name="structure">
        ///     The structure to examine.</param>
        /// <param name="cell">
        ///     Starting cell.</param>
        /// <param name="direction">
        ///     Direction to attempt to move in.</param>
        /// <param name="newCell">
        ///     Receives the cell landed on.</param>
        /// <param name="amount">
        ///     Number of steps to move.</param>
        public static bool TryMove<TCell, TDirection>(this Structure<TCell> structure, TCell cell, TDirection direction, out TCell newCell, int amount = 1)
            where TCell : IHasDirection<TCell, TDirection>
        {
            newCell = cell;
            for (var i = 0; i < amount; i++)
            {
                var c = newCell.Move(direction, amount);
                if (!structure.Cells.Contains(c) || !structure.Links.Contains(new Link<TCell>(newCell, c)))
                {
                    newCell = default;
                    return false;
                }
                newCell = c;
            }
            return true;
        }

        /// <summary>
        ///     Determines whether it is possible within <paramref name="structure"/> to move the specified <paramref
        ///     name="amount"/> of steps in the specified <paramref name="direction"/>.</summary>
        /// <param name="structure">
        ///     The structure to examine.</param>
        /// <param name="cell">
        ///     Starting cell.</param>
        /// <param name="direction">
        ///     Direction to attempt to move in.</param>
        /// <param name="amount">
        ///     Number of steps to move.</param>
        public static bool CanMove<TCell, TDirection>(this Structure<TCell> structure, TCell cell, TDirection direction, int amount = 1)
            where TCell : IHasDirection<TCell, TDirection> => TryMove(structure, cell, direction, out _, amount);

        /// <summary>
        ///     Moves the specified <paramref name="amount"/> of steps in the specified <paramref name="direction"/> within
        ///     <paramref name="structure"/> and returns the cell landed on.</summary>
        /// <param name="structure">
        ///     The structure within which to move.</param>
        /// <param name="cell">
        ///     Starting cell.</param>
        /// <param name="direction">
        ///     Direction to attempt to move in.</param>
        /// <param name="amount">
        ///     Number of steps to move.</param>
        /// <exception cref="InvalidOperationException">
        ///     It is not possible to move the specified number of steps.</exception>
        public static TCell Move<TCell, TDirection>(this Structure<TCell> structure, TCell cell, TDirection direction, int amount = 1)
            where TCell : IHasDirection<TCell, TDirection> => TryMove(structure, cell, direction, out var newCell, amount)
                ? newCell
                : throw new InvalidOperationException("The structure does not allow movement for that many steps in that direction.");

        /// <summary>
        ///     Returns the maximum number of steps it is possible to move in the specified <paramref name="direction"/>
        ///     within <paramref name="structure"/>.</summary>
        /// <param name="structure">
        ///     The structure within which to move.</param>
        /// <param name="cell">
        ///     Starting cell.</param>
        /// <param name="direction">
        ///     Direction to attempt to move in.</param>
        /// <param name="lastCell">
        ///     Receives the final cell landed on.</param>
        /// <exception cref="InvalidOperationException">
        ///     The structure contains a cycle that causes the movement to loop back on itself.</exception>
        public static int MaxMoves<TCell, TDirection>(this Structure<TCell> structure, TCell cell, TDirection direction, out TCell lastCell)
            where TCell : IHasDirection<TCell, TDirection>
        {
            lastCell = cell;
            var already = new HashSet<TCell> { cell };
            var amount = 0;
            while (true)
            {
                var c = lastCell.Move(direction);
                if (!already.Add(c))
                    throw new InvalidOperationException("The structure contains a cycle that causes the movement to loop back on itself.");
                if (!structure.Cells.Contains(c) || !structure.Links.Contains(new Link<TCell>(lastCell, c)))
                    return amount;
                amount++;
                lastCell = c;
            }
        }

        /// <summary>
        ///     Returns a new <see cref="Coord.Direction"/> which is the specified multiple of 45° clockwise from the current
        ///     direction.</summary>
        /// <param name="dir">
        ///     Original starting direction.</param>
        /// <param name="amount">
        ///     Number of 45° turns to perform. Use negative numbers to go counter-clockwise.</param>
        public static Coord.Direction Clockwise(this Coord.Direction dir, int amount = 1) => (Coord.Direction) ((((int) dir + amount) % 8 + 8) % 8);

        /// <summary>
        ///     Returns a new <see cref="Hex.Direction"/> which is the specified multiple of 60° clockwise from the current
        ///     direction.</summary>
        /// <param name="dir">
        ///     Original starting direction.</param>
        /// <param name="amount">
        ///     Number of 60° turns to perform. Use negative numbers to go counter-clockwise.</param>
        public static Hex.Direction Clockwise(this Hex.Direction dir, int amount = 1) => (Hex.Direction) ((((int) dir + amount) % 6 + 6) % 6);
    }
}
