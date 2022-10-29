namespace RT.Coordinates
{
    /// <summary>
    ///     Provides a means for a cell in a structure to define directions one can move to another cell.</summary>
    /// <typeparam name="TCell">
    ///     Type of cell (for example, <see cref="Coord"/> or <see cref="Hex"/>).</typeparam>
    /// <typeparam name="TDirection">
    ///     Type (usually an enum type) that defines the set of possible directions (for example, <see
    ///     cref="Coord.Direction"/> or <see cref="Hex.Direction"/>).</typeparam>
    public interface IHasDirection<TCell, TDirection> where TCell : IHasDirection<TCell, TDirection>
    {
        /// <summary>Returns the cell that is <paramref name="amount"/> steps in the specified <paramref name="direction"/>.</summary>
        TCell Move(TDirection direction, int amount = 1);
    }
}
