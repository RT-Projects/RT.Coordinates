using System;
using System.Collections.Generic;

namespace RT.Coordinates
{
    /// <summary>Represents a cell in an <see cref="OctoGrid"/>. Each cell may be an octagon or a square.</summary>
    public struct OctoCell : IEquatable<OctoCell>, IHasSvgGeometry, INeighbor<OctoCell>
    {
        /// <summary>X-coordinate of the cell.</summary>
        public int X { get; private set; }
        /// <summary>Y-coordinate of the cell.</summary>
        public int Y { get; private set; }
        /// <summary>
        ///     Specifies whether this cell is a square or an octagon. If it is a square, it is south-east of the octagon with
        ///     the same coordinates.</summary>
        public bool IsSquare { get; private set; }

        /// <summary>Constructor.</summary>
        public OctoCell(int x, int y, bool isSquare)
        {
            X = x;
            Y = y;
            IsSquare = isSquare;
        }

        /// <summary>
        ///     Returns a set of <see cref="OctoCell"/> cells that form a rectangle. Along the perimeter all the cells will be
        ///     octagons. Only squares internal to these octagons are included.</summary>
        /// <param name="width">
        ///     The number of octagons in the x direction.</param>
        /// <param name="height">
        ///     The number of octagons in the y direction.</param>
        public static IEnumerable<OctoCell> LargeRectangle(int width, int height)
        {
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    yield return new OctoCell(x, y, false);
                    if (x < width - 1 && y < height - 1)
                        yield return new OctoCell(x, y, true);
                }
        }

        /// <inheritdoc/>
        public bool Equals(OctoCell other) => other.X == X && other.Y == Y && other.IsSquare == IsSquare;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is OctoCell oc && Equals(oc);
        /// <inheritdoc/>
        public override int GetHashCode() => X * 1073741827 + Y * 47 + (IsSquare ? 1 : 0);

        /// <inheritdoc/>
        public IEnumerable<Link<Vertex>> Edges => IsSquare
            ? new[] { new OctoVertex(X + 1, Y, 0), new OctoVertex(X + 1, Y + 1, 2), new OctoVertex(X + 1, Y + 1, 1), new OctoVertex(X, Y + 1, 3) }.MakeEdges()
            : new[] { new OctoVertex(X, Y, 0), new OctoVertex(X, Y, 1), new OctoVertex(X, Y, 2), new OctoVertex(X, Y, 3), new OctoVertex(X + 1, Y, 1), new OctoVertex(X + 1, Y, 0), new OctoVertex(X, Y + 1, 3), new OctoVertex(X, Y + 1, 2) }.MakeEdges();

        /// <inheritdoc/>
        public PointD Center => IsSquare ? new PointD(X + 1, Y + 1) : new PointD(X + .5, Y + .5);

        /// <inheritdoc/>
        public IEnumerable<OctoCell> Neighbors => IsSquare
            ? new[] { new OctoCell(X, Y, false), new OctoCell(X + 1, Y, false), new OctoCell(X + 1, Y + 1, false), new OctoCell(X, Y + 1, false) }
            : new[] { new OctoCell(X, Y - 1, true), new OctoCell(X, Y - 1, true), new OctoCell(X + 1, Y, false), new OctoCell(X, Y, true), new OctoCell(X, Y + 1, false), new OctoCell(X - 1, Y, true), new OctoCell(X - 1, Y, false) };

        /// <inheritdoc/>
        public override string ToString() => string.Format(IsSquare ? "[{0},{1}]" : "({0},{1})", X, Y);
    }
}
