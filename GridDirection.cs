namespace RT.Coordinates
{
    /// <summary>Identifies a direction within a 2D rectilinear grid.</summary>
    public enum GridDirection
    {
        /// <summary>Up (dx = 0, dy = -1).</summary>
        Up,
        /// <summary>Up and right (dx = 1, dy = -1).</summary>
        UpRight,
        /// <summary>Right (dx = 1, dy = 0).</summary>
        Right,
        /// <summary>Down and right (dx = 1, dy = 1).</summary>
        DownRight,
        /// <summary>Down (dx = 0, dy = 1).</summary>
        Down,
        /// <summary>Down and left (dx = -1, dy = 1).</summary>
        DownLeft,
        /// <summary>Left (dx = -1, dy = 0).</summary>
        Left,
        /// <summary>Up and left (dx = -1, dy = -1).</summary>
        UpLeft
    }
}
