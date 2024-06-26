using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a grid that is a combination (merger) of multiple cells behaving as one.</summary>
    /// <typeparam name="TCell">
    ///     Type of the underlying cells.</typeparam>
    /// <remarks>
    ///     See <see cref="Structure{TCell}.CombineCells(TCell[])"/> for a code example.</remarks>
    public struct CombinedCell<TCell> : IEquatable<CombinedCell<TCell>>, IHasSvgGeometry, IEnumerable<TCell>
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
            _underlyingCells = new HashSet<TCell>();
            fillHashset(_underlyingCells, cells);
            if (_underlyingCells.Count == 0)
                throw new ArgumentException($"Cannot create a {typeof(CombinedCell<TCell>).FullName} containing zero cells.");
        }

        internal CombinedCell(IEnumerable<TCell> cells, bool allowEmpty) : this()
        {
            if (cells == null)
                throw new ArgumentNullException(nameof(cells));
            _underlyingCells = new HashSet<TCell>();
            fillHashset(_underlyingCells, cells);
            if (!allowEmpty && _underlyingCells.Count == 0)
                throw new ArgumentException($"Cannot create a {typeof(CombinedCell<TCell>).FullName} containing zero cells.");
        }

        /// <summary>Constructs a <see cref="CombinedCell{TCell}"/> structure consisting of a single cell.</summary>
        public CombinedCell(TCell singleCell) : this()
        {
            _underlyingCells = new HashSet<TCell> { singleCell };
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
        public IEnumerable<Link<Vertex>> Edges
        {
            get
            {
                if (_underlyingCells.Count == 1)
                {
                    if (!(_underlyingCells.First() is IHasSvgGeometry cellG))
                        throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Edges)} when a contained cell does not implement {typeof(IHasSvgGeometry).FullName}.");
                    return cellG.Edges;
                }

                var edges = new HashSet<Link<Vertex>>();
                foreach (var cell in _underlyingCells)
                {
                    if (!(cell is IHasSvgGeometry cellG))
                        throw new InvalidOperationException($"Attempt to call {nameof(CombinedCell<TCell>)}.{nameof(Edges)} when a contained cell does not implement {typeof(IHasSvgGeometry).FullName}.");
                    foreach (var edge in cellG.Edges)
                        if (!edges.Remove(edge))
                            edges.Add(edge);
                }
                return edges;
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
                    if (!(c is IHasSvgGeometry cs))
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
