using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>Describes a 2D grid of square cells.</summary>
    public class Grid : Structure<Coord>
    {
        /// <summary>
        ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
        ///     IEnumerable{TCell}})"/>.</summary>
        public Grid(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> links = null, Func<Coord, IEnumerable<Coord>> getNeighbors = null)
            : base(cells, links, getNeighbors)
        {
        }

        private Grid(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> links, bool toroidalX, bool toroidalY)
            : base(cells, links, null)
        {
            _toroidalX = toroidalX;
            _toroidalY = toroidalY;
        }

        /// <summary>
        ///     Constructs a rectilinear grid that is <paramref name="width"/> cells wide and <paramref name="height"/> cells
        ///     tall.</summary>
        /// <param name="width">
        ///     Width of the grid.</param>
        /// <param name="height">
        ///     Height of the grid.</param>
        /// <param name="includeDiagonalLinks">
        ///     If <c>true</c>, links between diagonally adjacent cells are included in the structure.</param>
        /// <param name="toroidalX">
        ///     If <c>true</c>, treats the grid as horizontally toroidal (the left/right edges wrap around).</param>
        /// <param name="toroidalY">
        ///     If <c>true</c>, treats the grid as vertically toroidal (the top/bottom edges wrap around).</param>
        public Grid(int width, int height, bool includeDiagonalLinks = false, bool toroidalX = false, bool toroidalY = false)
            : base(Coord.Rectangle(width, height), getNeighbors: getNeighborsGetter(width, height, includeDiagonalLinks, toroidalX, toroidalY))
        {
            _width = width;
            _height = height;
            _toroidalX = toroidalX;
            _toroidalY = toroidalY;
        }

        private static Func<Coord, IEnumerable<Coord>> getNeighborsGetter(int width, int height, bool includeDiagonal, bool toroidalX, bool toroidalY)
        {
            return c => get(c);
            IEnumerable<Coord> get(Coord c)
            {
                foreach (var neighbor in c.GetNeighbors(includeDiagonal))
                    yield return neighbor;
                if (toroidalX && c.X == 0)
                    yield return new Coord(width - 1, c.Y);
                if (toroidalX && c.X == width - 1)
                    yield return new Coord(0, c.Y);
                if (toroidalY && c.Y == 0)
                    yield return new Coord(c.X, height - 1);
                if (toroidalY && c.Y == height - 1)
                    yield return new Coord(c.X, 0);
            }
        }

        private readonly int _width;
        private readonly int _height;
        private readonly bool _toroidalX;
        private readonly bool _toroidalY;

        /// <inheritdoc/>
        protected override Structure<Coord> makeModifiedStructure(IEnumerable<Coord> cells, IEnumerable<Link<Coord>> traversible) => new Grid(cells, traversible, _toroidalX, _toroidalY);

        /// <inheritdoc/>
        protected override EdgeType svgEdgeType(Link<Vertex> edge, List<Coord> cells)
        {
            if (cells.Count == 1)
            {
                var c = cells[0];
                if (_toroidalX && c.X == 0 && edge.Cells.All(v => v is CoordVertex cv && cv.Cell.X == 0))
                    return _links.Contains(new Link<Coord>(c, c.MoveXBy(_width - 1))) ? EdgeType.Passage : EdgeType.Wall;
                else if (_toroidalX && c.X == _width - 1 && edge.Cells.All(v => v is CoordVertex cv && cv.Cell.X == _width))
                    return _links.Contains(new Link<Coord>(c, c.MoveXBy(-_width + 1))) ? EdgeType.Passage : EdgeType.Wall;
                else if (_toroidalY && c.Y == 0 && edge.Cells.All(v => v is CoordVertex cv && cv.Cell.Y == 0))
                    return _links.Contains(new Link<Coord>(c, c.MoveYBy(_height - 1))) ? EdgeType.Passage : EdgeType.Wall;
                else if (_toroidalY && c.Y == _height - 1 && edge.Cells.All(v => v is CoordVertex cv && cv.Cell.Y == _height))
                    return _links.Contains(new Link<Coord>(c, c.MoveYBy(-_height + 1))) ? EdgeType.Passage : EdgeType.Wall;
            }
            return base.svgEdgeType(edge, cells);
        }
    }
}
