using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a tile in a 2D flat-topped hexagonal grid.</para></summary>
    /// <remarks>
    ///     Represents a hexagonal tile in a two-dimensional grid in which each tile is a hexagon with a flat top and bottom
    ///     and two of its vertices pointing left and right. Each hex is represented as a pair of coordinates (Q, R), where an
    ///     increasing Q coordinate moves down and right, while an increasing R coordinate moves down.</remarks>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.5 -3.5 7 7'&gt;&lt;path d='M4 -3.46410161513775L3.5
    ///     -3.46410161513775L3.25 -3.89711431702997M2.75 -3.89711431702997L2.5 -3.46410161513775L2 -3.46410161513775L1.75
    ///     -3.89711431702997M3.5 -3.46410161513775L3.25 -3.03108891324554L2.75 -3.03108891324554L2.5 -3.46410161513775M4
    ///     -2.59807621135332L3.5 -2.59807621135332L3.25 -3.03108891324554M1.25 -3.89711431702997L1 -3.46410161513775L0.5
    ///     -3.46410161513775L0.25 -3.89711431702997M2 -3.46410161513775L1.75 -3.03108891324554L1.25 -3.03108891324554L1
    ///     -3.46410161513775M2.75 -3.03108891324554L2.5 -2.59807621135332L2 -2.59807621135332L1.75 -3.03108891324554M3.5
    ///     -2.59807621135332L3.25 -2.1650635094611L2.75 -2.1650635094611L2.5 -2.59807621135332M4 -1.73205080756888L3.5
    ///     -1.73205080756888L3.25 -2.1650635094611M-0.25 -3.89711431702997L-0.5 -3.46410161513775L-1 -3.46410161513775L-1.25
    ///     -3.89711431702997M0.5 -3.46410161513775L0.25 -3.03108891324554L-0.25 -3.03108891324554L-0.5 -3.46410161513775M1.25
    ///     -3.03108891324554L1 -2.59807621135332L0.5 -2.59807621135332L0.25 -3.03108891324554M2 -2.59807621135332L1.75
    ///     -2.1650635094611L1.25 -2.1650635094611L1 -2.59807621135332M2.75 -2.1650635094611L2.5 -1.73205080756888L2
    ///     -1.73205080756888L1.75 -2.1650635094611M3.5 -1.73205080756888L3.25 -1.29903810567666L2.75 -1.29903810567666L2.5
    ///     -1.73205080756888M4 -0.866025403784439L3.5 -0.866025403784439L3.25 -1.29903810567666M-1.75 -3.89711431702997L-2
    ///     -3.46410161513775L-2.5 -3.46410161513775L-2.75 -3.89711431702997M-1 -3.46410161513775L-1.25
    ///     -3.03108891324554L-1.75 -3.03108891324554L-2 -3.46410161513775M-0.25 -3.03108891324554L-0.5 -2.59807621135332L-1
    ///     -2.59807621135332L-1.25 -3.03108891324554M0.5 -2.59807621135332L0.25 -2.1650635094611L-0.25 -2.1650635094611L-0.5
    ///     -2.59807621135332M1.25 -2.1650635094611L1 -1.73205080756888L0.5 -1.73205080756888L0.25 -2.1650635094611M2
    ///     -1.73205080756888L1.75 -1.29903810567666L1.25 -1.29903810567666L1 -1.73205080756888M2.75 -1.29903810567666L2.5
    ///     -0.866025403784439L2 -0.866025403784439L1.75 -1.29903810567666M3.5 -0.866025403784439L3.25 -0.433012701892219L2.75
    ///     -0.433012701892219L2.5 -0.866025403784439M4 0L3.5 0L3.25 -0.433012701892219M-3.25 -3.89711431702997L-3.5
    ///     -3.46410161513775L-4 -3.46410161513775M-2.5 -3.46410161513775L-2.75 -3.03108891324554L-3.25 -3.03108891324554L-3.5
    ///     -3.46410161513775M-1.75 -3.03108891324554L-2 -2.59807621135332L-2.5 -2.59807621135332L-2.75 -3.03108891324554M-1
    ///     -2.59807621135332L-1.25 -2.1650635094611L-1.75 -2.1650635094611L-2 -2.59807621135332M-0.25 -2.1650635094611L-0.5
    ///     -1.73205080756888L-1 -1.73205080756888L-1.25 -2.1650635094611M0.5 -1.73205080756888L0.25 -1.29903810567666L-0.25
    ///     -1.29903810567666L-0.5 -1.73205080756888M1.25 -1.29903810567666L1 -0.866025403784439L0.5 -0.866025403784439L0.25
    ///     -1.29903810567666M2 -0.866025403784439L1.75 -0.433012701892219L1.25 -0.433012701892219L1 -0.866025403784439M2.75
    ///     -0.433012701892219L2.5 0L2 0L1.75 -0.433012701892219M3.5 0L3.25 0.433012701892219L2.75 0.433012701892219L2.5 0M4
    ///     0.866025403784439L3.5 0.866025403784439L3.25 0.433012701892219M-3.25 -3.03108891324554L-3.5 -2.59807621135332L-4
    ///     -2.59807621135332M-2.5 -2.59807621135332L-2.75 -2.1650635094611L-3.25 -2.1650635094611L-3.5
    ///     -2.59807621135332M-1.75 -2.1650635094611L-2 -1.73205080756888L-2.5 -1.73205080756888L-2.75 -2.1650635094611M-1
    ///     -1.73205080756888L-1.25 -1.29903810567666L-1.75 -1.29903810567666L-2 -1.73205080756888M-0.25
    ///     -1.29903810567666L-0.5 -0.866025403784439L-1 -0.866025403784439L-1.25 -1.29903810567666M0.5
    ///     -0.866025403784439L0.25 -0.433012701892219L-0.25 -0.433012701892219L-0.5 -0.866025403784439M1.25
    ///     -0.433012701892219L1 0L0.5 0L0.25 -0.433012701892219M2 0L1.75 0.433012701892219L1.25 0.433012701892219L1 0M2.75
    ///     0.433012701892219L2.5 0.866025403784439L2 0.866025403784439L1.75 0.433012701892219M3.5 0.866025403784439L3.25
    ///     1.29903810567666L2.75 1.29903810567666L2.5 0.866025403784439M4 1.73205080756888L3.5 1.73205080756888L3.25
    ///     1.29903810567666M-3.25 -2.1650635094611L-3.5 -1.73205080756888L-4 -1.73205080756888M-2.5 -1.73205080756888L-2.75
    ///     -1.29903810567666L-3.25 -1.29903810567666L-3.5 -1.73205080756888M-1.75 -1.29903810567666L-2
    ///     -0.866025403784439L-2.5 -0.866025403784439L-2.75 -1.29903810567666M-1 -0.866025403784439L-1.25
    ///     -0.433012701892219L-1.75 -0.433012701892219L-2 -0.866025403784439M-0.25 -0.433012701892219L-0.5 0L-1 0L-1.25
    ///     -0.433012701892219M0.5 0L0.25 0.433012701892219L-0.25 0.433012701892219L-0.5 0M1.25 0.433012701892219L1
    ///     0.866025403784439L0.5 0.866025403784439L0.25 0.433012701892219M2 0.866025403784439L1.75 1.29903810567666L1.25
    ///     1.29903810567666L1 0.866025403784439M2.75 1.29903810567666L2.5 1.73205080756888L2 1.73205080756888L1.75
    ///     1.29903810567666M3.5 1.73205080756888L3.25 2.1650635094611L2.75 2.1650635094611L2.5 1.73205080756888M4
    ///     2.59807621135332L3.5 2.59807621135332L3.25 2.1650635094611M-3.25 -1.29903810567666L-3.5 -0.866025403784439L-4
    ///     -0.866025403784439M-2.5 -0.866025403784439L-2.75 -0.433012701892219L-3.25 -0.433012701892219L-3.5
    ///     -0.866025403784439M-1.75 -0.433012701892219L-2 0L-2.5 0L-2.75 -0.433012701892219M-1 0L-1.25
    ///     0.433012701892219L-1.75 0.433012701892219L-2 0M-0.25 0.433012701892219L-0.5 0.866025403784439L-1
    ///     0.866025403784439L-1.25 0.433012701892219M0.5 0.866025403784439L0.25 1.29903810567666L-0.25 1.29903810567666L-0.5
    ///     0.866025403784439M1.25 1.29903810567666L1 1.73205080756888L0.5 1.73205080756888L0.25 1.29903810567666M2
    ///     1.73205080756888L1.75 2.1650635094611L1.25 2.1650635094611L1 1.73205080756888M2.75 2.1650635094611L2.5
    ///     2.59807621135332L2 2.59807621135332L1.75 2.1650635094611M3.5 2.59807621135332L3.25 3.03108891324554L2.75
    ///     3.03108891324554L2.5 2.59807621135332M4 3.46410161513775L3.5 3.46410161513775L3.25 3.03108891324554M-3.25
    ///     -0.433012701892219L-3.5 0L-4 0M-2.5 0L-2.75 0.433012701892219L-3.25 0.433012701892219L-3.5 0M-1.75
    ///     0.433012701892219L-2 0.866025403784439L-2.5 0.866025403784439L-2.75 0.433012701892219M-1 0.866025403784439L-1.25
    ///     1.29903810567666L-1.75 1.29903810567666L-2 0.866025403784439M-0.25 1.29903810567666L-0.5 1.73205080756888L-1
    ///     1.73205080756888L-1.25 1.29903810567666M0.5 1.73205080756888L0.25 2.1650635094611L-0.25 2.1650635094611L-0.5
    ///     1.73205080756888M1.25 2.1650635094611L1 2.59807621135332L0.5 2.59807621135332L0.25 2.1650635094611M2
    ///     2.59807621135332L1.75 3.03108891324554L1.25 3.03108891324554L1 2.59807621135332M2.75 3.03108891324554L2.5
    ///     3.46410161513775L2 3.46410161513775L1.75 3.03108891324554M3.5 3.46410161513775L3.25 3.89711431702997M2.75
    ///     3.89711431702997L2.5 3.46410161513775M-3.25 0.433012701892219L-3.5 0.866025403784439L-4 0.866025403784439M-2.5
    ///     0.866025403784439L-2.75 1.29903810567666L-3.25 1.29903810567666L-3.5 0.866025403784439M-1.75 1.29903810567666L-2
    ///     1.73205080756888L-2.5 1.73205080756888L-2.75 1.29903810567666M-1 1.73205080756888L-1.25 2.1650635094611L-1.75
    ///     2.1650635094611L-2 1.73205080756888M-0.25 2.1650635094611L-0.5 2.59807621135332L-1 2.59807621135332L-1.25
    ///     2.1650635094611M0.5 2.59807621135332L0.25 3.03108891324554L-0.25 3.03108891324554L-0.5 2.59807621135332M1.25
    ///     3.03108891324554L1 3.46410161513775L0.5 3.46410161513775L0.25 3.03108891324554M2 3.46410161513775L1.75
    ///     3.89711431702997M1.25 3.89711431702997L1 3.46410161513775M-3.25 1.29903810567666L-3.5 1.73205080756888L-4
    ///     1.73205080756888M-2.5 1.73205080756888L-2.75 2.1650635094611L-3.25 2.1650635094611L-3.5 1.73205080756888M-1.75
    ///     2.1650635094611L-2 2.59807621135332L-2.5 2.59807621135332L-2.75 2.1650635094611M-1 2.59807621135332L-1.25
    ///     3.03108891324554L-1.75 3.03108891324554L-2 2.59807621135332M-0.25 3.03108891324554L-0.5 3.46410161513775L-1
    ///     3.46410161513775L-1.25 3.03108891324554M0.5 3.46410161513775L0.25 3.89711431702997M-0.25 3.89711431702997L-0.5
    ///     3.46410161513775M-3.25 2.1650635094611L-3.5 2.59807621135332L-4 2.59807621135332M-2.5 2.59807621135332L-2.75
    ///     3.03108891324554L-3.25 3.03108891324554L-3.5 2.59807621135332M-1.75 3.03108891324554L-2 3.46410161513775L-2.5
    ///     3.46410161513775L-2.75 3.03108891324554M-1 3.46410161513775L-1.25 3.89711431702997M-1.75 3.89711431702997L-2
    ///     3.46410161513775M-3.25 3.03108891324554L-3.5 3.46410161513775L-4 3.46410161513775M-2.5 3.46410161513775L-2.75
    ///     3.89711431702997M-3.25 3.89711431702997L-3.5 3.46410161513775' fill='none' stroke-width='.05' stroke='black'
    ///     /&gt;&lt;/svg&gt;</image>
    public struct Hex : IEquatable<Hex>, INeighbor<Hex>, INeighbor<object>, IHasSvgGeometry, IHasDirection<Hex, Hex.Direction>
    {
        /// <summary>Returns the Q coordinate (see <see cref="Hex"/> remarks).</summary>
        public int Q { get; private set; }
        /// <summary>Returns the R coordinate (see <see cref="Hex"/> remarks).</summary>
        public int R { get; private set; }

        /// <summary>Constructor.</summary>
        public Hex(int q, int r) : this() { Q = q; R = r; }

        /// <summary>
        ///     Returns a collection of tiles that form a hexagon of the specified size and position.</summary>
        /// <param name="sideLength">
        ///     The number of hexagonal tiles that make up each side of the large hexagon. If this is <c>1</c>, a single tile
        ///     is returned. If this is <c>2</c>, there are 7 tiles. The number of tiles is given by the centered hexagonal
        ///     numbers (<c>https://en.wikipedia.org/wiki/Centered_hexagonal_number</c>).</param>
        /// <param name="center">
        ///     Specifies the center hex. Default is (0, 0).</param>
        public static IEnumerable<Hex> LargeHexagon(int sideLength, Hex center = default)
        {
            for (var r = -sideLength + 1; r < sideLength; r++)
                for (var q = -sideLength + 1; q < sideLength; q++)
                {
                    var hex = new Hex(q, r);
                    if (hex.Distance < sideLength)
                        yield return hex + center;
                }
        }

        /// <summary>
        ///     The ratio of the height of a hex tile to its width. In other words, if the width of a hex tile is 1 (from the
        ///     left-pointing vertex to the right-pointing vertex), this constant is equal to the height (from the top edge to
        ///     the bottom edge).</summary>
        /// <remarks>
        ///     The constant is equal to (√3)/2, or sin(60°).</remarks>
        public const double WidthToHeight = 0.8660254037844386;

        /// <summary>
        ///     Returns the total width of a <see cref="LargeHexagon(int, Hex)"/> structure, assuming each hex tile’s width is
        ///     <c>1</c>.</summary>
        /// <param name="sideLength">
        ///     The number of hexagonal tiles that make up each side of the large hexagon.</param>
        public static double LargeWidth(int sideLength) => (3 * sideLength - 1) * .5;

        /// <summary>
        ///     Returns the total height of a <see cref="LargeHexagon(int, Hex)"/> structure, assuming each hex tile’s width
        ///     is <c>1</c>.</summary>
        /// <param name="sideLength">
        ///     The number of hexagonal tiles that make up each side of the large hexagon.</param>
        public static double LargeHeight(int sideLength) => (2 * sideLength - 1) * WidthToHeight;

        /// <summary>
        ///     Returns a series of points that describe the outline of a <see cref="LargeHexagon(int, Hex)"/> structure.</summary>
        /// <param name="sideLength">
        ///     The number of hexagonal tiles that make up each side of the large hexagon.</param>
        /// <param name="hexWidth">
        ///     Specifies the width of each hex tile.</param>
        /// <param name="expand">
        ///     An amount by which to expand the outline outwards.</param>
        public static IEnumerable<PointD> LargeHexagonOutline(int sideLength, double hexWidth, double expand = 0)
        {
            const double tan30 = 0.5773502691896257;
            const double cos30 = 0.8660254037844387;

            sideLength--;

            // North-east
            var offset = expand * new PointD(tan30, -1);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD((i * .75 - .25) * hexWidth, (i * .5 - sideLength - .50) * hexWidth * WidthToHeight) + offset;
                yield return new PointD((i * .75 + .25) * hexWidth, (i * .5 - sideLength - .50) * hexWidth * WidthToHeight) + offset;
            }
            // East
            offset = expand * new PointD(1 / cos30, 0);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD((sideLength * .75 + .25) * hexWidth, (sideLength * .5 - sideLength + i - .50) * hexWidth * WidthToHeight) + offset;
                yield return new PointD((sideLength * .75 + .50) * hexWidth, (sideLength * .5 - sideLength + i + .00) * hexWidth * WidthToHeight) + offset;
            }
            // South-east
            offset = expand * new PointD(tan30, 1);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD(((sideLength - i) * .75 + .50) * hexWidth, ((sideLength - i) * .5 + i + .00) * hexWidth * WidthToHeight) + offset;
                yield return new PointD(((sideLength - i) * .75 + .25) * hexWidth, ((sideLength - i) * .5 + i + .50) * hexWidth * WidthToHeight) + offset;
            }
            // South-west
            offset = expand * new PointD(-tan30, 1);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD((-i * .75 + .25) * hexWidth, (-i * .5 + sideLength + .50) * hexWidth * WidthToHeight) + offset;
                yield return new PointD((-i * .75 - .25) * hexWidth, (-i * .5 + sideLength + .50) * hexWidth * WidthToHeight) + offset;
            }
            // West
            offset = expand * new PointD(-1 / cos30, 0);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD((-sideLength * .75 - .25) * hexWidth, (-sideLength * .5 + sideLength - i + .50) * hexWidth * WidthToHeight) + offset;
                yield return new PointD((-sideLength * .75 - .50) * hexWidth, (-sideLength * .5 + sideLength - i + .00) * hexWidth * WidthToHeight) + offset;
            }
            // North-west
            offset = expand * new PointD(-tan30, -1);
            for (var i = 0; i <= sideLength; i++)
            {
                if (i > 0)
                    yield return new PointD(((-sideLength + i) * .75 - .50) * hexWidth, ((-sideLength + i) * .5 - i + .00) * hexWidth * WidthToHeight) + offset;
                yield return new PointD(((-sideLength + i) * .75 - .25) * hexWidth, ((-sideLength + i) * .5 - i - .50) * hexWidth * WidthToHeight) + offset;
            }
        }

        /// <summary>
        ///     Returns a collection containing all of the current tile’s neighbors.</summary>
        /// <remarks>
        ///     The collection starts with the upper-left neighbor and proceeds clockwise.</remarks>
        public readonly IEnumerable<Hex> Neighbors
        {
            get
            {
                yield return new Hex(Q - 1, R);
                yield return new Hex(Q, R - 1);
                yield return new Hex(Q + 1, R - 1);
                yield return new Hex(Q + 1, R);
                yield return new Hex(Q, R + 1);
                yield return new Hex(Q - 1, R + 1);
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <summary>
        ///     Returns the hex tile reached by moving in the specified direction.</summary>
        /// <param name="dir">
        ///     Direction to move in.</param>
        /// <param name="amount">
        ///     Number of tiles to move. Default is <c>1</c>.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The value of <paramref name="dir"/> is not a valid HexDirection enum value.</exception>
        public readonly Hex Move(Direction dir, int amount = 1) => dir switch
        {
            Direction.UpLeft => new Hex(Q - amount, R),
            Direction.Up => new Hex(Q, R - amount),
            Direction.UpRight => new Hex(Q + amount, R - amount),
            Direction.DownRight => new Hex(Q + amount, R),
            Direction.Down => new Hex(Q, R + amount),
            Direction.DownLeft => new Hex(Q - amount, R + amount),
            _ => throw new ArgumentOutOfRangeException("dir", "Invalid HexDirection value."),
        };

        /// <summary>
        ///     Calculates the number of steps required to move from the center hex (0, 0) to the current hex.</summary>
        /// <remarks>
        ///     <para>
        ///         Note that this is not Euclidean distance. This returns the smallest number of hex tiles that must be
        ///         traversed to go from (0, 0) to the current hex.</para>
        ///     <para>
        ///         To calculate the distance between two hex tiles <c>h1</c> and <c>h2</c>, write <c>(h1 - h2).Distance</c>.</para>
        ///     <para>
        ///         To calculate actual Euclidean distance, use <see cref="Center"/> and <see cref="PointD.Distance"/>.</para></remarks>
        public readonly int Distance => Math.Max(Math.Abs(Q), Math.Max(Math.Abs(R), Math.Abs(-Q - R)));

        /// <summary>
        ///     Assuming a <see cref="LargeHexagon(int, Hex)"/> structure of side length <paramref name="sideLength"/>,
        ///     returns a collection specifying which edges of the structure the current hex tile is adjacent to.</summary>
        /// <remarks>
        ///     If the current hex tile is in a corner of the structure, two values are returned; otherwise, only one.</remarks>
        public readonly IEnumerable<Direction> GetEdges(int sideLength)
        {
            // Don’t use ‘else’ because multiple conditions could apply
            if (Q + R == -sideLength)
                yield return Direction.UpLeft;
            if (R == -sideLength)
                yield return Direction.Up;
            if (Q == sideLength)
                yield return Direction.UpRight;
            if (Q + R == sideLength)
                yield return Direction.DownRight;
            if (R == sideLength)
                yield return Direction.Down;
            if (Q == -sideLength)
                yield return Direction.DownLeft;
        }

        /// <summary>
        ///     Returns a polygon describing the shape and position of the current hex tile in 2D space.</summary>
        /// <param name="hexWidth">
        ///     Specifies the width of a single hex tile in the grid.</param>
        public readonly PointD[] GetPolygon(double hexWidth) => [
            new((Q * .75 - .50) * hexWidth, (Q * .5 + R + .00) * hexWidth * WidthToHeight),
            new((Q * .75 - .25) * hexWidth, (Q * .5 + R - .50) * hexWidth * WidthToHeight),
            new((Q * .75 + .25) * hexWidth, (Q * .5 + R - .50) * hexWidth * WidthToHeight),
            new((Q * .75 + .50) * hexWidth, (Q * .5 + R + .00) * hexWidth * WidthToHeight),
            new((Q * .75 + .25) * hexWidth, (Q * .5 + R + .50) * hexWidth * WidthToHeight),
            new((Q * .75 - .25) * hexWidth, (Q * .5 + R + .50) * hexWidth * WidthToHeight)
        ];

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <summary>Returns the vertices along the perimeter of this <see cref="Hex"/>, going clockwise from the top-left.</summary>
        public readonly Coordinates.Vertex[] Vertices => [
            new Vertex(this, false),
            new Vertex(this, true),
            new Vertex(Move(Direction.DownRight), false),
            new Vertex(Move(Direction.Down), true),
            new Vertex(Move(Direction.Down), false),
            new Vertex(Move(Direction.DownLeft), true)
        ];

        /// <summary>Returns the center of the hex tile in 2D space.</summary>
        public readonly PointD Center => new(Q * .75, (Q * .5 + R) * WidthToHeight);

        /// <inheritdoc/>
        public override readonly string ToString() => $"H({Q},{R})";

        /// <summary>Compares this hex tile to another for equality.</summary>
        public readonly bool Equals(Hex other) => Q == other.Q && R == other.R;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Hex hex && Equals(hex);
        /// <summary>Compares two <see cref="Hex"/> values for equality.</summary>
        public static bool operator ==(Hex one, Hex two) => one.Q == two.Q && one.R == two.R;
        /// <summary>Compares two <see cref="Hex"/> values for inequality.</summary>
        public static bool operator !=(Hex one, Hex two) => one.Q != two.Q || one.R != two.R;
        /// <inheritdoc/>
        public override readonly int GetHashCode() => Q * 1073741827 + R;

        /// <summary>Adds two hexes (treating them as vectors).</summary>
        public static Hex operator +(Hex one, Hex two) => new(one.Q + two.Q, one.R + two.R);
        /// <summary>Subtracts a hex from another (treating them as vectors).</summary>
        public static Hex operator -(Hex one, Hex two) => new(one.Q - two.Q, one.R - two.R);
        /// <summary>
        ///     Multiplies a hex’s coordinates by a <paramref name="multiplier"/>.</summary>
        /// <returns>
        ///     This operation is equivalent to moving from the center hex (0, 0) to the current hex, then making the same
        ///     move again, for a total of <paramref name="multiplier"/> times.</returns>
        public static Hex operator *(Hex hex, int multiplier) => new(hex.Q * multiplier, hex.R * multiplier);
        /// <summary>
        ///     Multiplies a hex’s coordinates by a <paramref name="multiplier"/>.</summary>
        /// <returns>
        ///     This operation is equivalent to moving from the center hex (0, 0) to the current hex, then making the same
        ///     move again, for a total of <paramref name="multiplier"/> times.</returns>
        public static Hex operator *(int multiplier, Hex hex) => new(hex.Q * multiplier, hex.R * multiplier);

        /// <summary>
        ///     Rotates the current hex’s position within the grid about the center hex (0, 0) clockwise by the specified
        ///     multiple of 60°.</summary>
        /// <param name="rotation">
        ///     Amount of rotation. For example, a value of <c>1</c> results in a clockwise rotation by 60°.</param>
        public readonly Hex Rotate(int rotation) => (((rotation % 6) + 6) % 6) switch
        {
            0 => this,
            1 => new Hex(-R, Q + R),
            2 => new Hex(-Q - R, Q),
            3 => new Hex(-Q, -R),
            4 => new Hex(R, -Q - R),
            5 => new Hex(Q + R, -Q),

            // This case will never occur because the calculation in the ‘switch’ expression cannot return any other values than 0–5.
            _ => throw new ArgumentException("Rotation must be between 0 and 5.", nameof(rotation)),
        };

        /// <summary>
        ///     Returns the hex tile which is equal to the current hex tile when mirrored about the X-axis (horizontal line
        ///     going through the center hex (0, 0)).</summary>
        public readonly Hex Mirrored => new(Q, -R - Q);

        /// <summary>
        ///     Returns a string representing this hex tile’s position within a <see cref="LargeHexagon(int, Hex)"/> structure
        ///     in a more human-intuitive (but mathematically unhelpful) format. The first coordinate identifies a column of
        ///     hexes, counting from 1 on the far left of the grid. The second specifies the position of the hex within that
        ///     column, counting from 1 at the top.</summary>
        /// <param name="sideLength">
        ///     The side length of the hexagonal grid.</param>
        public readonly string ConvertCoordinates(int sideLength) => Q >= 0
                ? $"({Q + sideLength}, {R + sideLength})"
                : $"({Q + sideLength}, {Q + R + sideLength})";

        /// <summary>Identifies a direction within a 2D hexagonal grid.</summary>
        public enum Direction
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

        /// <summary>Provides a collection of all hexagonal directions.</summary>
        public static readonly IEnumerable<Direction> AllDirections = (Direction[]) Enum.GetValues(typeof(Direction));

        /// <summary>Describes a 2D grid of flat-topped hexagonal cells.</summary>
        public class Grid : Structure<Hex>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Hex> cells, IEnumerable<Link<Hex>> links = null, Func<Hex, IEnumerable<Hex>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a hexagonal grid of the specified <paramref name="sideLength"/>.</summary>
            /// <param name="sideLength">
            ///     Size of the grid.</param>
            public Grid(int sideLength)
                : base(Hex.LargeHexagon(sideLength))
            {
            }

            /// <inheritdoc/>
            protected override Structure<Hex> makeModifiedStructure(IEnumerable<Hex> cells, IEnumerable<Link<Hex>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }

        /// <summary>Describes a vertex (gridline intersection) in a hexagonal grid (<see cref="Grid"/>).</summary>
        /// <remarks>
        ///     Constructor.</remarks>
        /// <param name="hex">
        ///     The hex just below this vertex.</param>
        /// <param name="right">
        ///     If <c>true</c>, identifies the top-right vertex of <paramref name="hex"/>; otherwise, the top-left.</param>
        public class Vertex(Hex hex, bool right) : Coordinates.Vertex
        {
            /// <summary>Returns the hex just below this vertex.</summary>
            public Hex Hex { get; private set; } = hex;

            /// <summary>If <c>true</c>, this vertex is the top-right vertex of <see cref="Hex"/>; otherwise, the top-left.</summary>
            public bool Right { get; private set; } = right;

            /// <inheritdoc/>
            public override PointD Point => new(Hex.Q * .75 + (Right ? .25 : -.25), (Hex.Q * .5 + Hex.R - .5) * WidthToHeight);

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex hv && hv.Hex == Hex && hv.Right == Right;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex hv && Equals(hv);
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Hex.GetHashCode() * (Right ? 1048609 : 1048601));
        }
    }
}
