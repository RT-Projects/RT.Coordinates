namespace RT.Coordinates
{
    /// <summary>Identifies a direction within a 2D hexagonal grid.</summary>
    public enum HexDirection
    {
        /// <summary>Up and left (dq = -1, dr = 0).</summary>
        UpLeft,
        /// <summary>Up (dq = 0, dr = -1).</summary>
        Up,
        /// <summary>Up and right (dq = 1, dr = -1).</summary>
        UpRight,
        /// <summary>Down and right (dq = 1, dr = 0).</summary>
        DownRight,
        /// <summary>Down (dq = 0, dr = 1).</summary>
        Down,
        /// <summary>Down and left (dq = -1, dr = 1).</summary>
        DownLeft
    }
}
