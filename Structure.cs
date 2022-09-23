using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a structure of connected cells, such as a grid.</summary>
    /// <typeparam name="TCell">
    ///     The type of cells in the structure; for example, <see cref="Coord"/> or <see cref="Hex"/>.</typeparam>
    public class Structure<TCell> where TCell : IEquatable<TCell>
    {
        /// <summary>Contains the set of cells the structure consists of.</summary>
        protected readonly HashSet<TCell> _cells;
        /// <summary>Contains the set of links (connections) between cells, defining which pairs of cells are neighbors.</summary>
        protected readonly HashSet<Link<TCell>> _links;

        /// <summary>
        ///     Constructs a structure with the specified cells and links between them.</summary>
        /// <param name="cells">
        ///     The set of cells the structure consists of; for example, <see cref="Coord.Rectangle(int, int)"/> or <see
        ///     cref="Hex.LargeHexagon(int)"/>.</param>
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
                _links = new HashSet<Link<TCell>>(links);
            else
            {
                _links = new HashSet<Link<TCell>>();
                foreach (var cell in cells)
                    foreach (var neighbor in getNeighbors != null ? getNeighbors(cell) : cell is INeighbor<TCell> cn ? cn.Neighbors : throw new InvalidOperationException($"The ‘{nameof(Structure<TCell>)}’ constructor expects that either ‘{nameof(links)}’ or ‘{nameof(getNeighbors)}’ is specified or that ‘{typeof(TCell).FullName}’ implements ‘{typeof(INeighbor<TCell>).FullName}’."))
                        if (_cells.Contains(neighbor))
                            _links.Add(new Link<TCell>(cell, neighbor));
            }
        }

        /// <summary>Returns this structure’s full set of cells.</summary>
        public IEnumerable<TCell> Cells => _cells;
        /// <summary>Returns the full set of links between the cells in this structure.</summary>
        public IEnumerable<Link<TCell>> Links => _links;

        /// <summary>Determines whether the specified link is traversible.</summary>
        public bool IsLink(Link<TCell> link) => _links.Contains(link);
        /// <summary>Determines whether a direct link between the specified cells exists in this structure and is traversible.</summary>
        public bool IsLink(TCell cell1, TCell cell2) => _links.Contains(new Link<TCell>(cell1, cell2));

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rnd">
        ///     A random number generator.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public Structure<TCell> GenerateMaze(Random rnd = null) => GenerateMaze((rnd ?? new Random()).Next);

        /// <summary>
        ///     Generates a maze on this structure.</summary>
        /// <param name="rndNext">
        ///     A delegate that can provide random numbers.</param>
        /// <exception cref="InvalidOperationException">
        ///     The current structure is disjointed (consists of more than one piece).</exception>
        public Structure<TCell> GenerateMaze(Func<int, int, int> rndNext)
        {
            if (rndNext == null)
                throw new ArgumentNullException(nameof(rndNext));

            var lnks = new Dictionary<TCell, List<TCell>>();
            foreach (var lnk in _links)
                foreach (var cell in lnk.Cells)
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
                var ix = rndNext(0, todo.Count);
                var cell = todo[ix];

                var availableLinks = lnks[cell].Where(otherCell => cells.Contains(otherCell)).ToArray();
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

            return generateMazeResult(_cells, traversible);
        }

        /// <summary>Provides a way to override the constructor call for a new structure when a maze is generated.</summary>
        protected virtual Structure<TCell> generateMazeResult(HashSet<TCell> cells, HashSet<Link<TCell>> traversible) => new Structure<TCell>(cells, traversible);

        /// <summary>Returns an SVG file that visualizes this structure.</summary>
        public string Svg()
        {
            var allEdges = new Dictionary<Link<Vertex<TCell>>, List<TCell>>();
            foreach (var cell in _cells)
                foreach (var edge in ((cell as IHasVertices<TCell>) ?? svgThrow()).Vertices.SelectConsecutivePairs(closed: true, (v1, v2) => new Link<Vertex<TCell>>(v1, v2)))
                    allEdges.AddSafe(edge, cell);

            var outlineEdges = new List<Link<Vertex<TCell>>>();
            var wallEdges = new List<Link<Vertex<TCell>>>();
            var passageEdges = new List<Link<Vertex<TCell>>>();

            foreach (var kvp in allEdges)
                (svgEdgeType(kvp.Key, kvp.Value) switch { EdgeType.Outline => outlineEdges, EdgeType.Passage => passageEdges, _ => wallEdges }).Add(kvp.Key);

            var outline = svgPath(outlineEdges);
            var walls = svgPath(wallEdges);
            var passages = svgPath(passageEdges);

            var minX = allEdges.SelectMany(kvp => kvp.Key.Cells).Min(v => v.X);
            var minY = allEdges.SelectMany(kvp => kvp.Key.Cells).Min(v => v.Y);
            var maxX = allEdges.SelectMany(kvp => kvp.Key.Cells).Max(v => v.X);
            var maxY = allEdges.SelectMany(kvp => kvp.Key.Cells).Max(v => v.Y);
            return $"<svg xmlns='http://www.w3.org/2000/svg' viewBox='{minX - .1} {minY - .1} {maxX - minX + .2} {maxY - minY + .2}' fill='none'><path d='{passages}' stroke-width='.02' stroke='#ccc' stroke-dasharray='.1' /><path d='{walls}' stroke-width='.05' stroke='black' /><path d='{outline}' stroke-width='.1' stroke='black' /></svg>";
        }

        /// <summary>
        ///     Determines what type of edge to draw in SVG for a particular edge between cells.</summary>
        /// <param name="edge">
        ///     The edge.</param>
        /// <param name="cells">
        ///     The set of cells adjacent to this edge, in no guaranteed order.</param>
        protected virtual EdgeType svgEdgeType(Link<Vertex<TCell>> edge, List<TCell> cells) => cells.Count == 1 ? EdgeType.Outline : cells.Count == 2 && _links.Contains(new Link<TCell>(cells[0], cells[1])) ? EdgeType.Passage : EdgeType.Wall;

        private static IHasVertices<TCell> svgThrow() => throw new InvalidOperationException($"To generate SVG, the type {typeof(TCell).FullName} must implement {typeof(IHasVertices<TCell>).FullName}.");

        private string svgPath(IEnumerable<Link<Vertex<TCell>>> edges)
        {
            var segments = new List<List<Vertex<TCell>>>();
            var closed = new List<bool>();
            var endPoints = new Dictionary<Vertex<TCell>, int>();

            foreach (var edge in edges)
            {
                var v1 = edge.Cells.First();
                var v2 = edge.Cells.Last();
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
                    segments.Add(new List<Vertex<TCell>> { v1, v2 });
                    closed.Add(false);
                }
            }

            return segments.Select((seg, ix) => seg == null ? "" : $"M{seg.Select(v => $"{v.X} {v.Y}").JoinString(" ")}{(closed[ix] ? "z" : "")}").JoinString();
        }
    }
}
