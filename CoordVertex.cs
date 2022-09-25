namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in a rectilinear grid (<see cref="Grid"/>).</summary>
    public class CoordVertex : Vertex
    {
        /// <summary>Returns the grid cell whose top-left corner is this coordinate.</summary>
        public Coord Cell { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="x">
        ///     The x-coordinate of the cell that has this vertex as its top-left corner.</param>
        /// <param name="y">
        ///     The y-coordinate of the cell that has this vertex as its top-left corner.</param>
        public CoordVertex(int x, int y)
        {
            Cell = new Coord(x, y);
        }

        /// <summary>Constructor.</summary>
        public CoordVertex(Coord cell)
        {
            Cell = cell;
        }

        /// <inheritdoc/>
        public override double X => Cell.X;
        /// <inheritdoc/>
        public override double Y => Cell.Y;

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is CoordVertex cv && cv.Cell.Equals(Cell);
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CoordVertex cv && cv.Cell.Equals(Cell);
        /// <inheritdoc/>
        public override int GetHashCode() => Cell.GetHashCode();
    }
}
