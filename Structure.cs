using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a structure of connected cells, such as a grid.</summary>
    /// <typeparam name="TCell">
    ///     The type of cells in the structure; for example, <see cref="Square"/> or <see cref="Hex"/>.</typeparam>
    public class Structure<TCell>
    {
        /// <summary>Contains the set of cells the structure consists of.</summary>
        protected readonly HashSet<TCell> _cells;
        /// <summary>Contains the set of links (connections) between cells, defining which pairs of cells are neighbors.</summary>
        protected readonly HashSet<Link<TCell>> _links;

        /// <summary>
        ///     Constructs a structure with the specified cells and links between them.</summary>
        /// <param name="cells">
        ///     The set of cells the structure consists of; for example, <see cref="Square.Rectangle(int, int, int, int)"/> or
        ///     <see cref="Hex.LargeHexagon(int, Hex)"/>.</param>
        /// <param name="links">
        ///     An explicit set of links (connections) between cells, defining which pairs of cells are neighbors. See
        ///     remarks.</param>
        /// <param name="getNeighbors">
        ///     A delegate that defines the neighbors of each cells. See remarks.</param>
        /// <remarks>
        ///     <para>
        ///         One of the following three must be true:</para>
        ///     <list type="bullet">
        ///         <item><description>
        ///             <paramref name="links"/> is specified and <paramref name="getNeighbors"/> is <c>null</c>. In this
        ///             case, the explicit set of links is used.</description></item>
        ///         <item><description>
        ///             <paramref name="getNeighbors"/> is specified and <paramref name="links"/> is <c>null</c>. In this
        ///             case, the delegate is called on every cell to discover all links. The delegate is allowed to return
        ///             additional cells that are not in <paramref name="cells"/>; those will be discarded.</description></item>
        ///         <item><description>
        ///             <typeparamref name="TCell"/> implements <see cref="INeighbor{TCell}"/>, in which case the behavior is
        ///             the same as with <paramref name="getNeighbors"/> but <see cref="INeighbor{TCell}.Neighbors"/> is used
        ///             to retrieve the neighbors.</description></item></list></remarks>
        public Structure(IEnumerable<TCell> cells, IEnumerable<Link<TCell>> links = null, Func<TCell, IEnumerable<TCell>> getNeighbors = null)
        {
            _cells = new HashSet<TCell>(cells);
            if (links != null)
                _links = new HashSet<Link<TCell>>(links.Where(l => l.All(_cells.Contains)));
            else
            {
                _links = new HashSet<Link<TCell>>();
                foreach (var cell in cells)
                    foreach (var neighbor in getNeighbors != null ? getNeighbors(cell) : cell is INeighbor<TCell> cn ? cn.Neighbors : throw new InvalidOperationException($"The ‘{nameof(Structure<TCell>)}’ constructor expects that either ‘{nameof(links)}’ or ‘{nameof(getNeighbors)}’ is specified or that every cell type (e.g. {cell.GetType().FullName}) implements ‘{typeof(INeighbor<TCell>).FullName}’."))
                        if (_cells.Contains(neighbor))
                            _links.Add(new Link<TCell>(cell, neighbor));
            }
        }

        /// <summary>Returns this structure’s full set of cells.</summary>
        public IEnumerable<TCell> Cells { get { foreach (var cell in _cells) yield return cell; } }
        /// <summary>Returns the full set of links between the cells in this structure.</summary>
        public IEnumerable<Link<TCell>> Links { get { foreach (var link in _links) yield return link; } }

        /// <summary>Determines whether this structure contains the specified <paramref name="cell"/>.</summary>
        public bool Contains(TCell cell) => _cells.Contains(cell);

        /// <summary>Determines whether the specified link is traversible.</summary>
        public bool IsLink(Link<TCell> link) => _links.Contains(link);
        /// <summary>Determines whether a direct link between the specified cells exists in this structure and is traversible.</summary>
        public bool IsLink(TCell cell1, TCell cell2) => _links.Contains(new Link<TCell>(cell1, cell2));

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <param name="bias">
        ///     Provides a means to influence the algorithm.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public Structure<TCell> GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => GenerateMaze((rnd ?? new Random()).Next, bias);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <param name="bias">
        ///     Provides a means to influence the algorithm.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public Structure<TCell> GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default)
        {
            if (rndNext == null)
                throw new ArgumentNullException(nameof(rndNext));

            var lnks = new Dictionary<TCell, List<TCell>>();
            foreach (var lnk in _links)
                foreach (var cell in lnk)
                    lnks.AddSafe(cell, lnk.Other(cell));

            var cells = new HashSet<TCell>(_cells);

            // Find a random starting cell
            var startCell = cells.Skip(rndNext(0, cells.Count)).First();

            // Maze algorithm starts here
            var todo = new List<TCell> { startCell };
            cells.Remove(startCell);

            var traversible = new HashSet<Link<TCell>>();

            while (cells.Count > 0)
            {
                if (todo.Count == 0)
                    throw new InvalidOperationException("The specified set of links for the maze is disjointed (consists of more than one piece).");
                var ix = bias switch
                {
                    MazeBias.Default => rndNext(0, todo.Count),
                    MazeBias.Straight => 0,
                    MazeBias.Winding => todo.Count - 1,
                    _ => throw new ArgumentException($"Invalid ‘{nameof(bias)}’ value: ‘{bias}’.")
                };
                var cell = todo[ix];

                var availableLinks = (lnks.TryGetValue(cell, out var otherCells) ? otherCells : throw new InvalidOperationException($"The cell {cell} has no links."))
                    .Where(otherCell => cells.Contains(otherCell)).ToArray();
                if (availableLinks.Length == 0)
                    todo.RemoveAt(ix);
                else
                {
                    var otherCell = availableLinks[availableLinks.Length == 1 ? 0 : rndNext(0, availableLinks.Length)];
                    traversible.Add(new Link<TCell>(cell, otherCell));
                    cells.Remove(otherCell);
                    todo.Add(otherCell);
                }
            }

            return makeModifiedStructure(_cells, traversible);
        }

        /// <summary>Provides a way to override the constructor call for a new structure when a maze is generated.</summary>
        protected virtual Structure<TCell> makeModifiedStructure(IEnumerable<TCell> cells, IEnumerable<Link<TCell>> traversible) => new Structure<TCell>(cells, traversible);

        /// <summary>Returns an SVG file that visualizes this structure.</summary>
        public string Svg(SvgInstructions inf = null)
        {
            var allEdges = new Dictionary<Link<Vertex>, List<TCell>>();
            foreach (var cell in _cells)
                foreach (var edge in getEdges(cell, inf))
                    allEdges.AddSafe(edge, cell);

            var highlights = new StringBuilder();
            var getVertexPoint = inf?.GetVertexPoint ?? (v => v.Point);
            StringBuilder addFill(TCell cell, string fill) => fill == null ? null : highlights.Append($"<path d='{GridUtils.SvgEdgesPath(getEdges(cell, inf), getVertexPoint)}' fill='{fill}' stroke='none' />");
            StringBuilder addFillAndOpacity(TCell cell, string fill, string opacity) => opacity == null ? addFill(cell, fill) : highlights.Append($"<path d='{GridUtils.SvgEdgesPath(getEdges(cell, inf), getVertexPoint)}' fill='{fill}' fill-opacity='{opacity}' stroke='none' />");
            StringBuilder addFillColor(TCell cell, SvgColor color) => addFillAndOpacity(cell, color.SvgFillColor, color.SvgFillOpacity);
            StringBuilder addFillObject(TCell cell, object fill) => fill == null ? null : fill is SvgColor color ? addFillColor(cell, color) : addFill(cell, fill.ToString());

            if (inf?.HighlightCells is IEnumerable enumerable)
                foreach (var obj in enumerable)
                {
                    if (obj is TCell c && _cells.Contains(c))
                        addFillColor(c, inf.HighlightColor);
                    else if (obj is DictionaryEntry de && de.Key is TCell c2 && _cells.Contains(c2))
                    {
                        if (de.Value is SvgColor color)
                            addFillColor(c2, color);
                        else if (de.Value != null)
                            addFill(c2, de.Value.ToString());
                    }
                }
            else if (inf?.HighlightCells is Func<object, object> fnc1)
                foreach (var cell in _cells)
                    addFillObject(cell, fnc1(cell));
            else if (inf?.HighlightCells is Func<object, string> fnc2)
                foreach (var cell in _cells)
                    addFill(cell, fnc2(cell));
            else if (inf?.HighlightCells is Func<object, SvgColor> fnc3)
                foreach (var cell in _cells)
                    addFillColor(cell, fnc3(cell));

            var outlineEdges = new List<Link<Vertex>>();
            var outlineCells = new List<TCell>();
            var wallEdges = new List<Link<Vertex>>();
            var wallLinks = new List<Link<TCell>>();
            var passageEdges = new List<Link<Vertex>>();
            var passageLinks = new List<Link<TCell>>();

            foreach (var kvp in allEdges)
            {
                EdgeType edgeType;
                TCell cell1, cell2;
                var objInf = inf?.GetEdgeType?.Invoke(kvp.Key, kvp.Value);
                if (objInf == null)
                    (edgeType, cell1, cell2) = svgEdgeType(kvp.Key, kvp.Value);
                else
                {
                    edgeType = objInf.Value.EdgeType;
                    cell1 = (TCell) objInf.Value.Cell1;
                    cell2 = (TCell) objInf.Value.Cell2;
                }

                switch (edgeType)
                {
                    case EdgeType.Outline:
                        outlineEdges.Add(kvp.Key);
                        outlineCells.Add(cell1);
                        break;
                    case EdgeType.Passage:
                        passageEdges.Add(kvp.Key);
                        passageLinks.Add(new Link<TCell>(cell1, cell2));
                        break;
                    case EdgeType.Wall:
                        wallEdges.Add(kvp.Key);
                        wallLinks.Add(new Link<TCell>(cell1, cell2));
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid {nameof(EdgeType)} value encountered: {edgeType}");
                }
            }

            var bridges = new StringBuilder();
            foreach (var bridge in _links.Except(allEdges.Values.Where(v => v.Count == 2).Select(v => new Link<TCell>(v[0], v[1]))))
                if (drawBridge(bridge))
                {
                    var (cell1, cell2) = bridge;
                    var center1 = getCenter(cell1, inf);
                    var center2 = getCenter(cell2, inf);
                    bridges.Append(inf?.BridgeSvg?.Invoke(cell1, center1, cell2, center2) ?? SvgInstructions.DrawBridge(center1, center2));
                }

            // Slightly dirty trick using inline assignment to ensure that ‘path’ is evaluated only once and only if needed:
            //  • If ‘inf?.OutlineSeparate ?? false’ is false, it should not be evaluated at all, because we don’t need it and GridUtils.SvgEdgesPath() is costly
            //  • If ‘inf?.OutlinePath’ is null, the invocation is skipped and ‘path’ remains null, so GridUtils.SvgEdgesPath() will get evaluated inside the default string
            //  • If ‘inf?.OutlinePath’ is not null, ‘path’ will get assigned in the invocation parameter list; but if that invocation RETURNS null, we need to re-use the value in the default string
            string path = null;
            var outline = outlineEdges.Count == 0 ? "" : (inf?.OutlineSeparate ?? false)
                ? outlineEdges.Select((edge, ix) => inf.OutlinePaths(svgEdgePath(edge, getVertexPoint), outlineCells[ix])).JoinString()
                : inf?.OutlinePath?.Invoke(path = GridUtils.SvgEdgesPath(outlineEdges, getVertexPoint)) ?? $"<path d='{path ?? GridUtils.SvgEdgesPath(outlineEdges, getVertexPoint)}' fill='none' stroke-width='.1' stroke='black' />";

            path = null;
            var walls = wallEdges.Count == 0 ? "" : (inf?.WallsSeparate ?? false)
                ? wallEdges.Select((edge, ix) => inf.WallsPaths(svgEdgePath(edge, getVertexPoint), wallLinks[ix].Apart(out var cc), cc)).JoinString()
                : inf?.WallsPath?.Invoke(path = GridUtils.SvgEdgesPath(wallEdges, getVertexPoint)) ?? $"<path d='{path ?? GridUtils.SvgEdgesPath(wallEdges, getVertexPoint)}' fill='none' stroke-width='.05' stroke='black' />";

            path = null;
            var passages = passageEdges.Count == 0 ? "" : (inf?.PassagesSeparate ?? false)
                ? passageEdges.Select((edge, ix) => inf.PassagesPaths(svgEdgePath(edge, getVertexPoint), passageLinks[ix].Apart(out var cc), cc)).JoinString()
                : inf?.PassagesPath?.Invoke(path = GridUtils.SvgEdgesPath(passageEdges, getVertexPoint)) ?? $"<path d='{path ?? GridUtils.SvgEdgesPath(passageEdges, getVertexPoint)}' fill='none' stroke-width='.02' stroke='#ccc' stroke-dasharray='.1' />";

            var allPoints = allEdges.SelectMany(kvp => kvp.Key.Select(getVertexPoint)).ToList();
            var minX = allPoints.Min(v => v.X);
            var minY = allPoints.Min(v => v.Y);
            var maxX = allPoints.Max(v => v.X);
            var maxY = allPoints.Max(v => v.Y);
            var startTag = inf != null && inf.SvgAttributes == null ? "" :
                $"<svg {string.Format(inf?.SvgAttributes ?? "xmlns='http://www.w3.org/2000/svg' viewBox='{0} {1} {2} {3}' font-size='.2' text-anchor='middle'", minX - .1, minY - .1, maxX - minX + .2, maxY - minY + .2)}>";
            var endTag = inf != null && inf.SvgAttributes == null ? "" : "</svg>";

            return startTag +
                inf?.ExtraSvg1 +
                highlights +
                inf?.ExtraSvg2 +
                passages +
                walls +
                outline +
                bridges +
                inf?.ExtraSvg3 +
                (inf?.PerCell == null ? "" : _cells.Select(cell => processCellSvg(cell, inf)).JoinString()) +
                inf?.ExtraSvg4 +
                endTag;
        }

        private static string processCellSvg(TCell cell, SvgInstructions inf)
        {
            var svg = inf?.PerCell(cell);
            if (svg == null)
                return "";
            var center = getCenter(cell, inf);
            return $"<g transform='translate({center.X} {center.Y})'>{svg}</g>";
        }

        private static IHasSvgGeometry geom(TCell cell) => (cell as IHasSvgGeometry) ??
            throw new InvalidOperationException($"To generate SVG, the type ‘{cell.GetType().FullName}’ must implement ‘{typeof(IHasSvgGeometry).FullName}’ or the provided ‘{typeof(SvgInstructions)}’ instance must provide ‘{nameof(SvgInstructions.GetEdges)}’ and ‘{nameof(SvgInstructions.GetCenter)}’ delegates.");

        private static IEnumerable<Link<Vertex>> getEdges(TCell cell, SvgInstructions inf) => inf?.GetEdges?.Invoke(cell) ?? geom(cell).Edges;
        private static PointD getCenter(TCell cell, SvgInstructions inf) => inf?.GetCenter?.Invoke(cell) ?? geom(cell).Center;

        /// <summary>
        ///     When overridden in a derived class, determines what type of edge to draw in SVG for a particular edge between
        ///     cells or at the edge of the grid.</summary>
        /// <param name="edge">
        ///     The edge.</param>
        /// <param name="cells">
        ///     The set of cells adjacent to this edge, in no guaranteed order.</param>
        /// <returns>
        ///     An <see cref="EdgeInfo{TCell}"/> structure that determines the desired edge type via <see
        ///     cref="EdgeInfo{TCell}.EdgeType"/> and the adjacent cell(s) via <see cref="EdgeInfo{TCell}.Cell1"/> and
        ///     (optionally) <see cref="EdgeInfo{TCell}.Cell2"/>.</returns>
        protected virtual EdgeInfo<TCell> svgEdgeType(Link<Vertex> edge, List<TCell> cells) =>
            cells == null || cells.Count == 0 ? throw new InvalidOperationException($"‘{nameof(cells)}’ cannot be null or empty.") : cells.Count == 1
                ? new EdgeInfo<TCell> { EdgeType = EdgeType.Outline, Cell1 = cells[0] }
                : new EdgeInfo<TCell> { EdgeType = _links.Contains(new Link<TCell>(cells[0], cells[1])) ? EdgeType.Passage : EdgeType.Wall, Cell1 = cells[0], Cell2 = cells[1] };

        /// <summary>
        ///     When overridden in a derived class, determines whether a bridge should be drawn for the specified <paramref
        ///     name="link"/>. This method is only called for links between cells that have no edges in common; otherwise,
        ///     <see cref="svgEdgeType(Link{Vertex}, List{TCell})"/> is called instead.</summary>
        /// <param name="link">
        ///     Specifies the link for which to decide whether to draw a bridge.</param>
        protected virtual bool drawBridge(Link<TCell> link) => true;

        private string svgEdgePath(Link<Vertex> segment, Func<Vertex, PointD> getVertexPoint)
        {
            var one = segment.Apart(out var two);
            var p = getVertexPoint(one);
            return string.Format("M{0} {1}{2}", p.X, p.Y, two.SvgPathFragment(one, getVertexPoint, isLast: false));
        }

        /// <summary>Adds the specified link to this structure.</summary>
        public void AddLink(Link<TCell> link) => _links.Add(link);
        /// <summary>Adds a link between the specified cells to this structure.</summary>
        public void AddLink(TCell cell1, TCell cell2) => _links.Add(new Link<TCell>(cell1, cell2));
        /// <summary>Adds the specified links to this structure.</summary>
        public void AddLinks(params Link<TCell>[] links) { foreach (var link in links) _links.Add(link); }
        /// <summary>Adds the specified links to this structure.</summary>
        public void AddLinks(IEnumerable<Link<TCell>> links) { foreach (var link in links) _links.Add(link); }

        /// <summary>Removes the specified link from this structure.</summary>
        public void RemoveLink(Link<TCell> link) => _links.Remove(link);
        /// <summary>Removes the specified links from this structure.</summary>
        public void RemoveLinks(params Link<TCell>[] links) { foreach (var link in links) _links.Remove(link); }
        /// <summary>Removes the specified links from this structure.</summary>
        public void RemoveLinks(IEnumerable<Link<TCell>> links) { foreach (var link in links) _links.Remove(link); }

        /// <summary>Removes the specified cell from this structure.</summary>
        public void RemoveCell(TCell cell) { _cells.Remove(cell); _links.RemoveWhere(l => l.Contains(cell)); }
        /// <summary>Removes the specified cells from this structure.</summary>
        public void RemoveCells(params TCell[] cells) { foreach (var cell in cells) _cells.Remove(cell); _links.RemoveWhere(l => cells.Any(c => l.Contains(c))); }
        /// <summary>Removes the specified cells from this structure.</summary>
        public void RemoveCells(IEnumerable<TCell> cells) { foreach (var cell in cells) _cells.Remove(cell); _links.RemoveWhere(l => cells.Any(c => l.Contains(c))); }
        /// <summary>Removes all cells from this structure that match the specified <paramref name="predicate"/>.</summary>
        public void RemoveCells(Predicate<TCell> predicate) { _cells.RemoveWhere(predicate); _links.RemoveWhere(l => l.Any(c => !_cells.Contains(c))); }

        /// <summary>Adds the specified cells to this structure.</summary>
        public void AddCell(TCell cell) { _cells.Add(cell); }
        /// <summary>Adds the specified cells to this structure.</summary>
        public void AddCells(params TCell[] cells) { foreach (var cell in cells) _cells.Add(cell); }
        /// <summary>Adds the specified cells to this structure.</summary>
        public void AddCells(IEnumerable<TCell> cells) { foreach (var cell in cells) _cells.Add(cell); }

        /// <summary>
        ///     Finds the shortest path from the specified <paramref name="origin"/> cell to every other cell in the
        ///     structure.</summary>
        /// <param name="origin">
        ///     The start cell at which all paths begin.</param>
        /// <returns>
        ///     A dictionary mapping from each cell to a structure revealing the distance from <paramref name="origin"/> as
        ///     well as the previous cell on the path.</returns>
        public Dictionary<TCell, CellWithDistance<TCell>> FindPaths(TCell origin) => findPaths(origin, default, false);

        /// <summary>
        ///     Finds the shortest path from the specified <paramref name="from"/> cell to the specified <paramref name="to"/>
        ///     cell.</summary>
        /// <param name="from">
        ///     The starting cell.</param>
        /// <param name="to">
        ///     The destination cell.</param>
        /// <returns>
        ///     A collection containing every cell along the path, including <paramref name="from"/> and <paramref
        ///     name="to"/>.</returns>
        public IEnumerable<TCell> FindPath(TCell from, TCell to) => findPaths(from, to, true).GetPathTo(to);

        private Dictionary<TCell, CellWithDistance<TCell>> findPaths(TCell origin, TCell stopAt, bool hasStopAt)
        {
            var links = new Dictionary<TCell, List<TCell>>();
            foreach (var link in _links)
                foreach (var c in link)
                    (links.TryGetValue(c, out var list) ? list : (links[c] = new List<TCell>())).Add(link.Other(c));

            var result = new Dictionary<TCell, CellWithDistance<TCell>>();
            var q = new Queue<CellWithDistance<TCell>>();
            q.Enqueue(new CellWithDistance<TCell>(origin, default, 0));
            while (q.Count > 0)
            {
                var item = q.Dequeue();
                if (result.ContainsKey(item.Cell))
                    continue;
                result[item.Cell] = item;
                if (hasStopAt && item.Cell.Equals(stopAt))
                    break;
                if (!links.TryGetValue(item.Cell, out var list))
                    continue;
                foreach (var other in list)
                    q.Enqueue(new CellWithDistance<TCell>(other, item.Cell, item.Distance + 1));
            }
            return result;
        }

        /// <summary>
        ///     Returns a new structure in which the specified set of cells is combined (merged) into a single cell.</summary>
        /// <param name="cells">
        ///     Set of cells to combine into one.</param>
        /// <remarks>
        ///     <para>
        ///         Note that the type of the structure changes from <c>Structure&lt;TCell&gt;</c> to
        ///         <c>Structure&lt;CombinedCell&lt;TCell&gt;&gt;</c>. If you wish to combine multiple groups of cells, call
        ///         this overload with zero parameters first just to change the type, then subsequently use <see
        ///         cref="GridUtils.CombineCells{TCell}(Structure{CombinedCell{TCell}}, TCell[])"/> to combine each group. The
        ///         following example code illustrates this principle by creating a grid in which some horizontal or vertical
        ///         pairs of cells are combined:</para>
        ///     <code>
        ///         var grid = new Grid(12, 12).CombineCells();
        ///         foreach (var cell in grid.Cells.Select(cc =&gt; cc.First()))
        ///             if ((cell.X % 4 == 0 &amp;&amp; cell.Y % 4 &lt; 2) || (cell.X % 4 == 2 &amp;&amp; cell.Y % 4 &gt; 1))
        ///                 grid = grid.CombineCells(new Coord[] { cell, cell.Move(GridDirection.Right) });
        ///             else if ((cell.X % 4 &gt; 1 &amp;&amp; cell.Y % 4 == 0) || (cell.X % 4 &lt; 2 &amp;&amp; cell.Y % 4 == 2))
        ///                 grid = grid.CombineCells(new Coord[] { cell, cell.Move(GridDirection.Down) });</code></remarks>
        public Structure<CombinedCell<TCell>> CombineCells(params TCell[] cells) => CombineCells((IEnumerable<TCell>) cells);

        /// <summary>
        ///     Returns a new structure in which the specified set of cells is combined (merged) into a single cell.</summary>
        /// <param name="cells">
        ///     Set of cells to combine into one.</param>
        /// <remarks>
        ///     See remarks at the other overload, <see cref="CombineCells(TCell[])"/>.</remarks>
        public Structure<CombinedCell<TCell>> CombineCells(IEnumerable<TCell> cells)
        {
            var combo = new CombinedCell<TCell>(cells, allowEmpty: true);
            if (combo.Count == 0)
                return new Structure<CombinedCell<TCell>>(
                    _cells.Select(c => new CombinedCell<TCell>(c)),
                    _links.Select(link => new Link<CombinedCell<TCell>>(new CombinedCell<TCell>(link.Apart(out var other)), new CombinedCell<TCell>(other))));
            return new Structure<CombinedCell<TCell>>(
                _cells.Where(c => !combo.Contains(c)).Select(c => new CombinedCell<TCell>(c)).Concat(new CombinedCell<TCell>[] { combo }),
                _links.Select(link =>
                {
                    var (c1, c2) = link;
                    if (combo.Contains(c1))
                        return combo.Contains(c2) ? null : new Link<CombinedCell<TCell>>(combo, new CombinedCell<TCell>(c2)).Nullable();
                    if (combo.Contains(c2))
                        return new Link<CombinedCell<TCell>>(new CombinedCell<TCell>(c1), combo).Nullable();
                    return new Link<CombinedCell<TCell>>(new CombinedCell<TCell>(c1), new CombinedCell<TCell>(c2)).Nullable();
                }).WhereNotNull());
        }
    }
}
