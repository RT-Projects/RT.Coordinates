using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Describes a vertex (gridline intersection) in a rectilinear grid (<see cref="Grid"/>).</summary>
    public class CoordVertex : Vertex<Coord>
    {
        /// <summary>Returns the x-coordinate of the vertex within the grid.</summary>
        public int GridX { get; private set; }
        /// <summary>Returns the y-coordinate of the vertex within the grid.</summary>
        public int GridY { get; private set; }
        /// <summary>Returns the width of the grid.</summary>
        public int Width { get; private set; }
        /// <summary>Returns the height of the grid.</summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="x">
        ///     The x-coordinate of the cell that has this vertex as its top-left corner.</param>
        /// <param name="y">
        ///     The y-coordinate of the cell that has this vertex as its top-left corner.</param>
        /// <param name="width">
        ///     The width of the whole grid.</param>
        /// <param name="height">
        ///     The height of the whole grid.</param>
        public CoordVertex(int x, int y, int width, int height)
        {
            if (x < 0 || x > width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"‘{nameof(x)}’ must be in the range 0 to ‘{nameof(width)}’.");
            if (y < 0 || y > height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"‘{nameof(y)}’ must be in the range 0 to ‘{nameof(height)}’.");
            GridX = x;
            GridY = y;
            Width = width;
            Height = height;
        }

        /// <inheritdoc/>
        public override IEnumerable<Coord> Cells
        {
            get
            {
                if (GridX > 0 && GridY > 0)
                    yield return new Coord(Width, Height, GridX - 1, GridY - 1);
                if (GridX < Width && GridY > 0)
                    yield return new Coord(Width, Height, GridX, GridY - 1);
                if (GridX > 0 && GridY < Height)
                    yield return new Coord(Width, Height, GridX - 1, GridY);
                if (GridX < Width && GridY < Height)
                    yield return new Coord(Width, Height, GridX, GridY);
            }
        }

        /// <inheritdoc/>
        public override double X => GridX;
        /// <inheritdoc/>
        public override double Y => GridY;

        /// <inheritdoc/>
        public override bool Equals(Vertex<Coord> other) => other is CoordVertex cv && cv.GridX == GridX && cv.GridY == GridY;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CoordVertex cv && Equals(cv);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(GridX * 1048589 + GridY);
    }
}
