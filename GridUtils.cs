using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RT.Geometry;

namespace RT.Coordinates;

/// <summary>Contains some extension methods.</summary>
public static class GridUtils
{
    /// <summary>
    ///     Given the output from <see cref="Structure{TCell}.FindPath(TCell, TCell)"/>, reconstructs a path from the origin
    ///     cell to the specified <paramref name="destination"/> cell.</summary>
    /// <typeparam name="TCell">
    ///     Type of cell (e.g., <see cref="Square"/>, <see cref="Hex"/> or <see cref="Tri"/>).</typeparam>
    /// <param name="paths">
    ///     The output from <see cref="Structure{TCell}.FindPath(TCell, TCell)"/>.</param>
    /// <param name="destination">
    ///     The destination cell to reconstruct the path to.</param>
    /// <returns>
    ///     A collection containing every cell along the path, including the origin cell and <paramref name="destination"/>.</returns>
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
        var rawCells = new HashSet<TCell>(cells);
        var cellsToCombine = new HashSet<TCell>();
        var newCells = new HashSet<CombinedCell<TCell>>(structure.Cells);

        while (rawCells.Count > 0)
        {
            var rawCell = rawCells.First();
            if (newCells.FirstOrNull(c => c.Contains(rawCell)) is not { } superCell)
                throw new InvalidOperationException($"Cell {rawCell} is not part of this structure.");
            if (!superCell.All(rawCells.Contains))
                throw new InvalidOperationException($"Cell {rawCell} is already in a combination with other cells.");
            foreach (var subcell in superCell)
            {
                rawCells.Remove(subcell);
                cellsToCombine.Add(subcell);
            }
            newCells.Remove(superCell);
        }

        var combo = new CombinedCell<TCell>(cellsToCombine);

