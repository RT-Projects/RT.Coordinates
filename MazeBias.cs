namespace RT.Coordinates;

/// <summary>
///     Describes a way in which the maze generation algorithm (<see cref="Structure{TCell}.GenerateMaze(System.Func{int, int,
///     int}, MazeBias)"/>) proceeds.</summary>
public enum MazeBias
{
    /// <summary>The algorithm picks cells at random without bias.</summary>
    Default,
    /// <summary>
    ///     The algorithm exhausts all links of each new cell before moving on to the next (breadth-first). This generally
    ///     leads to long, straight corridors.</summary>
    Straight,
    /// <summary>
    ///     The algorithm explores a link for every new cell before returning to the previous (depth-first). This generally
    ///     leads to more winding paths.</summary>
    Winding
}
