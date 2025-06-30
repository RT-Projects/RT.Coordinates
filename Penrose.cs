using System;
using System.Collections.Generic;
using System.Linq;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Describes a cell in a Penrose tiling (P2, consisting of kites and darts; or P3, consisting of thick and thin
    ///         rhombuses).</para></summary>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.5 -3.5 7 7'&gt;&lt;path d='M3.07768353717525
    ///     -4.23606797749979L2.48989828488278 -3.42705098312484L2.48989828488278 -4.42705098312484M0.951056516295154
    ///     -2.92705098312484L0 -2.61803398874989L0.587785252292473 -3.42705098312484M-0.951056516295154 -2.92705098312484L0
    ///     -2.61803398874989L-0.587785252292473 -3.42705098312484M4.02874005347041 -2.92705098312484L3.07768353717525
    ///     -2.61803398874989L2.48989828488278 -3.42705098312484L3.44095480117793 -3.73606797749979M-2.48989828488278
    ///     -4.42705098312484L-2.48989828488278 -3.42705098312484L-3.07768353717525 -4.23606797749979M-1.53884176858763
    ///     -3.73606797749979L-0.951056516295154 -2.92705098312484L-1.90211303259031 -2.61803398874989L-2.48989828488278
    ///     -3.42705098312484L-1.53884176858763 -3.73606797749979L-0.587785252292473 -3.42705098312484L0
    ///     -4.23606797749979L0.587785252292473 -3.42705098312484L1.53884176858763 -3.73606797749979L2.48989828488278
    ///     -3.42705098312484L1.90211303259031 -2.61803398874989L0.951056516295154 -2.92705098312484L1.53884176858763
    ///     -3.73606797749979M-3.44095480117793 -3.73606797749979L-2.48989828488278 -3.42705098312484L-3.07768353717525
    ///     -2.61803398874989L-4.02874005347041 -2.92705098312484M1.90211303259031 -2.61803398874989L1.90211303259031
    ///     -1.61803398874989L0.951056516295154 -1.30901699437495L0.951056516295154 -2.30901699437495zM3.07768353717525
    ///     -1.61803398874989L3.07768353717525 -2.61803398874989L2.48989828488278 -1.80901699437495L1.90211303259031
    ///     -2.61803398874989M2.48989828488278 -1.80901699437495L2.48989828488278 -0.809016994374947L1.90211303259031
    ///     -1.61803398874989M1.53884176858763 -0.5L1.53884176858763 0.5L2.48989828488278 0.190983005625053L2.48989828488278
    ///     -0.809016994374947L1.53884176858763 -0.5L0.951056516295154 -1.30901699437495L0 -1.61803398874989L0
    ///     -2.61803398874989L0.951056516295154 -2.30901699437495M1.53884176858763 -0.5L0.951056516295154 0.309016994374947L0
    ///     0L0.587785252292473 -0.809016994374947zM0.951056516295154 1.30901699437495L0 1.61803398874989L0.587785252292473
    ///     2.42705098312484L1.53884176858763 2.11803398874989L0.951056516295154 1.30901699437495L0 1L0 0L-0.587785252292473
    ///     -0.809016994374947L0 -1.61803398874989L0.587785252292473 -0.809016994374947M0 -1.61803398874989L-0.951056516295154
    ///     -1.30901699437495L-0.951056516295154 -2.30901699437495L0 -2.61803398874989M-0.587785252292473
    ///     -0.809016994374947L-1.53884176858763 -0.5L-0.951056516295154 -1.30901699437495L-1.90211303259031
    ///     -1.61803398874989L-1.90211303259031 -2.61803398874989L-0.951056516295154 -2.30901699437495M2.48989828488278
    ///     -0.809016994374947L3.07768353717525 -1.61803398874989L4.02874005347041 -1.30901699437495L3.44095480117793
    ///     -0.5zM3.07768353717525 -2.61803398874989L4.02874005347041 -2.30901699437495M3.07768353717525 1L2.48989828488278
    ///     1.80901699437495L3.44095480117793 2.11803398874989L4.02874005347041 1.30901699437495L3.07768353717525
    ///     1L3.07768353717525 0L4.02874005347041 0.309016994374947L3.44095480117793 -0.5M3.07768353717525 0L2.48989828488278
    ///     -0.809016994374947M2.48989828488278 0.190983005625053L3.07768353717525 1L2.1266270208801
    ///     1.30901699437495L1.53884176858763 0.5L0.951056516295154 1.30901699437495L0.951056516295154
    ///     0.309016994374947M2.48989828488278 1.80901699437495L1.53884176858763 2.11803398874989L2.1266270208801
    ///     1.30901699437495M0 1L-0.951056516295154 1.30901699437495L0 1.61803398874989L-0.587785252292473
    ///     2.42705098312484L-1.53884176858763 2.11803398874989L-0.951056516295154 1.30901699437495M-0.951056516295154
    ///     0.309016994374947L-0.951056516295154 1.30901699437495L-1.53884176858763 0.5L-2.1266270208801
    ///     1.30901699437495L-3.07768353717525 1L-2.48989828488278 0.190983005625053M1.53884176858763
    ///     2.11803398874989L2.48989828488278 2.42705098312484L2.48989828488278 3.42705098312484L1.53884176858763
    ///     3.11803398874989zM3.07768353717525 4.23606797749979L2.48989828488278 3.42705098312484L3.44095480117793
    ///     3.11803398874989L3.44095480117793 2.11803398874989L2.48989828488278 2.42705098312484M4.02874005347041
    ///     2.92705098312484L3.44095480117793 2.11803398874989M4.02874005347041 3.92705098312484L3.44095480117793
    ///     3.11803398874989M-1.90211303259031 -2.61803398874989L-2.48989828488278 -1.80901699437495L-3.07768353717525
    ///     -2.61803398874989L-3.07768353717525 -1.61803398874989M-1.90211303259031 -1.61803398874989L-2.48989828488278
    ///     -0.809016994374947L-2.48989828488278 -1.80901699437495M-1.53884176858763 -0.5L-1.53884176858763
    ///     0.5L-2.48989828488278 0.190983005625053L-2.48989828488278 -0.809016994374947L-1.53884176858763
    ///     -0.5L-0.951056516295154 0.309016994374947L0 0M-2.48989828488278 -0.809016994374947L-3.44095480117793
    ///     -0.5L-4.02874005347041 -1.30901699437495L-3.07768353717525 -1.61803398874989zM-4.02874005347041
    ///     -2.30901699437495L-3.07768353717525 -2.61803398874989M-2.48989828488278 -0.809016994374947L-3.07768353717525
    ///     0M-3.07768353717525 1L-2.48989828488278 1.80901699437495L-3.44095480117793 2.11803398874989L-4.02874005347041
    ///     1.30901699437495L-3.07768353717525 1L-3.07768353717525 0L-4.02874005347041 0.309016994374947L-3.44095480117793
    ///     -0.5M-2.1266270208801 1.30901699437495L-1.53884176858763 2.11803398874989L-2.48989828488278
    ///     1.80901699437495M-1.53884176858763 2.11803398874989L-1.53884176858763 3.11803398874989L-2.48989828488278
    ///     3.42705098312484L-2.48989828488278 2.42705098312484zM-3.07768353717525 4.23606797749979L-2.48989828488278
    ///     3.42705098312484L-3.44095480117793 3.11803398874989L-3.44095480117793 2.11803398874989L-2.48989828488278
    ///     2.42705098312484M-3.44095480117793 2.11803398874989L-4.02874005347041 2.92705098312484M-3.44095480117793
    ///     3.11803398874989L-4.02874005347041 3.92705098312484M1.90211303259031 4.23606797749979L2.48989828488278
    ///     3.42705098312484M0 4.23606797749979L0 3.23606797749979L0.951056516295154 2.92705098312484L0.951056516295154
    ///     3.92705098312484L1.53884176858763 3.11803398874989M0.951056516295154 2.92705098312484L1.53884176858763
    ///     2.11803398874989M-0.587785252292473 2.42705098312484L0 3.23606797749979L0.587785252292473
    ///     2.42705098312484M-1.53884176858763 3.11803398874989L-0.951056516295154 3.92705098312484L-0.951056516295154
    ///     2.92705098312484L0 3.23606797749979M-0.951056516295154 2.92705098312484L-1.53884176858763
    ///     2.11803398874989M-1.90211303259031 4.23606797749979L-2.48989828488278 3.42705098312484' fill='none'
    ///     stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-3.5 -3.5 7 7'&gt;&lt;path d='M-1.53884176858763
    ///     -4.73606797749979L-2.48989828488278 -3.42705098312484L-3.44095480117793 -4.73606797749979M3.44095480117793
    ///     -4.73606797749979L2.48989828488278 -3.42705098312484L1.53884176858763 -4.73606797749979M2.48989828488278
    ///     -1.80901699437495L2.48989828488278 -3.42705098312484L4.02874005347041 -2.92705098312484L3.44095480117793
    ///     -2.11803398874989L3.44095480117793 -1.11803398874989L4.02874005347041 -0.309016994374947L4.97979656976556
    ///     0M-2.48989828488278 -1.80901699437495L-1.53884176858763 -2.11803398874989L-2.48989828488278
    ///     -0.809016994374947L-3.44095480117793 -2.11803398874989zM-0.951056516295154 -2.92705098312484L-0.951056516295154
    ///     -3.92705098312484L0 -2.61803398874989L-1.53884176858763 -2.11803398874989zM-2.48989828488278
    ///     -1.80901699437495L-2.48989828488278 -3.42705098312484M-2.48989828488278 1.80901699437495L-2.48989828488278
    ///     0.809016994374947L-1.53884176858763 2.11803398874989L-3.07768353717525 2.61803398874989zM-3.07768353717525
    ///     0L-4.02874005347041 -0.309016994374947L-2.48989828488278 -0.809016994374947L-2.48989828488278
    ///     0.809016994374947zM-2.48989828488278 1.80901699437495L-4.02874005347041 1.30901699437495L-3.07768353717525
    ///     0M-4.97979656976556 0L-4.02874005347041 -0.309016994374947L-3.44095480117793 -1.11803398874989L-3.44095480117793
    ///     -2.11803398874989L-4.02874005347041 -2.92705098312484L-2.48989828488278 -3.42705098312484L-0.951056516295154
    ///     -2.92705098312484M-0.951056516295154 0.309016994374947L-1.53884176858763 -0.5L0 0L-0.951056516295154
    ///     1.30901699437495zM-0.951056516295154 0.309016994374947L-2.48989828488278 0.809016994374947M-1.53884176858763
    ///     -2.11803398874989L-0.587785252292473 -0.809016994374947M-2.48989828488278 -0.809016994374947L-1.53884176858763
    ///     -0.5L-0.587785252292473 -0.809016994374947L0 -1.61803398874989L0 0L1.53884176858763 -0.5L0.587785252292473
    ///     -0.809016994374947L0 -1.61803398874989L0 -2.61803398874989L1.53884176858763 -2.11803398874989L0.587785252292473
    ///     -0.809016994374947M0 0L0.951056516295154 1.30901699437495L0 1L-0.951056516295154
    ///     1.30901699437495L-1.53884176858763 2.11803398874989L0 2.61803398874989L0 1M1.53884176858763
    ///     -2.11803398874989L2.48989828488278 -1.80901699437495L3.44095480117793 -2.11803398874989L2.48989828488278
    ///     -0.809016994374947L2.48989828488278 0.809016994374947L0.951056516295154 0.309016994374947M0.951056516295154
    ///     -2.92705098312484L2.48989828488278 -3.42705098312484M-3.44095480117793 -1.11803398874989L-4.97979656976556
    ///     -1.61803398874989M2.48989828488278 0.809016994374947L2.48989828488278 1.80901699437495L3.07768353717525
    ///     2.61803398874989L1.53884176858763 2.11803398874989L2.48989828488278 0.809016994374947L3.07768353717525
    ///     0L4.02874005347041 -0.309016994374947L2.48989828488278 -0.809016994374947M3.07768353717525 0L4.02874005347041
    ///     1.30901699437495L2.48989828488278 1.80901699437495M4.02874005347041 2.92705098312484L3.07768353717525
    ///     2.61803398874989L2.1266270208801 2.92705098312484L1.53884176858763 3.73606797749979L1.53884176858763
    ///     4.73606797749979M4.97979656976556 -1.61803398874989L3.44095480117793 -1.11803398874989M1.53884176858763
    ///     2.11803398874989L1.53884176858763 3.73606797749979L0.951056516295154 2.92705098312484L0
    ///     2.61803398874989L1.53884176858763 2.11803398874989L0.951056516295154 1.30901699437495L0.951056516295154
    ///     0.309016994374947L1.53884176858763 -0.5L2.48989828488278 -0.809016994374947L1.53884176858763
    ///     -2.11803398874989L0.951056516295154 -2.92705098312484L0.951056516295154 -3.92705098312484L0 -2.61803398874989M0
    ///     2.61803398874989L-0.951056516295154 2.92705098312484L-1.53884176858763 3.73606797749979L-1.53884176858763
    ///     2.11803398874989M0.951056516295154 2.92705098312484L0 4.23606797749979L-0.951056516295154
    ///     2.92705098312484M-1.53884176858763 4.73606797749979L-1.53884176858763 3.73606797749979L-2.1266270208801
    ///     2.92705098312484L-3.07768353717525 2.61803398874989L-4.02874005347041 2.92705098312484M-2.1266270208801
    ///     2.92705098312484L-3.07768353717525 4.23606797749979M3.07768353717525 4.23606797749979L2.1266270208801
    ///     2.92705098312484' fill='none' stroke-width='.05' stroke='black' /&gt;&lt;/svg&gt;</image>
    /// <param name="kind">
    ///     The kind of shape.</param>
    /// <param name="corner">
    ///     The “main” corner of the shape.</param>
    /// <param name="angle">
    ///     The rotation angle of the shape.</param>
    public struct Penrose(Penrose.Kind kind, Pentavector corner, int angle) : IEquatable<Penrose>, INeighbor<Penrose>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>Indicates which kind of shape this is.</summary>
        public Kind TileKind { get; private set; } = kind;
        /// <summary>Indicates the “main” corner of this shape.</summary>
        public Pentavector Corner { get; private set; } = corner;
        /// <summary>Indicates the rotation angle of this shape.</summary>
        public int Angle { get; private set; } = (angle % 10 + 10) % 10;

        /// <inheritdoc/>
        public readonly bool Equals(Penrose other) => other.TileKind == TileKind && other.Corner == Corner && other.Angle == Angle;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is Penrose other && other.TileKind == TileKind && other.Corner == Corner && other.Angle == Angle;
        /// <inheritdoc/>
        public override readonly int GetHashCode() => unchecked((Corner.GetHashCode() * 10 + Angle) * 4 + (int) TileKind);

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <inheritdoc/>
        public readonly PointD Center => TileKind switch
        {
            Kind.Dart => (3 * Corner.MultiplyByPhi + Pentavector.Base(Angle)).Point / 3,
            Kind.Kite => (Corner.MultiplyByPhi + Pentavector.Base(Angle)).Point,
            Kind.ThinRhomb => (2 * (Corner + Pentavector.Base(Angle)) + Pentavector.Base(Angle + 3).DivideByPhi).Point / 2,
            Kind.ThickRhomb => (2 * Corner + Pentavector.Base(Angle).MultiplyByPhi).Point / 2,
            _ => throw new InvalidOperationException($"Invalid TileKind value ‘{TileKind}’.")
        };

        /// <summary>
        ///     Returns the vertices along the perimeter of this <see cref="Penrose"/>, going clockwise from the “main” vertex
        ///     (<see cref="Corner"/> in P2; <see cref="Corner"/> times φ in P3).</summary>
        public readonly Vertex[] Vertices => TileKind switch
        {
            Kind.Dart => [Corner.MultiplyByPhi, Corner.MultiplyByPhi + Pentavector.Base(Angle + 7), Corner.MultiplyByPhi + Pentavector.Base(Angle), Corner.MultiplyByPhi + Pentavector.Base(Angle + 3)],
            Kind.Kite => [Corner.MultiplyByPhi, (Corner + Pentavector.Base(Angle + 9)).MultiplyByPhi, (Corner + Pentavector.Base(Angle)).MultiplyByPhi, (Corner + Pentavector.Base(Angle + 1)).MultiplyByPhi],
            Kind.ThickRhomb => [Corner, Corner + Pentavector.Base(Angle + 9), Corner + Pentavector.Base(Angle + 9) + Pentavector.Base(Angle + 1), Corner + Pentavector.Base(Angle + 1)],
            Kind.ThinRhomb => [Corner, Corner + Pentavector.Base(Angle), Corner + Pentavector.Base(Angle) + Pentavector.Base(Angle + 1), Corner + Pentavector.Base(Angle + 1)],
            _ => throw new InvalidOperationException($"Invalid TileKind value ‘{TileKind}’."),
        };

        /// <summary>
        ///     Returns a collection of tiles obtained from one iteration of deflating (expanding) this tile.</summary>
        /// <remarks>
        ///     Note that deflating several adjacent tiles will generate some duplicated tiles.</remarks>
        public readonly IEnumerable<Penrose> DeflatedTiles
        {
            get
            {
                switch (TileKind)
                {
                    case Kind.Kite:
                        yield return new Penrose(Kind.Dart, Corner.MultiplyByPhi + Pentavector.Base(Angle + 9).DivideByPhi, Angle + 4);
                        yield return new Penrose(Kind.Dart, Corner.MultiplyByPhi + Pentavector.Base(Angle + 1).DivideByPhi, Angle + 6);
                        yield return new Penrose(Kind.Kite, (Corner + Pentavector.Base(Angle + 9)).MultiplyByPhi, Angle + 3);
                        yield return new Penrose(Kind.Kite, (Corner + Pentavector.Base(Angle + 1)).MultiplyByPhi, Angle + 7);

                        // Extra
                        yield return new Penrose(Kind.Kite, (Corner + Pentavector.Base(Angle + 1)).MultiplyByPhi, Angle + 5);
                        yield return new Penrose(Kind.Kite, (Corner + Pentavector.Base(Angle + 9)).MultiplyByPhi, Angle + 5);
                        yield break;

                    case Kind.Dart:
                        yield return new Penrose(Kind.Dart, Corner.MultiplyByPhi + new Pentavector(0, -1, 1, -1).Rotate(Angle), Angle + 6);
                        yield return new Penrose(Kind.Dart, Corner.MultiplyByPhi + new Pentavector(1, -1, 1, 0).Rotate(Angle), Angle + 4);
                        yield return new Penrose(Kind.Kite, Corner.MultiplyByPhi + Pentavector.Base(Angle), Angle + 5);

                        // Extra
                        yield return new Penrose(Kind.Kite, Corner.MultiplyByPhi + Pentavector.Base(Angle), Angle + 7);
                        yield return new Penrose(Kind.Kite, Corner.MultiplyByPhi + Pentavector.Base(Angle), Angle + 3);
                        yield break;

                    case Kind.ThickRhomb:
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Pentavector.Base(Angle)).MultiplyByPhi, Angle + 5);
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Pentavector.Base(Angle).MultiplyByPhi).MultiplyByPhi, Angle + 6);
                        yield return new Penrose(Kind.ThickRhomb, (Corner + Pentavector.Base(Angle).MultiplyByPhi).MultiplyByPhi, Angle + 4);
                        yield return new Penrose(Kind.ThinRhomb, (Corner + Pentavector.Base(Angle)).MultiplyByPhi, Angle + 6);
                        yield return new Penrose(Kind.ThinRhomb, Corner.MultiplyByPhi + new Pentavector(1, 0, 1, 0).Rotate(Angle), Angle + 8);

                        // Extra
                        yield return new Penrose(Kind.ThickRhomb, Corner.MultiplyByPhi + new Pentavector(1, 0, 1, 0).Rotate(Angle), Angle + 7);
                        yield return new Penrose(Kind.ThickRhomb, Corner.MultiplyByPhi + new Pentavector(0, -1, 0, -1).Rotate(Angle), Angle + 3);
                        yield break;

                    case Kind.ThinRhomb:
                        var cb = (Corner + Pentavector.Base(Angle + 1)).MultiplyByPhi;
                        yield return new Penrose(Kind.ThickRhomb, cb, Angle + 6);
                        yield return new Penrose(Kind.ThickRhomb, cb, Angle);
                        yield return new Penrose(Kind.ThinRhomb, cb, Angle + 7);
                        yield return new Penrose(Kind.ThinRhomb, (Corner + Pentavector.Base(Angle)).MultiplyByPhi + Pentavector.Base(Angle + 9), Angle + 3);
                        yield break;

                    default: throw new InvalidOperationException($"Invalid Penrose.TileKind value ‘{TileKind}’.");
                }
            }
        }

        /// <inheritdoc/>
        public readonly IEnumerable<Penrose> Neighbors
        {
            get
            {
                if (TileKind == Kind.Kite)
                {
                    // Neighbors left/right of the Corner vertex
                    yield return new Penrose(Kind.Kite, Corner, Angle + 8);
                    yield return new Penrose(Kind.Kite, Corner, Angle + 2);
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(0, -1, 1, -1).Rotate(Angle), Angle);
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(1, -1, 1, 0).Rotate(Angle), Angle);

                    // Neighbors left/right of the opposite vertex
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(0, 0, 0, -1).Rotate(Angle), Angle + 9);
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(1, 0, 0, 0).Rotate(Angle), Angle + 1);
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(1, -1, 1, -2).Rotate(Angle), Angle + 4);
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(2, -1, 1, -1).Rotate(Angle), Angle + 6);
                }
                else if (TileKind == Kind.Dart)
                {
                    // Neighbors left/right of the Corner vertex
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(-1, 1, -1, 1).Rotate(Angle), Angle + 9);
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(-1, 1, -1, 1).Rotate(Angle), Angle + 1);

                    // Neighbors of the left/right edge
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(-1, 1, -1, 0).Rotate(Angle), Angle);
                    yield return new Penrose(Kind.Kite, Corner + new Pentavector(0, 1, -1, 1).Rotate(Angle), Angle);

                    // Neighbors left/right of the opposite vertex
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(0, 2, -2, 1).Rotate(Angle), Angle + 8);
                    yield return new Penrose(Kind.Dart, Corner + new Pentavector(-1, 2, -2, 0).Rotate(Angle), Angle + 2);
                }
                else if (TileKind == Kind.ThickRhomb)
                {
                    // Neighbors left/right of the Corner vertex
                    yield return new Penrose(Kind.ThickRhomb, Corner, Angle + 2);
                    yield return new Penrose(Kind.ThickRhomb, Corner, Angle + 8);
                    yield return new Penrose(Kind.ThinRhomb, Corner, Angle + 1);
                    yield return new Penrose(Kind.ThinRhomb, Corner + new Pentavector(0, 0, -1, -1).Rotate(Angle), Angle + 3);

                    // Neighbors left/right of the opposite vertex
                    yield return new Penrose(Kind.ThickRhomb, Corner + new Pentavector(0, 0, -1, -1).Rotate(Angle), Angle + 2);
                    yield return new Penrose(Kind.ThinRhomb, Corner + new Pentavector(0, 0, 0, -1).Rotate(Angle), Angle);
                    yield return new Penrose(Kind.ThickRhomb, Corner + new Pentavector(1, 1, 0, 0).Rotate(Angle), Angle + 8);
                    yield return new Penrose(Kind.ThinRhomb, Corner + new Pentavector(2, -1, 1, -2).Rotate(Angle), Angle + 4);
                }
                else if (TileKind == Kind.ThinRhomb)
                {
                    // Neighbors left/right of the Corner vertex
                    yield return new Penrose(Kind.ThickRhomb, Corner, Angle + 9);
                    yield return new Penrose(Kind.ThickRhomb, Corner + new Pentavector(0, 0, 0, 1).Rotate(Angle), Angle);
                    yield return new Penrose(Kind.ThinRhomb, Corner + new Pentavector(1, 1, 0, 0).Rotate(Angle), Angle + 6);

                    // Neighbors left/right of the opposite vertex
                    yield return new Penrose(Kind.ThickRhomb, Corner + new Pentavector(2, -1, 1, -1).Rotate(Angle), Angle + 7);
                    yield return new Penrose(Kind.ThickRhomb, Corner + new Pentavector(2, 0, 1, -1).Rotate(Angle), Angle + 6);
                    yield return new Penrose(Kind.ThinRhomb, Corner + new Pentavector(2, -1, 1, -1).Rotate(Angle), Angle + 4);
                }
                else
                    throw new InvalidOperationException($"Invalid Penrose.TileKind value ‘{TileKind}’.");
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <inheritdoc/>
        public override readonly string ToString() => $"P{Corner}/{(int) TileKind}/{Angle}";

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

            /// <summary>
            ///     Constructs a grid consisting of <see cref="Penrose"/> tiles obtained by deflating (subdividing) a star
            ///     consisting of five initial shapes.</summary>
            /// <param name="useRhombs">
            ///     If <c>true</c>, the initial shapes are <see cref="Kind.ThickRhomb"/> tiles and the final grid will consist
            ///     of those and <see cref="Kind.ThinRhomb"/> tiles. If <c>false</c>, the initial shapes are <see
            ///     cref="Kind.Kite"/> tiles and the final grid will consist of those and <see cref="Kind.Dart"/> tiles.</param>
            /// <param name="numIterations">
            ///     Number of deflation iterations to perform.</param>
            public Grid(bool useRhombs, int numIterations) : base(GenerateGrid(useRhombs, numIterations))
            {
            }

            /// <summary>
            ///     Rotates the entire grid by the specified <paramref name="angle"/>.</summary>
            /// <param name="angle">
            ///     The angle, specified as a multiple of 36° going clockwise (SVG) or counter-clockwise (geometry).</param>
            /// <returns>
            ///     The new grid.</returns>
            public Grid Rotate(int angle) => new(_cells.Select(c => c.Rotate(angle)), _links.Select(l => new Link<Penrose>(l.Apart(out var other).Rotate(angle), other.Rotate(angle))));

            /// <inheritdoc/>
            protected override Structure<Penrose> makeModifiedStructure(IEnumerable<Penrose> cells, IEnumerable<Link<Penrose>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);

            private static IEnumerable<Penrose> GenerateGrid(bool useRhombs, int numIterations)
            {
                var origTiles = Enumerable.Range(0, 5).Select(angle => new Penrose(useRhombs ? Kind.ThickRhomb : Kind.Kite, default, 2 * angle));
                var tiles = origTiles;

                IEnumerable<Penrose> prev = null;
                for (var i = 0; i < numIterations; i++)
                {
                    prev = tiles;
                    tiles = tiles.SelectMany(t => t.DeflatedTiles).Distinct();
                }

                return tiles;
            }
        }

        /// <summary>
        ///     Rotates this cell by the specified <paramref name="angle"/> about the origin.</summary>
        /// <param name="angle">
        ///     The angle, specified as a multiple of 36° going clockwise (SVG) or counter-clockwise (geometry).</param>
        public readonly Penrose Rotate(int angle) => new(TileKind, Corner.Rotate(angle), Angle + angle);
    }
}