        return new Structure<CombinedCell<TCell>>(
            newCells.Concat([combo]),
            structure.Links.Select(link =>
            {
                var (cc1, cc2) = link;
                var o1 = cellsToCombine.Contains(cc1.First());
                var o2 = cellsToCombine.Contains(cc2.First());
                return
                    o1 && o2 ? null :
                    o1 ? new Link<CombinedCell<TCell>>(combo, cc2) :
                    o2 ? new Link<CombinedCell<TCell>>(combo, cc1) :
                    link.Nullable();
            }).WhereNotNull());
    }

    /// <summary>
    ///     Generates a set of edges from a collection of vertices in which the vertices are in the correct order (either
    ///     clockwise or counter-clockwise).</summary>
    public static IEnumerable<Edge> MakeEdges(this IEnumerable<Vertex> vertices) => vertices.SelectConsecutivePairs(true, (v1, v2) => new Edge(v1, v2));

    /// <summary>
    ///     Determines whether it is possible within <paramref name="structure"/> to move the specified <paramref
    ///     name="amount"/> of steps in the specified <paramref name="direction"/> and sets <paramref name="newCell"/> to the
    ///     cell landed on.</summary>
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
    ///     Returns the maximum number of steps it is possible to move in the specified <paramref name="direction"/> within
    ///     <paramref name="structure"/>.</summary>
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
    ///     Returns a new <see cref="Square.Direction"/> which is the specified multiple of 45° clockwise from the current
    ///     direction.</summary>
    /// <param name="dir">
    ///     Original starting direction.</param>
    /// <param name="amount">
    ///     Number of 45° turns to perform. Use negative numbers to go counter-clockwise.</param>
    public static Square.Direction Clockwise(this Square.Direction dir, int amount = 1) => (Square.Direction) ((((int) dir + amount) % 8 + 8) % 8);

    /// <summary>
    ///     Returns a new <see cref="Hex.Direction"/> which is the specified multiple of 60° clockwise from the current
    ///     direction.</summary>
    /// <param name="dir">
    ///     Original starting direction.</param>
    /// <param name="amount">
    ///     Number of 60° turns to perform. Use negative numbers to go counter-clockwise.</param>
    public static Hex.Direction Clockwise(this Hex.Direction dir, int amount = 1) => (Hex.Direction) ((((int) dir + amount) % 6 + 6) % 6);

    /// <summary>
    ///     Returns an SVG path string (usable in the <c>d</c> attribute of a <c>&lt;path&gt;</c> element) that draws all of
    ///     the specified edges.</summary>
    /// <param name="edges">
    ///     A set of edges to render.</param>
    /// <param name="getVertexPoint">
    ///     An optional function that can customize the coordinates of each vertex.</param>
    /// <param name="r">
    ///     Provides a means to turn floating-point values into strings. <see cref="Structure{TCell}.Svg(SvgInstructions)"/>
    ///     uses this to respect the user’s <see cref="SvgInstructions.Precision"/> instruction.</param>
    public static string SvgEdgesPath(IEnumerable<Edge> edges, Func<Vertex, PointD> getVertexPoint = null, Func<double, string> r = null) =>
        SvgEdgesPath(edges.Select(e => e.ToLink()), getVertexPoint, r);

    /// <summary>
    ///     Returns an SVG path string (usable in the <c>d</c> attribute of a <c>&lt;path&gt;</c> element) that draws all of
    ///     the specified edges.</summary>
    /// <param name="edges">
    ///     A set of edges to render.</param>
    /// <param name="getVertexPoint">
    ///     An optional function that can customize the coordinates of each vertex.</param>
    /// <param name="r">
    ///     Provides a means to turn floating-point values into strings. <see cref="Structure{TCell}.Svg(SvgInstructions)"/>
    ///     uses this to respect the user’s <see cref="SvgInstructions.Precision"/> instruction.</param>
    public static string SvgEdgesPath(IEnumerable<Link<Vertex>> edges, Func<Vertex, PointD> getVertexPoint = null, Func<double, string> r = null)
    {
        getVertexPoint ??= (v => v.Point);
        r ??= (d => d.ToString());
        var sb = new StringBuilder();
        foreach (var segment in combineSegments(edges))
        {
            var p = getVertexPoint(segment.Vertices[0]);
            sb.AppendFormat("M{0} {1}", r(p.X), r(p.Y));
            for (var i = 1; i < segment.Vertices.Count; i++)
                sb.Append(segment.Vertices[i].SvgPathFragment(segment.Vertices[i - 1], getVertexPoint, r, isLast: false));
            if (segment.Closed)
            {
                sb.Append(segment.Vertices[0].SvgPathFragment(segment.Vertices[segment.Vertices.Count - 1], getVertexPoint, r, isLast: true));
                sb.Append('z');
            }
        }
        return sb.ToString();
    }

    private static IEnumerable<SvgSegment> combineSegments(IEnumerable<Link<Vertex>> edges)
    {
        var segments = new List<List<Vertex>>();
        var closed = new List<bool>();
        var endPoints = new Dictionary<Vertex, int>();

        foreach (var edge in edges)
        {
            var (v1, v2) = edge;
            if (endPoints.TryGetValue(v1, out var ix1))
            {
                if (endPoints.TryGetValue(v2, out var ix2))
                {
                    if (ix1 == ix2)
                    {
                        // The edge completes a closed loop
                        closed[ix1] = true;
                    }
                    else
                    {
                        // The edge connects two existing segments
                        if (segments[ix1][0].Equals(v1))
                            segments[ix1].Reverse();
                        segments[ix1].AddRange(segments[ix2][0].Equals(v2) ? segments[ix2] : segments[ix2].AsEnumerable().Reverse());
                        segments[ix2] = null;
                        endPoints[segments[ix1].Last()] = ix1;
                    }
                    endPoints.Remove(v1);
                    endPoints.Remove(v2);
                }
                else
                {
                    if (segments[ix1][0].Equals(v1))
                        segments[ix1].Insert(0, v2);
                    else
                        segments[ix1].Add(v2);
                    endPoints.Remove(v1);
                    endPoints[v2] = ix1;
                }
            }
            else if (endPoints.TryGetValue(v2, out var ix2))
            {
                if (segments[ix2][0].Equals(v2))
                    segments[ix2].Insert(0, v1);
                else
                    segments[ix2].Add(v1);
                endPoints.Remove(v2);
                endPoints[v1] = ix2;
            }
            else
            {
                // This is an entirely new segment
                endPoints[v1] = segments.Count;
                endPoints[v2] = segments.Count;
                segments.Add([v1, v2]);
                closed.Add(false);
            }
        }

        for (var i = 0; i < segments.Count; i++)
            if (segments[i] != null)
                yield return new SvgSegment(segments[i], closed[i]);
    }

    /// <summary>
    ///     Converts a string representation of a cell into the original cell.</summary>
    /// <param name="str">
    ///     A string representation in the same format as returned by the original cell’s <c>ToString()</c> method.</param>
    /// <returns>
    ///     This method can parse cells of type <see cref="Cairo"/>, <see cref="Chamf"/>, <see cref="CircularCell"/>, <see
    ///     cref="Floret"/>, <see cref="Hex"/>, <see cref="Kite"/>, <see cref="OctoCell"/>, <see cref="Penrose"/>, <see
    ///     cref="PentaCell"/>, <see cref="Rhomb"/>, <see cref="Rhombihexadel"/>, <see cref="Snubquad"/>, <see
    ///     cref="Square"/>, and <see cref="Tri"/>.</returns>
    public static object Parse(string str)
    {
        Match m;
        foreach (var parser in _parsers)
            if ((m = parser.Regex.Match(str)).Success)
                return parser.Generator(m);
        throw new InvalidOperationException($"The string ‘{str}’ cannot be parsed into a cell type.");
    }

    /// <summary>Returns the parameters as an array.</summary>
    internal static T[] NewArray<T>(params T[] array) => array;

    private static readonly cellParserInfo[] _parsers = NewArray
    (
        new cellParserInfo(@"^C\((-?\d+),(-?\d+)\)/([0-3])$", m => new Cairo(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Cairo.Position) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^C\((\d+);(\d+)/(\d+)→(\d+)/(\d+)\)$", m => new CircularCell(int.Parse(m.Groups[1].Value), new CircleFraction(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)), new CircleFraction(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value)))),
        new cellParserInfo(@"^([Ee])\((\d+),(\d+),(\d+)\)$", m => new PentaCell(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), m.Groups[1].Value[0] == 'e')),
        new cellParserInfo(@"^F\((-?\d+),(-?\d+)\)/([0-5])$", m => new Floret(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Floret.Position) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^H\((-?\d+),(-?\d+)\)$", m => new Hex(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))),
        new cellParserInfo(@"^H\((-?\d+),(-?\d+)\)/([0-4])$", m => new Chamf(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Chamf.Tile) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^K\((-?\d+),(-?\d+)\)/([0-5])$", m => new Kite(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Kite.Position) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^M\((-?\d+),(-?\d+)\)/([0-5])$", m => new Rhombihexadel(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Rhombihexadel.Tile) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^([Oo])\((-?\d+),(-?\d+)\)$", m => new OctoCell(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), m.Groups[1].Value[0] == 'o')),
        new cellParserInfo(@"^P\((-?\d+),(-?\d+),(-?\d+),(-?\d+)\)/([0-3])/([0-9])$", m => new Penrose((Penrose.Kind) int.Parse(m.Groups[5].Value), new Pentavector(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value)), int.Parse(m.Groups[6].Value))),
        new cellParserInfo(@"^R\((-?\d+),(-?\d+)\)/([0-2])$", m => new Rhomb(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Rhomb.Position) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^S\((-?\d+),(-?\d+)\)$", m => new Square(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))),
        new cellParserInfo(@"^S\((-?\d+),(-?\d+)\)/([0-5])$", m => new Snubquad(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), (Snubquad.Tile) int.Parse(m.Groups[3].Value))),
        new cellParserInfo(@"^T\((-?\d+),(-?\d+)\)$", m => new Tri(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value))));

    private struct cellParserInfo(string regex, Func<Match, object> generator)
    {
        public Regex Regex { get; private set; } = new Regex(regex, RegexOptions.Compiled);
        public Func<Match, object> Generator { get; private set; } = generator;
    }
}
