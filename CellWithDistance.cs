using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>
    ///     Encapsulates a cell and its distance from an origin cell within a structure.</summary>
    /// <typeparam name="TCell">
    ///     Type of cell (e.g., <see cref="Square"/>, <see cref="Hex"/> or <see cref="Tri"/>).</typeparam>
    /// <remarks>
    ///     Used in the return value of <see cref="Structure{TCell}.FindPaths(TCell)"/>.</remarks>
    public struct CellWithDistance<TCell>
    {
        /// <summary>The relevant cell.</summary>
        public TCell Cell { get; private set; }
        /// <summary>
        ///     The previous cell in the path from the origin cell to this cell.</summary>
        /// <remarks>
        ///     If <see cref="Distance"/> is <c>0</c>, this value is meaningless and must be ignored.</remarks>
        public TCell Parent { get; private set; }
        /// <summary>
        ///     The amount of steps required to reach this cell from the origin cell. If this is <c>0</c>, this cell is the
        ///     origin cell.</summary>
        public int Distance { get; private set; }

        /// <summary>Constructor.</summary>
        public CellWithDistance(TCell cell, TCell parent, int distance)
        {
            Cell = cell;
            Parent = parent;
            Distance = distance;
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is CellWithDistance<TCell> other && EqualityComparer<TCell>.Default.Equals(Parent, other.Parent) && Distance == other.Distance;

        /// <inheritdoc/>
        public override readonly int GetHashCode() => Parent.GetHashCode() * 1068603923 + Distance;

        /// <summary>Deconstructor.</summary>
        public readonly void Deconstruct(out TCell parent, out int distance)
        {
            parent = Parent;
            distance = Distance;
        }
    }
}
