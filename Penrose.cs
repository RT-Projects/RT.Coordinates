using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     Describes a cell in a Penrose tiling (P2, consisting of kites and darts; or P3, consisting of thick and thin
    ///     rhombuses).</summary>
    public struct Penrose : IEquatable<Penrose>, IHasSvgGeometry
    {
        /// <summary>
        ///     Constructor.</summary>
        /// <param name="kind">
        ///     The kind of shape.</param>
        /// <param name="corner">
        ///     The “main” corner of the shape.</param>
        /// <param name="angle">
        ///     The rotation angle of the shape.</param>
        public Penrose(Kind kind, Vector corner, int angle)
        {
            TileKind = kind;
            Corner = corner;
            Angle = (angle % 10 + 10) % 10;
        }

        /// <summary>Indicates which kind of shape this is.</summary>
        public Kind TileKind { get; private set; }
        /// <summary>Indicates the “main” corner of this shape.</summary>
        public Vector Corner { get; private set; }
        /// <summary>Indicates the rotation angle of this shape.</summary>
        public int Angle { get; private set; }

        /// <inheritdoc/>
        public bool Equals(Penrose other) => other.TileKind == TileKind && other.Corner == Corner && other.Angle == Angle;
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Penrose other && other.TileKind == TileKind && other.Corner == Corner && other.Angle == Angle;
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked((Corner.GetHashCode() * 10 + Angle) * 4 + (int) TileKind);

        /// <inheritdoc/>
        public IEnumerable<Link<Coordinates.Vertex>> Edges => Vertices.MakeEdges();

        /// <inheritdoc/>
        public PointD Center => TileKind switch
        {
            Kind.ThinRhomb => (2 * (Corner + Vector.Base(Angle)) + Vector.Base(Angle + 3).DivideByPhi).Point / 2,
            Kind.ThickRhomb => (2 * Corner + Vector.Base(Angle).MultiplyByPhi).Point / 2,
            _ => throw new InvalidOperationException($"Invalid TileKind value ‘{TileKind}’.")
        };

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Penrose"/>, going clockwise from the “main” vertex
        ///     (<see cref="Corner"/>).</summary>
        public Coordinates.Vertex[] Vertices => TileKind switch
        {
            Kind.Dart => throw new NotImplementedException(),
            Kind.Kite => throw new NotImplementedException(),
            Kind.ThickRhomb => new Coordinates.Vertex[] { Corner, Corner + Vector.Base(Angle + 9), Corner + Vector.Base(Angle + 9) + Vector.Base(Angle + 1), Corner + Vector.Base(Angle + 1) },
            Kind.ThinRhomb => new Coordinates.Vertex[] { Corner, Corner + Vector.Base(Angle), Corner + Vector.Base(Angle) + Vector.Base(Angle + 1), Corner + Vector.Base(Angle + 1) },
            _ => throw new InvalidOperationException($"Invalid TileKind value ‘{TileKind}’."),
        };

        /// <summary>
        ///     Returns a collection of tiles obtained from one iteration of deflating (expanding) this tile.</summary>
        /// <remarks>
        ///     Note that deflating several adjacent tiles will generate some duplicated tiles.</remarks>
        public IEnumerable<Penrose> DeflatedTiles
        {
            get
            {
                switch (TileKind)
                {
                    case Kind.Kite: throw new NotImplementedException();
                    case Kind.Dart: throw new NotImplementedException();

                    case Kind.ThickRhomb:
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Vector.Base(Angle)).MultiplyByPhi, Angle + 5);
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Vector.Base(Angle).MultiplyByPhi).MultiplyByPhi, Angle + 6);
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Vector.Base(Angle).MultiplyByPhi).MultiplyByPhi, Angle + 4);
                        yield return new Penrose(Kind.ThinRhomb, (Corner + Vector.Base(Angle)).MultiplyByPhi, Angle + 6);
                        yield return new Penrose(Kind.ThinRhomb, Corner.MultiplyByPhi + new Vector(1, 0, 1, 0).Rotate(Angle), Angle + 8);

                        // Extra
                        yield return new Penrose(Kind.ThickRhomb, Corner.MultiplyByPhi + new Vector(1, 0, 1, 0).Rotate(Angle), Angle + 7);
                        yield return new Penrose(Kind.ThickRhomb, Corner.MultiplyByPhi + new Vector(0, -1, 0, -1).Rotate(Angle), Angle + 3);
                        yield break;

                    case Kind.ThinRhomb:
                        var cb = (Corner + Vector.Base(Angle + 1)).MultiplyByPhi;
                        yield return new Penrose(Kind.ThickRhomb, cb, Angle + 6);
                        yield return new Penrose(Kind.ThickRhomb, cb, Angle);
                        yield return new Penrose(Kind.ThinRhomb, cb, Angle + 7);
                        yield return new Penrose(Kind.ThinRhomb, (Corner + Vector.Base(Angle)).MultiplyByPhi + Vector.Base(Angle + 9), Angle + 3);

                        // Extra
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Vector.Base(Angle)).MultiplyByPhi + Vector.Base(Angle + 7), Angle + 4);
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Vector.Base(Angle)).MultiplyByPhi + Vector.Base(Angle + 9), Angle + 2);
                        yield break;

                    default: throw new InvalidOperationException($"Invalid Penrose.TileKind value ‘{TileKind}’.");
                }
            }
        }

        /// <summary>
        ///     Returns a collection of <see cref="Link{Penrose}"/> objects describing the adjacency of the tiles generated by
        ///     <see cref="DeflatedTiles"/>.</summary>
        public IEnumerable<Link<Penrose>> DeflatedLinks
        {
            get
            {
                var arr = DeflatedTiles.ToArray();
                switch (TileKind)
                {
                    case Kind.Kite:
                    case Kind.Dart:
                        throw new NotImplementedException();

                    case Kind.ThickRhomb:
                        yield return new Link<Penrose>(arr[0], arr[3]);
                        yield return new Link<Penrose>(arr[0], arr[4]);
                        yield return new Link<Penrose>(arr[0], arr[5]);
                        yield return new Link<Penrose>(arr[0], arr[6]);
                        yield return new Link<Penrose>(arr[1], arr[2]);
                        yield return new Link<Penrose>(arr[1], arr[3]);
                        yield return new Link<Penrose>(arr[2], arr[4]);
                        yield return new Link<Penrose>(arr[3], arr[6]);
                        yield return new Link<Penrose>(arr[4], arr[5]);
                        yield break;

                    case Kind.ThinRhomb:
                        yield return new Link<Penrose>(arr[0], arr[2]);
                        yield return new Link<Penrose>(arr[0], arr[4]);
                        yield return new Link<Penrose>(arr[1], arr[3]);
                        yield return new Link<Penrose>(arr[1], arr[5]);
                        yield return new Link<Penrose>(arr[2], arr[3]);
                        yield return new Link<Penrose>(arr[2], arr[4]);
                        yield return new Link<Penrose>(arr[3], arr[5]);
                        yield break;

                    default:
                        throw new InvalidOperationException($"Invalid Penrose.TileKind value ‘{TileKind}’.");
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Corner}/{(int) TileKind}/{Angle}";

        /// <summary>
        ///     Describes a vertex in a Penrose-tiled grid using a system of four base vectors, which are unit vectors 36°
        ///     from one another.</summary>
        public struct Vector : IEquatable<Vector>
        {
            /// <summary>Constructor.</summary>
            public Vector(int a, int b, int c, int d)
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }

            /// <summary>
            ///     The A component of this vector, which extends outward from the origin at a 54° angle counter-clockwise
            ///     (SVG) or clockwise (geometry) from the x-axis.</summary>
            public int A { get; private set; }
            /// <summary>
            ///     The B component of this vector, which extends outward from the origin at a 18° angle counter-clockwise
            ///     (SVG) or clockwise (geometry) from the x-axis.</summary>
            public int B { get; private set; }
            /// <summary>
            ///     The C component of this vector, which extends outward from the origin at a 18° angle clockwise (SVG) or
            ///     counter-clockwise (geometry) from the x-axis.</summary>
            public int C { get; private set; }
            /// <summary>
            ///     The D component of this vector, which extends outward from the origin at a 54° angle clockwise (SVG) or
            ///     counter-clockwise (geometry) from the x-axis.</summary>
            public int D { get; private set; }

            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector at a 54° angle counter-clockwise (SVG) or
            ///     clockwise (geometry) from the x-axis.</summary>
            public static readonly Vector BaseA = new Vector(1, 0, 0, 0);
            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector at a 18° angle counter-clockwise (SVG) or
            ///     clockwise (geometry) from the x-axis.</summary>
            public static readonly Vector BaseB = new Vector(0, 1, 0, 0);
            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector at a 18° angle clockwise (SVG) or
            ///     counter-clockwise (geometry) from the x-axis.</summary>
            public static readonly Vector BaseC = new Vector(0, 0, 1, 0);
            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector at a 54° angle clockwise (SVG) or
            ///     counter-clockwise (geometry) from the x-axis.</summary>
            public static readonly Vector BaseD = new Vector(0, 0, 0, 1);
            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector pointing straight down (SVG) or up (geometry).</summary>
            public static readonly Vector BaseE = new Vector(-1, 1, -1, 1);

            /// <summary>
            ///     Returns a <see cref="Vector"/> describing a unit vector at the specified <paramref name="angle"/>.</summary>
            /// <param name="angle">
            ///     The angle, specified as a multiple of 36° going clockwise (SVG) or counter-clockwise (geometry) from
            ///     vertically straight up (SVG) or down (geometry).</param>
            /// <remarks>
            ///     Note that if <paramref name="angle"/> is <c>0</c>, the vector returned is the negative of <see
            ///     cref="BaseE"/>.</remarks>
            public static Vector Base(int angle) => ((angle % 10 + 10) % 10) switch
            {
                0 => -BaseE,
                1 => BaseA,
                2 => BaseB,
                3 => BaseC,
                4 => BaseD,
                5 => BaseE,
                6 => -BaseA,
                7 => -BaseB,
                8 => -BaseC,
                9 => -BaseD,
                _ => throw new InvalidOperationException($"Invalid angle value ‘{angle}’.")
            };

            /// <summary>
            ///     Rotates the vector the specified <paramref name="clockwiseAmount"/> times 36°.</summary>
            /// <returns>
            ///     The rotation proceeds clockwise in SVG and counter-clockwise in geometry.</returns>
            public Vector Rotate(int clockwiseAmount = 1)
            {
                clockwiseAmount = (clockwiseAmount % 10 + 10) % 10;
                var v = this;
                while (clockwiseAmount-- > 0)
                    v = new Vector(-v.D, v.D + v.A, -v.D + v.B, v.D + v.C);
                return v;
            }

            /// <summary>
            ///     Returns a new <see cref="Vector"/> whose direction is the same as this vector, but with a length
            ///     multiplied by 1/φ. φ is the golden ratio ((√(5)+1)/2).</summary>
            public Vector DivideByPhi => new Vector(B - D, C + D - B, A + B - C, C - A);
            /// <summary>
            ///     Returns a new <see cref="Vector"/> whose direction is the same as this vector, but with a length
            ///     multiplied by φ. φ is the golden ratio ((√(5)+1)/2).</summary>
            public Vector MultiplyByPhi => new Vector(A + B - D, C + D, A + B, C + D - A);

            /// <inheritdoc/>
            public bool Equals(Vector other) => other.A == A && other.B == B && other.C == C && other.D == D;
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vector other && other.A == A && other.B == B && other.C == C && other.D == D;
            /// <inheritdoc/>
            public override int GetHashCode() => unchecked(148139063 * A + 336220337 * B + 672753941 * C + 797808397 * D);

            /// <summary>Addition operator.</summary>
            public static Vector operator +(Vector one, Vector two) => new Vector(one.A + two.A, one.B + two.B, one.C + two.C, one.D + two.D);
            /// <summary>Subtraction operator.</summary>
            public static Vector operator -(Vector one, Vector two) => new Vector(one.A - two.A, one.B - two.B, one.C - two.C, one.D - two.D);
            /// <summary>Multiplication operator.</summary>
            public static Vector operator *(Vector one, int two) => new Vector(one.A * two, one.B * two, one.C * two, one.D * two);
            /// <summary>Multiplication operator.</summary>
            public static Vector operator *(int one, Vector two) => new Vector(two.A * one, two.B * one, two.C * one, two.D * one);
            /// <summary>Unary negation operator.</summary>
            public static Vector operator -(Vector op) => new Vector(-op.A, -op.B, -op.C, -op.D);

            /// <summary>Equality comparison.</summary>
            public static bool operator ==(Vector one, Vector two) => one.Equals(two);
            /// <summary>Inequality comparison.</summary>
            public static bool operator !=(Vector one, Vector two) => !one.Equals(two);

            private const double sin18 = .30901699437494742410;
            private const double cos18 = .95105651629515357210;
            private const double sin54 = .80901699437494742410;
            private const double cos54 = .58778525229247312918;

            /// <summary>Returns the X-coordinate of the 2D point represented by this vector.</summary>
            public double X => cos54 * (A + D) + cos18 * (B + C);
            /// <summary>Returns the Y-coordinate of the 2D point represented by this vector.</summary>
            public double Y => sin54 * (D - A) + sin18 * (C - B);
            /// <summary>Returns the 2D point represented by this vector.</summary>
            public PointD Point => new PointD(X, Y);

            /// <summary>Implicit operator converting <see cref="Vector"/> to <see cref="Vertex"/>.</summary>
            public static implicit operator Coordinates.Vertex(Vector v) => new Vertex(v);
            /// <inheritdoc/>
            public override string ToString() => $"({A},{B},{C},{D})";
        }

        /// <summary>Describes the kind of shape of a <see cref="Penrose"/> tile.</summary>
        public enum Kind
        {
            /// <summary>A kite shape, which is part of the P2 tiling.</summary>
            Kite,
            /// <summary>A dart shape, which is part of the P2 tiling.</summary>
            Dart,
            /// <summary>
            ///     A thick rhombus shape, which is part of the P3 tiling. The side lengths are 1 and the internal angles are
            ///     72° and 108°.</summary>
            ThickRhomb,
            /// <summary>
            ///     A thin rhombus shape, which is part of the P3 tiling. The side lengths are 1 and the internal angles are
            ///     36° and 144°.</summary>
            ThinRhomb
        }

        /// <summary>
        ///     Describes a vertex in a <see cref="Penrose"/> grid.</summary>
        /// <remarks>
        ///     This is merely a thin wrapper around <see cref="Vector"/> but for the purpose of implementing <see
        ///     cref="Coordinates.Vertex"/>.</remarks>
        public class Vertex : Coordinates.Vertex
        {
            /// <summary>The underlying <see cref="Vector"/>.</summary>
            public Vector Vector { get; private set; }
            /// <summary>Constructor.</summary>
            public Vertex(Vector vector) { Vector = vector; }

            /// <inheritdoc/>
            public override bool Equals(Coordinates.Vertex other) => other is Vertex v && v.Vector.Equals(Vector);
            /// <inheritdoc/>
            public override bool Equals(object obj) => obj is Vertex v && v.Vector.Equals(Vector);
            /// <inheritdoc/>
            public override int GetHashCode() => Vector.GetHashCode();
            /// <inheritdoc/>
            public override double X => Vector.X;
            /// <inheritdoc/>
            public override double Y => Vector.Y;
        }

        /// <summary>Represents a grid of <see cref="Penrose"/> cells. The grid conforms to Penrose tiling P2 or P3.</summary>
        public class Grid : Structure<Penrose>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<Penrose> cells, IEnumerable<Link<Penrose>> links = null, Func<Penrose, IEnumerable<Penrose>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            // Dirty trick so that the constructor needs to call GenerateGrid() only once but pass both pieces of information to the base constructor
            [ThreadStatic]
            private static GridInfo tempGridInfo;
            /// <summary>
            ///     Constructs a grid consisting of <see cref="Penrose"/> tiles obtained by deflating (subdividing) a star
            ///     consisting of five <see cref="Kind.ThickRhomb"/> shapes.</summary>
            /// <param name="numIterations">
            ///     Number of deflation iterations to perform.</param>
            public Grid(int numIterations) : base((tempGridInfo = GenerateGrid(numIterations)).Cells, tempGridInfo.Links)
            {
            }

            /// <inheritdoc/>
            protected override Structure<Penrose> makeModifiedStructure(IEnumerable<Penrose> cells, IEnumerable<Link<Penrose>> traversible) => new Grid(cells, traversible);

            /// <summary>
            ///     Generates a maze on this structure.</summary>
            /// <param name="rnd">
            ///     A random number generator.</param>
            /// <exception cref="InvalidOperationException">
            ///     The current structure is disjointed (consists of more than one piece).</exception>
            public new Grid GenerateMaze(Random rnd = null) => (Grid) base.GenerateMaze(rnd);

            /// <summary>
            ///     Generates a maze on this structure.</summary>
            /// <param name="rndNext">
            ///     A delegate that can provide random numbers.</param>
            /// <exception cref="InvalidOperationException">
            ///     The current structure is disjointed (consists of more than one piece).</exception>
            public new Grid GenerateMaze(Func<int, int, int> rndNext) => (Grid) base.GenerateMaze(rndNext);

            private static GridInfo GenerateGrid(int numIterations)
            {
                var tiles = Enumerable.Range(0, 5).Select(angle => new Penrose(Kind.ThickRhomb, default, 2 * angle));

                IEnumerable<Penrose> prev = null;
                for (var i = 0; i < numIterations; i++)
                {
                    prev = tiles;
                    tiles = tiles.SelectMany(t => t.DeflatedTiles).Distinct();
                }

                return new GridInfo { Cells = tiles, Links = prev.SelectMany(t => t.DeflatedLinks) };
            }

            struct GridInfo
            {
                public IEnumerable<Penrose> Cells;
                public IEnumerable<Link<Penrose>> Links;
            }
        }
    }
}
