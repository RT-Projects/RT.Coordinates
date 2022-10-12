namespace RT.Coordinates
{
    public interface IHasDirection<TCell, TDirection> where TCell : IHasDirection<TCell, TDirection>
    {
        TCell Move(TDirection direction, int amount = 1);
    }
}