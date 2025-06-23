using System;

namespace RT.Coordinates
{
    /// <summary>
    ///     Specifies that a cell’s shape should be modified to make the whole structure a rectangle with straight edges all
    ///     around. Only some grid geometries support this.</summary>
    [Flags]
    public enum AtEdges
    {
        /// <summary>This cell is unmodified.</summary>
        None = 0,
        /// <summary>This cell is at the top edge of a rectangular structure.</summary>
        Top = 1 << 0,
        /// <summary>This cell is at the right edge of a rectangular structure.</summary>
        Right = 1 << 1,
        /// <summary>This cell is at the bottom edge of a rectangular structure.</summary>
        Bottom = 1 << 2,
        /// <summary>This cell is at the left edge of a rectangular structure.</summary>
        Left = 1 << 3,
    }
}
