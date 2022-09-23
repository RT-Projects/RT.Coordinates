using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents a link between two cells in a grid or structure.</summary>
    /// <typeparam name="TCell">
    ///     The type of the cells linked.</typeparam>
    public struct Link<TCell> : IEquatable<Link<TCell>> where TCell : IEquatable<TCell>
    {
        private readonly TCell _cell1;
        private readonly TCell _cell2;

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="cell1">
        ///     One of the cells to link.</param>
        /// <param name="cell2">
        ///     One of the cells to link.</param>
        public Link(TCell cell1, TCell cell2)
        {
            _cell1 = cell1;
            _cell2 = cell2;
        }

        /// <summary>Returns the cells linked by this <see cref="Link{TCell}"/> without guaranteeing an order.</summary>
        public IEnumerable<TCell> Cells { get { yield return _cell1; yield return _cell2; } }

        /// <summary>
        ///     Given a cell, returns the cell on the other side of this link.</summary>
        /// <param name="one">
        ///     One of the cells in this link.</param>
        /// <exception cref="ArgumentException">
        ///     The specified cell is not part of this link.</exception>
        public TCell Other(TCell one) => _cell1.Equals(one) ? _cell2 : _cell2.Equals(one) ? _cell1 : throw new ArgumentException("Cell is not part of the link.", nameof(one));

        /// <inheritdoc/>
        public bool Equals(Link<TCell> other) => (_cell1.Equals(other._cell1) && _cell2.Equals(other._cell2)) || (_cell1.Equals(other._cell2) && _cell2.Equals(other._cell1));

        /// <summary>
        ///     Determines whether the specified <paramref name="cell"/> is part of this link.</summary>
        /// <param name="cell">
        ///     The cell to compare this link against.</param>
        public bool Contains(TCell cell) => _cell1.Equals(cell) || _cell2.Equals(cell);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Link<TCell> link && Equals(link);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var eq = EqualityComparer<TCell>.Default;
            return eq.GetHashCode(_cell1) ^ eq.GetHashCode(_cell2);
        }

        /// <inheritdoc/>
        public override string ToString() => $"{_cell1} ↔ {_cell2}";

        /// <summary>Compares two <see cref="Link{TCell}"/> values for equality.</summary>
        public static bool operator ==(Link<TCell> one, Link<TCell> two) => one.Equals(two);
        /// <summary>Compares two <see cref="Link{TCell}"/> values for inequality.</summary>
        public static bool operator !=(Link<TCell> one, Link<TCell> two) => !one.Equals(two);
    }
}
