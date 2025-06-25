using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Represents a cell in a circular grid. Any number of cells can make a full circle, and there can be any number of
    ///     circles of varying radius.</summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.5 -3.5 7 7'&gt;&lt;path d='M0 0L6.12303176911189E-17 -1A 1
    ///     1 0 0 1 0.38268343236509 -0.923879532511287A 1 1 0 0 1 0.923879532511287 -0.38268343236509A 1 1 0 0 1 1
    ///     0zM6.12303176911189E-17 1A 1 1 0 0 1 -0.38268343236509 0.923879532511287A 1 1 0 0 1 -0.923879532511287
    ///     0.38268343236509A 1 1 0 0 1 -1 1.22460635382238E-16L0 0L6.12303176911189E-17 1A 1 1 0 0 0 0.38268343236509
    ///     0.923879532511287A 1 1 0 0 0 0.923879532511287 0.38268343236509A 1 1 0 0 0 1 0M-1 1.22460635382238E-16A 1 1 0 0 1
    ///     -0.923879532511287 -0.38268343236509A 1 1 0 0 1 -0.38268343236509 -0.923879532511287A 1 1 0 0 1
    ///     6.12303176911189E-17 -1M0.38268343236509 -0.923879532511287L0.76536686473018 -1.84775906502257A 2 2 0 0 1 1
    ///     -1.73205080756888A 2 2 0 0 1 1.73205080756888 -1A 2 2 0 0 1 1.84775906502257 -0.76536686473018L0.923879532511287
    ///     -0.38268343236509M1.84775906502257 -0.76536686473018A 2 2 0 0 1 2 0A 2 2 0 0 1 1.84775906502257
    ///     0.76536686473018L0.923879532511287 0.38268343236509M1.84775906502257 0.76536686473018A 2 2 0 0 1 1.73205080756888
    ///     1A 2 2 0 0 1 1 1.73205080756888A 2 2 0 0 1 0.76536686473018 1.84775906502257L0.38268343236509
    ///     0.923879532511287M0.76536686473018 1.84775906502257A 2 2 0 0 1 1.22460635382238E-16 2A 2 2 0 0 1
    ///     -0.765366864730179 1.84775906502257L-0.38268343236509 0.923879532511287M-0.765366864730179 1.84775906502257A 2 2 0
    ///     0 1 -1 1.73205080756888A 2 2 0 0 1 -1.73205080756888 1A 2 2 0 0 1 -1.84775906502257
    ///     0.76536686473018L-0.923879532511287 0.38268343236509M-1.84775906502257 0.76536686473018A 2 2 0 0 1 -2
    ///     2.44921270764475E-16A 2 2 0 0 1 -1.84775906502257 -0.765366864730179L-0.923879532511287
    ///     -0.38268343236509M-1.84775906502257 -0.765366864730179A 2 2 0 0 1 -1.73205080756888 -1A 2 2 0 0 1 -1
    ///     -1.73205080756888A 2 2 0 0 1 -0.765366864730181 -1.84775906502257L-0.38268343236509
    ///     -0.923879532511287M-0.765366864730181 -1.84775906502257A 2 2 0 0 1 1.22460635382238E-16 -2A 2 2 0 0 1
    ///     0.76536686473018 -1.84775906502257M1.22460635382238E-16 -2L1.83690953073357E-16 -3A 3 3 0 0 1 0.585270966048385
    ///     -2.94235584120969A 3 3 0 0 1 1.5 -2.59807621135332L1 -1.73205080756888M1.5 -2.59807621135332A 3 3 0 0 1
    ///     1.66671069905881 -2.49440883690764A 3 3 0 0 1 2.49440883690764 -1.66671069905881A 3 3 0 0 1 2.59807621135332
    ///     -1.5L1.73205080756888 -1M2.59807621135332 -1.5A 3 3 0 0 1 2.94235584120969 -0.585270966048385A 3 3 0 0 1 3 0L2 0M3
    ///     0A 3 3 0 0 1 2.94235584120969 0.585270966048385A 3 3 0 0 1 2.59807621135332 1.5L1.73205080756888
    ///     1M2.59807621135332 1.5A 3 3 0 0 1 2.49440883690764 1.66671069905881A 3 3 0 0 1 1.66671069905881 2.49440883690764A
    ///     3 3 0 0 1 1.5 2.59807621135332L1 1.73205080756888M1.5 2.59807621135332A 3 3 0 0 1 0.585270966048385
    ///     2.94235584120969A 3 3 0 0 1 1.83690953073357E-16 3L1.22460635382238E-16 2M1.83690953073357E-16 3A 3 3 0 0 1
    ///     -0.585270966048385 2.94235584120969A 3 3 0 0 1 -1.5 2.59807621135332L-1 1.73205080756888M-1.5 2.59807621135332A 3
    ///     3 0 0 1 -1.66671069905881 2.49440883690764A 3 3 0 0 1 -2.49440883690764 1.66671069905881A 3 3 0 0 1
    ///     -2.59807621135332 1.5L-1.73205080756888 1M-2.59807621135332 1.5A 3 3 0 0 1 -2.94235584120969 0.585270966048386A 3
    ///     3 0 0 1 -3 3.67381906146713E-16L-2 2.44921270764475E-16M-3 3.67381906146713E-16A 3 3 0 0 1 -2.94235584120969
    ///     -0.585270966048385A 3 3 0 0 1 -2.59807621135332 -1.5L-1.73205080756888 -1M-2.59807621135332 -1.5A 3 3 0 0 1
    ///     -2.49440883690764 -1.66671069905881A 3 3 0 0 1 -1.66671069905881 -2.49440883690764A 3 3 0 0 1 -1.5
    ///     -2.59807621135332L-1 -1.73205080756888M-1.5 -2.59807621135332A 3 3 0 0 1 -0.585270966048386 -2.94235584120969A 3 3
    ///     0 0 1 1.83690953073357E-16 -3M0.585270966048385 -2.94235584120969L0.780361288064513
    ///     -3.92314112161292M1.03527618041008 -3.86370330515627A 4 4 0 0 1 2 -3.46410161513775A 4 4 0 0 1 2.22228093207841
    ///     -3.32587844921018L1.66671069905881 -2.49440883690764M2.22228093207841 -3.32587844921018A 4 4 0 0 1
    ///     2.82842712474619 -2.82842712474619A 4 4 0 0 1 3.32587844921018 -2.22228093207841L2.49440883690764
    ///     -1.66671069905881M3.32587844921018 -2.22228093207841A 4 4 0 0 1 3.46410161513775 -2A 4 4 0 0 1 3.86370330515627
    ///     -1.03527618041008M3.92314112161292 -0.780361288064513L2.94235584120969 -0.585270966048385M3.92314112161292
    ///     0.780361288064513L2.94235584120969 0.585270966048385M3.86370330515627 1.03527618041008A 4 4 0 0 1 3.46410161513775
    ///     2A 4 4 0 0 1 3.32587844921018 2.22228093207841L2.49440883690764 1.66671069905881M3.32587844921018
    ///     2.22228093207841A 4 4 0 0 1 2.82842712474619 2.82842712474619A 4 4 0 0 1 2.22228093207841
    ///     3.32587844921018L1.66671069905881 2.49440883690764M2.22228093207841 3.32587844921018A 4 4 0 0 1 2
    ///     3.46410161513775A 4 4 0 0 1 1.03527618041008 3.86370330515627M0.780361288064513 3.92314112161292L0.585270966048385
    ///     2.94235584120969M-0.780361288064513 3.92314112161292L-0.585270966048385 2.94235584120969M-1.03527618041008
    ///     3.86370330515627A 4 4 0 0 1 -2 3.46410161513775A 4 4 0 0 1 -2.22228093207841 3.32587844921018L-1.66671069905881
    ///     2.49440883690764M-2.22228093207841 3.32587844921018A 4 4 0 0 1 -2.82842712474619 2.82842712474619A 4 4 0 0 1
    ///     -3.32587844921018 2.22228093207841L-2.49440883690764 1.66671069905881M-3.32587844921018 2.22228093207841A 4 4 0 0
    ///     1 -3.46410161513775 2A 4 4 0 0 1 -3.86370330515627 1.03527618041008M-3.92314112161292
    ///     0.780361288064514L-2.94235584120969 0.585270966048386M-3.92314112161292 -0.780361288064513L-2.94235584120969
    ///     -0.585270966048385M-3.86370330515627 -1.03527618041008A 4 4 0 0 1 -3.46410161513775 -2A 4 4 0 0 1
    ///     -3.32587844921018 -2.22228093207841L-2.49440883690764 -1.66671069905881M-3.32587844921018 -2.22228093207841A 4 4 0
    ///     0 1 -2.82842712474619 -2.82842712474619A 4 4 0 0 1 -2.22228093207841 -3.32587844921018L-1.66671069905881
    ///     -2.49440883690764M-2.22228093207841 -3.32587844921018A 4 4 0 0 1 -2 -3.46410161513775A 4 4 0 0 1 -1.03527618041008
    ///     -3.86370330515627M-0.780361288064515 -3.92314112161292L-0.585270966048386 -2.94235584120969M2.5
    ///     -4.33012701892219L2 -3.46410161513775M3.53553390593274 -3.53553390593274L2.82842712474619
    ///     -2.82842712474619M4.33012701892219 -2.5L3.46410161513775 -2M4.33012701892219 2.5L3.46410161513775
    ///     2M3.53553390593274 3.53553390593274L2.82842712474619 2.82842712474619M2.5 4.33012701892219L2 3.46410161513775M-2.5
    ///     4.33012701892219L-2 3.46410161513775M-3.53553390593274 3.53553390593274L-2.82842712474619
    ///     2.82842712474619M-4.33012701892219 2.5L-3.46410161513775 2M-4.33012701892219 -2.5L-3.46410161513775
    ///     -2M-3.53553390593274 -3.53553390593274L-2.82842712474619 -2.82842712474619M-2.5 -4.33012701892219L-2
    ///     -3.46410161513775' fill='none' stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    public class CircularCell : IEquatable<CircularCell>
    {
        /// <summary>Constructor.</summary>
        public CircularCell(int radius, CircleFraction start, CircleFraction end)
        {
            if (start == end)
                throw new ArgumentException("‘start’ and ‘end’ can’t be equal.", nameof(end));
            Radius = radius;
            Start = start;
            End = end;
        }

        /// <summary>
        ///     Specifies the inner radius of this cell. The outer radius is this plus one.</summary>
        /// <remarks>
        ///     If this is <c>0</c> (zero), cells at this radius share a vertex at the center of the circle.</remarks>
        public int Radius { get; private set; }
        /// <summary>Where along the circular perimeter this cell begins.</summary>
        public CircleFraction Start { get; private set; }
        /// <summary>Where along the circular perimeter this cell ends.</summary>
        public CircleFraction End { get; private set; }

        /// <inheritdoc/>
        public override string ToString() => $"C({Radius};{Start}→{End})";
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CircularCell other && other.Radius == Radius && other.Start == Start && other.End == End;
        /// <inheritdoc/>
        public bool Equals(CircularCell other) => other.Radius == Radius && other.Start == Start && other.End == End;
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(Radius * 235794631 + Start.GetHashCode() * 484361 + End.GetHashCode());

        /// <summary>
        ///     Given a collection of <see cref="CircularCell"/> objects, returns the subset of those that would be neighbors
        ///     of the current cell.</summary>
        public IEnumerable<CircularCell> FindNeighbors(IEnumerable<CircularCell> cells) => cells.Where(cell =>
            // Cell clockwise from this
            (cell.Radius == Radius && cell.Start == End) ||
            // Cell counter-clockwise from this
            (cell.Radius == Radius && cell.End == Start) ||

            // Cells on a neighboring ring
            ((cell.Radius == Radius + 1 || cell.Radius + 1 == Radius) &&
                (cell.Start.IsStrictlyBetween(Start, End) || cell.End.IsStrictlyBetween(Start, End) || Start.IsStrictlyBetween(cell.Start, cell.End) || End.IsStrictlyBetween(cell.Start, cell.End) || (cell.Start == Start && cell.End == End)))
        );

        /// <summary>
        ///     Given a collection of <see cref="CircularCell"/> objects, returns a collection of edges that describes the
        ///     perimeter of the current cell.</summary>
        public IEnumerable<Edge> FindEdges(IEnumerable<CircularCell> cells)
        {
            var vertices = new List<Coordinates.Vertex> { new Vertex(Radius, Start), new Vertex(Radius + 1, Start) };

            var outerNotches = new HashSet<CircleFraction>();
            var innerNotches = new HashSet<CircleFraction>();
            foreach (var neighbor in FindNeighbors(cells))
            {
                if ((neighbor.Radius == Radius + 1 || neighbor.Radius + 1 == Radius) && neighbor.Start.IsStrictlyBetween(Start, End))
                    (neighbor.Radius == Radius + 1 ? outerNotches : innerNotches).Add(neighbor.Start);
                if ((neighbor.Radius == Radius + 1 || neighbor.Radius + 1 == Radius) && neighbor.End.IsStrictlyBetween(Start, End))
                    (neighbor.Radius == Radius + 1 ? outerNotches : innerNotches).Add(neighbor.End);
            }
            foreach (var notch in outerNotches.OrderBy(cf => cf))
                vertices.Add(new Vertex(Radius + 1, notch));
            vertices.Add(new Vertex(Radius + 1, End));
            if (Radius > 0)
            {
                vertices.Add(new Vertex(Radius, End));
                foreach (var notch in innerNotches.OrderByDescending(cf => cf))
                    vertices.Add(new Vertex(Radius, notch));
            }
            return vertices.MakeEdges();
        }

        /// <summary>Returns the center point of this cell.</summary>
        public PointD Center => new PointD(0, -Radius - .5).Rotate(Math.PI * (Start + (Start < End ? 0 : 1) + End));

        /// <summary>Describes a vertex in a <see cref="CircularCell"/> grid.</summary>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>Constructor.</summary>
            public Vertex(int radius, CircleFraction pos)
            {
                Radius = radius;
                Position = radius == 0 ? new CircleFraction(0, 1) : pos;
            }

            private static int gcd(int a, int b)
            {
                while (a != 0 && b != 0)
                {
                    if (a > b)
                        a %= b;
                    else
                        b %= a;
                }

                return a | b;
            }

            /// <summary>The distance of this vertex from the origin.</summary>
            public int Radius { get; private set; }
            /// <summary>
            ///     The angle of this vertex from the vertical, expressed as a fraction of the full circle.</summary>
            /// <remarks>
            ///     In SVG, the angle is clockwise. In geometry, the angle is counter-clockwise.</remarks>
            public CircleFraction Position { get; private set; }

            /// <inheritdoc/>
            public override PointD Point => new(
                Radius * Math.Cos(Math.PI * (2d * Position - .5)),
                Radius * Math.Sin(Math.PI * (2d * Position - .5)));

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex vx && vx.Radius == Radius && vx.Position == Position;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex vx && vx.Radius == Radius && vx.Position == Position;
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(Radius * 235794653 + Position.GetHashCode());

            /// <inheritdoc/>
            public override string ToString() => $"{Radius};{Position}";

            /// <inheritdoc/>
            public override string SvgPathFragment(Coordinates.Vertex from, Func<Coordinates.Vertex, PointD> getVertexPoint, Func<double, string> r, bool isLast)
            {
                if (from is not Vertex v || v.Radius != Radius)
                    return base.SvgPathFragment(from, getVertexPoint, r, isLast);

                var gc = gcd(Position.Denominator, v.Position.Denominator);
                var n1 = Position.Numerator * v.Position.Denominator / gc;
                var n2 = v.Position.Numerator * Position.Denominator / gc;
                var cd = Position.Denominator * v.Position.Denominator / gc;

                var p = getVertexPoint(this);
                return $"A {Radius} {Radius} 0 0 {((n2 > n1 ? (2 * (n2 - n1) > cd) : (2 * (n1 - n2) < cd)) ? "1" : "0")} {r(p.X)} {r(p.Y)}";
            }
        }

        /// <summary>
        ///     Describes a grid of circular cells. Any number of cells can make a full circle, and there can be any number of
        ///     circles of varying radius.</summary>
        public class Grid : Structure<CircularCell>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<CircularCell> cells, IEnumerable<Link<CircularCell>> links = null, Func<CircularCell, IEnumerable<CircularCell>> getNeighbors = null) : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a circular grid by dividing each ring into a specified number of equal-sized cells.</summary>
            /// <param name="divisionsPerRadius">
            ///     Defines the number of subdivisions at each radius.</param>
            /// <param name="offsets">
            ///     Optionally offsets the cells at each radius by a specified amount.</param>
            public Grid(int[] divisionsPerRadius, CircleFraction[] offsets = null)
                : base(MakeGrid(divisionsPerRadius, offsets, out var getNeighbors), getNeighbors: getNeighbors)
            {
            }

            private static IEnumerable<CircularCell> MakeGrid(int[] divisionsPerRadius, CircleFraction[] offsets, out Func<CircularCell, IEnumerable<CircularCell>> getNeighbors)
            {
                if (divisionsPerRadius == null)
                    throw new ArgumentNullException(nameof(divisionsPerRadius));
                if (divisionsPerRadius.Length == 0)
                    throw new ArgumentException($"‘{nameof(divisionsPerRadius)}’ cannot be an empty array.", nameof(divisionsPerRadius));
                if (divisionsPerRadius.Any(v => v <= 0))
                    throw new ArgumentException($"‘{nameof(divisionsPerRadius)}’ must not contain any non-positive values.", nameof(divisionsPerRadius));
                if (offsets != null && offsets.Length != divisionsPerRadius.Length)
                    throw new ArgumentException($"‘{nameof(offsets)}’, if specified, must have the same length as ‘{nameof(divisionsPerRadius)}’.", nameof(offsets));

                var cells = new List<CircularCell>();
                for (var r = 0; r < divisionsPerRadius.Length; r++)
                {
                    var ofs = offsets == null ? CircleFraction.Zero : offsets[r];
                    for (var i = 0; i < divisionsPerRadius[r]; i++)
                        cells.Add(new CircularCell(r, new CircleFraction(i, divisionsPerRadius[r]) + ofs, new CircleFraction(i + 1, divisionsPerRadius[r]) + ofs));
                }
                getNeighbors = c => c.FindNeighbors(cells);
                return cells;
            }

            /// <summary>
            ///     Constructs a circular grid by dividing each ring into a number of cells of specified relative sizes.</summary>
            /// <param name="sizesPerRadius">
            ///     Defines the sizes of the cells at each radius relative to one another.</param>
            /// <param name="offsets">
            ///     Optionally offsets the cells at each radius by a specified amount.</param>
            public Grid(int[][] sizesPerRadius, CircleFraction[] offsets = null)
                : base(MakeGrid(sizesPerRadius, offsets, out var getNeighbors), getNeighbors: getNeighbors)
            {
            }

            private static IEnumerable<CircularCell> MakeGrid(int[][] sizesPerRadius, CircleFraction[] offsets, out Func<CircularCell, IEnumerable<CircularCell>> getNeighbors)
            {
                if (sizesPerRadius == null)
                    throw new ArgumentNullException(nameof(sizesPerRadius));
                if (sizesPerRadius.Length == 0)
                    throw new ArgumentException($"‘{nameof(sizesPerRadius)}’ cannot be an empty array.", nameof(sizesPerRadius));
                if (sizesPerRadius.Any(arr => arr == null || arr.Length == 0))
                    throw new ArgumentException($"‘{nameof(sizesPerRadius)}’ must not contain any null or empty arrays.", nameof(sizesPerRadius));
                if (sizesPerRadius.Any(arr => arr.Any(v => v <= 0)))
                    throw new ArgumentException($"‘{nameof(sizesPerRadius)}’ must not contain any non-positive values.", nameof(sizesPerRadius));
                if (offsets != null && offsets.Length != sizesPerRadius.Length)
                    throw new ArgumentException($"‘{nameof(offsets)}’, if specified, must have the same length as ‘{nameof(sizesPerRadius)}’.", nameof(offsets));

                var cells = new List<CircularCell>();
                for (var r = 0; r < sizesPerRadius.Length; r++)
                {
                    var denominator = sizesPerRadius[r].Sum();
                    var ofs = offsets == null ? CircleFraction.Zero : offsets[r];
                    for (int i = 0, fr = 0; i < sizesPerRadius[r].Length; i++)
                        cells.Add(new CircularCell(r, new CircleFraction(fr, denominator) + ofs, new CircleFraction(fr += sizesPerRadius[r][i], denominator) + ofs));
                }
                getNeighbors = c => c.FindNeighbors(cells);
                return cells;
            }
        }
    }
}
