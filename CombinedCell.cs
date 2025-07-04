using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a grid that is a combination (merger) of multiple cells behaving as one.</summary>
    /// <typeparam name="TCell">
    ///     Type of the underlying cells.</typeparam>
    /// <remarks>
    ///     See <see cref="Structure{TCell}.CombineCells(TCell[])"/> for a code example.</remarks>
    public readonly struct CombinedCell<TCell> : IEquatable<CombinedCell<TCell>>, IHasSvgGeometry, IEnumerable<TCell>
    {
        private readonly HashSet<TCell> _underlyingCells;

        /// <inheritdoc/>
        public IEnumerator<TCell> GetEnumerator() => _underlyingCells.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>Returns the number of underlying cells.</summary>
        public int Count => _underlyingCells.Count;

        /// <summary>
        ///     Determines whether the specified <paramref name="cell"/> is one of the underlying cell of this combined cell.</summary>
        public bool Contains(TCell cell) => _underlyingCells.Contains(cell);

        /// <summary>Constructs a combined cell from a specified collection of cells.</summary>
        public CombinedCell(params TCell[] cells) : this(cells.AsEnumerable()) { }

        /// <summary>Constructs a combined cell from a specified collection of cells.</summary>
        public CombinedCell(IEnumerable<TCell> cells) : this()
        {
            if (cells == null)
                throw new ArgumentNullException(nameof(cells));
            _underlyingCells = [];
            fillHashset(_underlyingCells, cells);
            if (_underlyingCells.Count == 0)
                throw new ArgumentException($"Cannot create a {typeof(CombinedCell<TCell>).FullName} containing zero cells.");
        }

        internal CombinedCell(IEnumerable<TCell> cells, bool allowEmpty) : this()
        {
            if (cells == null)
                throw new ArgumentNullException(nameof(cells));
            _underlyingCells = [];
            fillHashset(_underlyingCells, cells);
            if (!allowEmpty && _underlyingCells.Count == 0)
                throw new ArgumentException($"Cannot create a {typeof(CombinedCell<TCell>).FullName} containing zero cells.");
        }

        /// <summary>Constructs a <see cref="CombinedCell{TCell}"/> structure consisting of a single cell.</summary>
        public CombinedCell(TCell singleCell) : this()
        {
            _underlyingCells = [singleCell];
        }

        private void fillHashset(HashSet<TCell> underlyingCells, IEnumerable<TCell> cells)
        {
            foreach (var cell in cells)
                if (cell is CombinedCell<TCell> cc)
                    fillHashset(underlyingCells, cc);
                else
                    underlyingCells.Add(cell);
        }

        /// <inheritdoc/>
        public bool Equals(CombinedCell<TCell> other) => other._underlyingCells.All(_underlyingCells.Contains) && _underlyingCells.All(other._underlyingCells.Contains);
        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj is CombinedCell<TCell> cc && Equals(cc)) || (_underlyingCells.Count == 1 && obj is TCell c && _underlyingCells.First().Equals(c));
        /// <inheritdoc/>
        public override int GetHashCode() => _underlyingCells.Count == 1 ? _underlyingCells.First().GetHashCode() : unchecked(_underlyingCells.Aggregate(0, (p, n) => p ^ n.GetHashCode()) + 235661 * _underlyingCells.Count);

        /// <inheritdoc/>
        public IEnumerable<Edge> Edges
        {
            get
            {
                if (_underlyingCells.Count == 1)
                {
                    return _underlyingCells.First() is not IHasSvgGeometry cellG
                        ? throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Edges)} when a contained cell does not implement {typeof(IHasSvgGeometry).FullName}.")
                        : cellG.Edges;
                }

                using var e = _underlyingCells.GetEnumerator();
                if (!e.MoveNext())
                    return [];
                if (e.Current is not IHasSvgGeometry firstGeom)
                    throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Edges)} when a contained cell ({e.Current}) does not implement {typeof(IHasSvgGeometry).FullName}.");
                var loops = new List<List<Edge>> { firstGeom.Edges.ToList() };
                while (e.MoveNext())
                {
                    if (e.Current is not IHasSvgGeometry geom)
                        throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Edges)} when a contained cell ({e.Current}) does not implement {typeof(IHasSvgGeometry).FullName}.");
                    loops.Add(geom.Edges.ToList());
                }

                for (var i = 0; i < loops.Count; i++)
                {
                    continue_i:
                    for (var j = i + 1; j < loops.Count; j++)
                    {
                        var oldConnIx = loops[i].IndexOf(edge => loops[j].Contains(edge.Reverse));
                        if (oldConnIx == -1)
                            continue;

                        var newConnIx = loops[j].IndexOf(loops[i][oldConnIx].Reverse);
                        loops[i].RemoveAt(oldConnIx);
                        loops[i].InsertRange(oldConnIx, loops[j].Take(newConnIx));
                        loops[i].InsertRange(oldConnIx, loops[j].Skip(newConnIx + 1));

                        for (var k = 0; k < loops[i].Count;)
                        {
                            if (loops[i][k].Reverse == loops[i][(k + 1) % loops[i].Count])
                            {
                                loops[i].RemoveAt(k);
                                loops[i].RemoveAt(k % loops[i].Count);
                            }
                            else
                                k++;
                        }

                        loops.RemoveAt(j);
                        goto continue_i;
                    }
                }

                return loops.SelectMany(x => x);
            }
        }

        /// <inheritdoc/>
        public PointD Center
        {
            get
            {
                double x = 0, y = 0;
                foreach (var c in _underlyingCells)
                {
                    if (c is not IHasSvgGeometry cs)
                        throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Center)} when a contained cell does not implement {typeof(IHasSvgGeometry).FullName}.");
                    x += cs.Center.X;
                    y += cs.Center.Y;
                }
                return new PointD(x / _underlyingCells.Count, y / _underlyingCells.Count);
            }
        }

        /// <inheritdoc/>
        public override readonly string ToString() => _underlyingCells.JoinString("+");
    }
}
