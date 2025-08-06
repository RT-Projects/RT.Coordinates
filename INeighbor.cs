using System.Collections.Generic;

namespace RT.Coordinates;

/// <summary>
///     Exposes a means for a cell to report what all of its neighbors are.</summary>
/// <typeparam name="TCell">
///     The type of cell.</typeparam>
public interface INeighbor<TCell>
{
    /// <summary>Returns the current cell’s neighbors.</summary>
    IEnumerable<TCell> Neighbors { get; }
}
