using System;
using System.Collections.Generic;
using System.Linq;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Describes a cell in a <see cref="Grid"/> consisting of hexagons, squares and triangles. Each hexagon is
    ///         surrounded by a ring alternating between squares and tris, and shares each square with the next adjoining
    ///         hexagon.</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.5 -3.5 7 7'&gt;&lt;path d='M3.36602540378444
    ///     -4.09807621135332L2.86602540378444 -3.23205080735332L1.86602540378444 -3.23205080735332L1.36602540378444
    ///     -4.09807621135332M2.86602540378444 -2.23205080778444L3.73205080756888 -2.73205080756888L2.86602540378444
    ///     -3.23205080735332zM0.5 -3.59807621156888L1 -2.73205080756888L0.5 -1.86602540356888L-0.5 -1.86602540356888L-1
    ///     -2.73205080756888L-1.86602540378444 -2.23205080778444L-1.86602540378444 -3.23205080735332L-1
    ///     -2.73205080756888L-0.5 -3.59807621156888M4.23205080756888 -0.866025404L3.36602540378444
    ///     -1.36602540378444L4.23205080756888 -1.86602540356888M-1.36602540378444 -4.09807621135332L-1.86602540378444
    ///     -3.23205080735332L-2.86602540378444 -3.23205080735332L-3.36602540378444 -4.09807621135332M-4.23205080756888
    ///     -1.86602540356888L-3.36602540378444 -1.36602540378444L-2.86602540378444 -2.23205080778444L-3.73205080756888
    ///     -2.73205080756888L-2.86602540378444 -3.23205080735332L-2.86602540378444 -2.23205080778444L-1.86602540378444
    ///     -2.23205080778444L-1.36602540378444 -1.36602540378444L-1.86602540378444 -0.49999999978444L-2.86602540378444
    ///     -0.49999999978444L-3.36602540378444 -1.36602540378444L-4.23205080756888 -0.866025404M1.36602540378444
    ///     -1.36602540378444L0.5 -0.866025404L0.5 -1.86602540356888L1.36602540378444 -1.36602540378444L1.86602540378444
    ///     -2.23205080778444L1 -2.73205080756888L1.86602540378444 -3.23205080735332L1.86602540378444
    ///     -2.23205080778444L2.86602540378444 -2.23205080778444L3.36602540378444 -1.36602540378444L2.86602540378444
    ///     -0.49999999978444L1.86602540378444 -0.49999999978444zM2.86602540378444 0.49999999978444L3.73205080756888
    ///     0L2.86602540378444 -0.49999999978444zM4.23205080756888 1.86602540356888L3.36602540378444
    ///     1.36602540378444L4.23205080756888 0.866025404M-1 0L-1.86602540378444 0.49999999978444L-1.86602540378444
    ///     -0.49999999978444L-1 0L-0.5 -0.866025404L-1.36602540378444 -1.36602540378444L-0.5 -1.86602540356888L-0.5
    ///     -0.866025404L0.5 -0.866025404L1 0L0.5 0.866025404L-0.5 0.866025404zM-4.23205080756888
    ///     0.866025404L-3.36602540378444 1.36602540378444L-2.86602540378444 0.49999999978444L-3.73205080756888
    ///     0L-2.86602540378444 -0.49999999978444L-2.86602540378444 0.49999999978444L-1.86602540378444
    ///     0.49999999978444L-1.36602540378444 1.36602540378444L-1.86602540378444 2.23205080778444L-2.86602540378444
    ///     2.23205080778444L-3.36602540378444 1.36602540378444L-4.23205080756888 1.86602540356888M1.36602540378444
    ///     1.36602540378444L0.5 1.86602540356888L0.5 0.866025404L1.36602540378444 1.36602540378444L1.86602540378444
    ///     0.49999999978444L1 0L1.86602540378444 -0.49999999978444L1.86602540378444 0.49999999978444L2.86602540378444
    ///     0.49999999978444L3.36602540378444 1.36602540378444L2.86602540378444 2.23205080778444L1.86602540378444
    ///     2.23205080778444zM2.86602540378444 3.23205080735332L3.73205080756888 2.73205080756888L2.86602540378444
    ///     2.23205080778444zM1.36602540378444 4.09807621135332L1.86602540378444 3.23205080735332L1
    ///     2.73205080756888L1.86602540378444 2.23205080778444L1.86602540378444 3.23205080735332L2.86602540378444
    ///     3.23205080735332L3.36602540378444 4.09807621135332M-0.5 3.59807621156888L-1 2.73205080756888L-1.86602540378444
    ///     3.23205080735332L-1.86602540378444 2.23205080778444L-1 2.73205080756888L-0.5 1.86602540356888L-1.36602540378444
    ///     1.36602540378444L-0.5 0.866025404L-0.5 1.86602540356888L0.5 1.86602540356888L1 2.73205080756888L0.5
    ///     3.59807621156888M-3.36602540378444 4.09807621135332L-2.86602540378444 3.23205080735332L-3.73205080756888
    ///     2.73205080756888L-2.86602540378444 2.23205080778444L-2.86602540378444 3.23205080735332L-1.86602540378444
    ///     3.23205080735332L-1.36602540378444 4.09807621135332' fill='none' stroke-width='.05' stroke='black'
    ///     /&gt;&lt;/svg&gt;</image>
    public struct Rhombihexadel : IEquatable<Rhombihexadel>, INeighbor<Rhombihexadel>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>
        ///     Constructor.</summary>
        /// <param name="hex">
        ///     The hex tile that <paramref name="subtile"/> is relative to.</param>
        /// <param name="subtile">
        ///     The subtile adjacent to <paramref name="hex"/>. Note that for tiles not listed in the <see cref="Tile"/> enum,
        ///     you will have to choose an adjoining hex. For example, a hex’s “top-left square” is the <see
        ///     cref="Tile.BottomRightSquare"/> of the hexagon that is <see cref="Hex.Direction.UpLeft"/> from it.</param>
        public Rhombihexadel(Hex hex, Tile subtile)
        {
            Hex = hex;
            Subtile = subtile;
        }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="q">
        ///     The Q-coordinate of the hex tile that <paramref name="subtile"/> is relative to.</param>
        /// <param name="r">
        ///     The R-coordinate of the hex tile that <paramref name="subtile"/> is relative to.</param>
        /// <param name="subtile">
        ///     The subtile adjacent to the hex tile. Note that for tiles not listed in the <see cref="Tile"/> enum, you will
        ///     have to choose an adjoining hex. For example, a hex’s “top-left square” is the <see
        ///     cref="Tile.BottomRightSquare"/> of the hexagon that is <see cref="Hex.Direction.UpLeft"/> from it.</param>
        public Rhombihexadel(int q, int r, Tile subtile)
        {
            Hex = new Hex(q, r);
            Subtile = subtile;
        }

        /// <summary>The hexagon that <see cref="Tile"/> is relative to.</summary>
        public Hex Hex { get; private set; }
        /// <summary>The tile, expressed relative to <see cref="Hex"/>.</summary>
        public Tile Subtile { get; private set; }

        /// <summary>Identifies one of the Rhombihexadel tiles relative to <see cref="Hex"/>.</summary>
        public enum Tile
        {
            /// <summary>The hexagon itself.</summary>
            Hexagon,
            /// <summary>The square at the bottom-right of <see cref="Hex"/>.</summary>
            BottomRightSquare,
            /// <summary>The tri at the bottom-right of <see cref="Hex"/>.</summary>
            BottomRightTri,
            /// <summary>The square at the bottom of <see cref="Hex"/>.</summary>
            BottomSquare,
            /// <summary>The tri at the bottom-left of <see cref="Hex"/>.</summary>
            BottomLeftTri,
            /// <summary>The square at the bottom-left of <see cref="Hex"/>.</summary>
            BottomLeftSquare
        }

        /// <inheritdoc/>
        public readonly bool Equals(Rhombihexadel other) => other.Hex.Equals(Hex) && other.Subtile == Subtile;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Rhombihexadel other && other.Hex.Equals(Hex) && other.Subtile == Subtile;
        /// <inheritdoc/>
        public override readonly int GetHashCode() => unchecked(Hex.GetHashCode() * 7 + (int) Subtile);

        /// <summary>Compares two <see cref="Rhombihexadel"/> values for equality.</summary>
        public static bool operator ==(Rhombihexadel one, Rhombihexadel two) => one.Hex == two.Hex && one.Subtile == two.Subtile;
        /// <summary>Compares two <see cref="Rhombihexadel"/> values for inequality.</summary>
        public static bool operator !=(Rhombihexadel one, Rhombihexadel two) => one.Hex != two.Hex || one.Subtile != two.Subtile;

        /// <inheritdoc/>
        public readonly IEnumerable<Rhombihexadel> Neighbors
        {
            get
            {
                if (Subtile == Tile.Hexagon)
                {
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.Up), Tile.BottomSquare);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.UpRight), Tile.BottomLeftSquare);
                    yield return new Rhombihexadel(Hex, Tile.BottomRightSquare);
                    yield return new Rhombihexadel(Hex, Tile.BottomSquare);
                    yield return new Rhombihexadel(Hex, Tile.BottomLeftSquare);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.UpLeft), Tile.BottomRightSquare);
                }
                else if (Subtile == Tile.BottomRightSquare)
                {
                    yield return new Rhombihexadel(Hex, Tile.Hexagon);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.UpRight), Tile.BottomLeftTri);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.DownRight), Tile.Hexagon);
                    yield return new Rhombihexadel(Hex, Tile.BottomRightTri);
                }
                else if (Subtile == Tile.BottomRightTri)
                {
                    yield return new Rhombihexadel(Hex, Tile.BottomRightSquare);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.DownRight), Tile.BottomLeftSquare);
                    yield return new Rhombihexadel(Hex, Tile.BottomSquare);
                }
                else if (Subtile == Tile.BottomSquare)
                {
                    yield return new Rhombihexadel(Hex, Tile.Hexagon);
                    yield return new Rhombihexadel(Hex, Tile.BottomRightTri);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.Down), Tile.Hexagon);
                    yield return new Rhombihexadel(Hex, Tile.BottomLeftTri);
                }
                else if (Subtile == Tile.BottomLeftTri)
                {
                    yield return new Rhombihexadel(Hex, Tile.BottomSquare);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.DownLeft), Tile.BottomRightSquare);
                    yield return new Rhombihexadel(Hex, Tile.BottomLeftSquare);
                }
                else if (Subtile == Tile.BottomLeftSquare)
                {
                    yield return new Rhombihexadel(Hex, Tile.Hexagon);
                    yield return new Rhombihexadel(Hex, Tile.BottomLeftTri);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.DownLeft), Tile.Hexagon);
                    yield return new Rhombihexadel(Hex.Move(Hex.Direction.UpLeft), Tile.BottomRightTri);
                }
                else
                    throw new InvalidOperationException($"Invalid {nameof(Subtile)} value: ‘{Subtile}’.");
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <summary>
        ///     Returns a collection of <see cref="Rhombihexadel"/> tiles that form a larger hexagonal structure in which
        ///     every hexagon is fully surrounded by all its neighboring squares and tris.</summary>
        /// <param name="sideLength">
        ///     Side length of the hexagon structure to produce.</param>
        public static IEnumerable<Rhombihexadel> LargeHexagon(int sideLength) => Hex.LargeHexagon(sideLength).SelectMany(hex => new[]
        {
            new Rhombihexadel(hex, Tile.Hexagon),
            new Rhombihexadel(hex.Move(Hex.Direction.Up), Tile.BottomSquare),
            new Rhombihexadel(hex.Move(Hex.Direction.Up), Tile.BottomRightTri),
            new Rhombihexadel(hex.Move(Hex.Direction.UpRight), Tile.BottomLeftSquare),
            new Rhombihexadel(hex.Move(Hex.Direction.UpRight), Tile.BottomLeftTri),
            new Rhombihexadel(hex, Tile.BottomRightSquare),
            new Rhombihexadel(hex, Tile.BottomRightTri),
            new Rhombihexadel(hex, Tile.BottomSquare),
            new Rhombihexadel(hex, Tile.BottomLeftTri),
            new Rhombihexadel(hex, Tile.BottomLeftSquare),
            new Rhombihexadel(hex.Move(Hex.Direction.UpLeft), Tile.BottomRightTri),
            new Rhombihexadel(hex.Move(Hex.Direction.UpLeft), Tile.BottomRightSquare),
            new Rhombihexadel(hex.Move(Hex.Direction.Up), Tile.BottomLeftTri)
        }).Distinct();

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Rhombihexadel"/>, going clockwise from the
        ///     top-left (hexagon) or starting with the two edges adjacent to its corresponding hexagon.</summary>
        public readonly Coordinates.Vertex[] Vertices => Subtile switch
        {
            Tile.Hexagon => [new Vertex(Hex, Vertex.Position.TopLeft), new Vertex(Hex, Vertex.Position.TopRight), new Vertex(Hex, Vertex.Position.Right), new Vertex(Hex, Vertex.Position.BottomRight), new Vertex(Hex, Vertex.Position.BottomLeft), new Vertex(Hex, Vertex.Position.Left)],
            Tile.BottomRightSquare => [new Vertex(Hex, Vertex.Position.BottomRight), new Vertex(Hex, Vertex.Position.Right), new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.TopLeft), new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.Left)],
            Tile.BottomRightTri => [new Vertex(Hex, Vertex.Position.BottomRight), new Vertex(Hex.Move(Hex.Direction.DownRight), Vertex.Position.Left), new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopRight)],
            Tile.BottomSquare => [new Vertex(Hex, Vertex.Position.BottomLeft), new Vertex(Hex, Vertex.Position.BottomRight), new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopRight), new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopLeft)],
            Tile.BottomLeftTri => [new Vertex(Hex, Vertex.Position.BottomLeft), new Vertex(Hex.Move(Hex.Direction.Down), Vertex.Position.TopLeft), new Vertex(Hex.Move(Hex.Direction.DownLeft), Vertex.Position.Right)],
            Tile.BottomLeftSquare => [new Vertex(Hex, Vertex.Position.Left), new Vertex(Hex, Vertex.Position.BottomLeft), new Vertex(Hex.Move(Hex.Direction.DownLeft), Vertex.Position.Right), new Vertex(Hex.Move(Hex.Direction.DownLeft), Vertex.Position.TopRight)],
            _ => throw new InvalidOperationException($"Invalid {nameof(Subtile)} value: ‘{Subtile}’.")
        };

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        private static readonly double[] _xs = [0, 1.18301270189222, .788675134594814, 0, -.788675134594814, -1.18301270189222];
        private static readonly double[] _ys = [0, .683012701892220, 1.36602540378444, 1.36602540378444, 1.36602540378444, .683012701892220];

        /// <inheritdoc/>
        public readonly PointD Center => new(Hex.Q * 2.36602540378444 + _xs[(int) Subtile], Hex.R * 2.73205080756888 + Hex.Q * 1.36602540378444 + _ys[(int) Subtile]);

        /// <inheritdoc/>
        public override readonly string ToString() => $"M({Hex.Q},{Hex.R})/{(int) Subtile}";

        /// <summary>
        ///     Describes a vertex in a <see cref="Rhombihexadel"/> grid. Each vertex is actually one of the 6 vertices of a
        ///     hexagon tile.</summary>
        public class Vertex(Hex hex, Vertex.Position pos) : Coordinates.Vertex
        {
            /// <summary>The relevant hexagon tile that shares this vertex.</summary>
            public Hex Hex { get; private set; } = hex;
            /// <summary>Which vertex on <see cref="Hex"/> this is.</summary>
            public Position Pos { get; private set; } = pos;

            private static readonly double[] _xs = [-.5, .5, 1, .5, -.5, -1];
            private static readonly double[] _ys = [-.8660254040, -.8660254040, 0, .8660254040, .8660254040, 0];

            /// <inheritdoc/>
            public override PointD Point => new(Hex.Q * 2.36602540378444 + _xs[(int) Pos], Hex.R * 2.73205080756888 + Hex.Q * 1.36602540378444 + _ys[(int) Pos]);

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex vx && vx.Hex.Equals(Hex) && vx.Pos == Pos;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex vx && vx.Hex.Equals(Hex) && vx.Pos == Pos;
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Hex.GetHashCode() * 7 + (int) Pos);

            /// <summary>
            ///     Identifies one of the six vertices of a hexagon. Every vertex in a <see cref="Rhombihexadel"/> grid is one
            ///     of these as it is always shared by exactly one hexagon tile.</summary>
            public enum Position
            {
                /// <summary>The top-left vertex of the hexagon.</summary>
                TopLeft,
                /// <summary>The top-right vertex of the hexagon.</summary>
                TopRight,
                /// <summary>The right vertex of the hexagon.</summary>
                Right,
                /// <summary>The bottom-right vertex of the hexagon.</summary>
                BottomRight,
                /// <summary>The bottom-left vertex of the hexagon.</summary>
                BottomLeft,
                /// <summary>The left vertex of the hexagon.</summary>
                Left
            }
        }

        /// <summary>
        ///     Describes a grid of <see cref="Rhombihexadel"/> cells, which are a mix of hexagons, squares and triangles.</summary>
        public class Grid : Structure<Rhombihexadel>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Rhombihexadel> cells, IEnumerable<Link<Rhombihexadel>> links = null, Func<Rhombihexadel, IEnumerable<Rhombihexadel>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Rhombihexadel"/> grid that forms a larger hexagonal structure in which every
            ///     hexagon is fully surrounded by all its neighboring squares and tris.</summary>
            /// <param name="sideLength">
            ///     Side length of the hexagon structure to produce.</param>
            public Grid(int sideLength)
                : base(LargeHexagon(sideLength))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Rhombihexadel> makeModifiedStructure(IEnumerable<Rhombihexadel> cells, IEnumerable<Link<Rhombihexadel>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }
    }
}
