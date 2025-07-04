using System;
using System.Collections.Generic;
using System.Linq;
using RT.Geometry;

namespace RT.Coordinates
{
    /// <summary>
    ///     <para>
    ///         Represents a cell in a <see cref="Grid"/> consisting mostly of pentagons and a few thin rhombuses that fill
    ///         the gaps.</para></summary>
    /// <remarks>
    ///     Represents a cell in a two-dimensional grid that is either a pentagon or a thin rhombus (<see cref="IsRhombus"/>).
    ///     The cells are represented by a <see cref="Level"/> (innermost pentagon is Level 0, each subsequent layer grows
    ///     outwards in all directions), an <see cref="Element"/> number and a <see cref="Rotation"/>. Since the structure has
    ///     pentagonal symmetry, the <see cref="Element"/> property only counts one fifth of the elements in a layer, while
    ///     the values 0–4 of <see cref="Rotation"/> are then used to create the 5 symmetric copies of those elements.</remarks>
    /// <image type="raw">
    ///     &lt;svg xmlns='http://www.w3.org/2000/svg' viewBox='-5.5 -5.5 11 11'&gt;&lt;path d='M0 -1L0.9510565162951535
    ///     -0.30901699437494745L0.5877852522924731 0.8090169943749475L-0.5877852522924731
    ///     0.8090169943749475L-0.9510565162951535 -0.30901699437494745zM-0.9510565162951535
    ///     -0.30901699437494745L-1.902113032590307 -1L-1.5388417685876266 -2.118033988749895L-0.3632712640026804
    ///     -2.118033988749895L0 -1L0.3632712640026804 -2.118033988749895L1.5388417685876266
    ///     -2.118033988749895L1.902113032590307 -1L0.9510565162951535 -0.30901699437494745L2.1266270208801
    ///     -0.30901699437494745L2.48989828488278 0.8090169943749475L1.5388417685876266 1.5L0.5877852522924731
    ///     0.8090169943749475L0.9510565162951535 1.9270509831248424L0 2.618033988749895L-0.9510565162951535
    ///     1.9270509831248424L-0.5877852522924731 0.8090169943749475L-1.5388417685876266 1.5L-2.48989828488278
    ///     0.8090169943749475L-2.1266270208801 -0.30901699437494745zM0.3632712640026804 -2.118033988749895L0
    ///     -3.23606797749979L0.9510565162951535 -3.9270509831248424L1.902113032590307 -3.23606797749979L1.5388417685876266
    ///     -2.118033988749895L2.48989828488278 -2.8090169943749475L3.4409548011779334 -2.118033988749895L3.077683537175253
    ///     -1L1.902113032590307 -1M2.1266270208801 -0.30901699437494745L3.077683537175253 -1L4.028740053470407
    ///     -0.30901699437494745L3.6654687894677265 0.8090169943749475L2.48989828488278 0.8090169943749475L3.4409548011779334
    ///     1.5L3.077683537175253 2.618033988749895L1.902113032590307 2.618033988749895L1.5388417685876266
    ///     1.5M0.9510565162951535 1.9270509831248424L1.902113032590307 2.618033988749895L1.5388417685876266
    ///     3.73606797749979L0.3632712640026804 3.73606797749979L0 2.618033988749895L-0.3632712640026804
    ///     3.73606797749979L-1.5388417685876266 3.73606797749979L-1.902113032590307 2.618033988749895L-0.9510565162951535
    ///     1.9270509831248424M-1.5388417685876266 1.5L-1.902113032590307 2.618033988749895L-3.077683537175253
    ///     2.618033988749895L-3.4409548011779334 1.5L-2.48989828488278 0.8090169943749475L-3.6654687894677265
    ///     0.8090169943749475L-4.028740053470407 -0.30901699437494745L-3.077683537175253 -1L-2.1266270208801
    ///     -0.30901699437494745M-1.902113032590307 -1L-3.077683537175253 -1L-3.4409548011779334
    ///     -2.118033988749895L-2.48989828488278 -2.8090169943749475L-1.5388417685876266 -2.118033988749895L-1.902113032590307
    ///     -3.23606797749979L-0.9510565162951535 -3.9270509831248424L0 -3.23606797749979L-0.3632712640026804
    ///     -2.118033988749895M-1.902113032590307 -3.23606797749979L-2.8531695488854605 -3.9270509831248424L-2.48989828488278
    ///     -5.045084971874737L-1.314327780297834 -5.045084971874737L-0.9510565162951535
    ///     -3.9270509831248424L-0.5877852522924731 -5.045084971874737L0.5877852522924731
    ///     -5.045084971874737L0.9510565162951535 -3.9270509831248424L1.314327780297834 -5.045084971874737L2.48989828488278
    ///     -5.045084971874737L2.8531695488854605 -3.9270509831248424L1.902113032590307 -3.23606797749979M2.48989828488278
    ///     -2.8090169943749475L2.8531695488854605 -3.9270509831248424L4.028740053470407 -3.9270509831248424L4.392011317473087
    ///     -2.8090169943749475L3.4409548011779334 -2.118033988749895L4.61652530576288 -2.118033988749895L4.97979656976556
    ///     -1L4.028740053470407 -0.30901699437494745L5.204310558055353 -0.30901699437494745L5.567581822058034
    ///     0.8090169943749475L4.61652530576288 1.5L3.6654687894677265 0.8090169943749475M0.3632712640026804
    ///     3.73606797749979L0 4.854101966249685L0.9510565162951535 5.545084971874737L1.902113032590307
    ///     4.854101966249685L1.5388417685876266 3.73606797749979L2.48989828488278 4.427050983124842L3.4409548011779334
    ///     3.73606797749979L3.077683537175253 2.618033988749895L4.028740053470407 3.3090169943749475L4.97979656976556
    ///     2.618033988749895L4.61652530576288 1.5L3.4409548011779334 1.5M-3.4409548011779334 1.5L-4.61652530576288
    ///     1.5L-4.97979656976556 2.618033988749895L-4.028740053470407 3.3090169943749475L-3.077683537175253
    ///     2.618033988749895L-3.4409548011779334 3.73606797749979L-2.48989828488278 4.427050983124842L-1.5388417685876266
    ///     3.73606797749979L-1.902113032590307 4.854101966249685L-0.9510565162951535 5.545084971874737L0
    ///     4.854101966249685L-0.3632712640026804 3.73606797749979M-3.6654687894677265 0.8090169943749475L-4.61652530576288
    ///     1.5L-5.567581822058034 0.8090169943749475L-5.204310558055353 -0.30901699437494745L-4.028740053470407
    ///     -0.30901699437494745L-4.97979656976556 -1L-4.61652530576288 -2.118033988749895L-3.4409548011779334
    ///     -2.118033988749895L-4.392011317473087 -2.8090169943749475L-4.028740053470407
    ///     -3.9270509831248424L-2.8531695488854605 -3.9270509831248424L-2.48989828488278
    ///     -2.8090169943749475M-0.5877852522924731 -5.045084971874737L-0.9510565162951535 -6.163118960624632L0
    ///     -6.854101966249685L0.9510565162951535 -6.163118960624632L0.5877852522924731 -5.045084971874737M1.314327780297834
    ///     -5.045084971874737L0.9510565162951535 -6.163118960624632L1.902113032590307 -6.854101966249685L2.8531695488854605
    ///     -6.163118960624632L2.48989828488278 -5.045084971874737L3.4409548011779334 -5.73606797749979L4.392011317473087
    ///     -5.045084971874737L4.028740053470407 -3.9270509831248424L4.97979656976556 -4.618033988749895L5.930853086060713
    ///     -3.9270509831248424L5.567581822058034 -2.8090169943749475L4.392011317473087 -2.8090169943749475M4.61652530576288
    ///     -2.118033988749895L5.567581822058034 -2.8090169943749475L6.5186383383531865 -2.118033988749895L6.155367074350506
    ///     -1L4.97979656976556 -1M5.204310558055353 -0.30901699437494745L6.155367074350506 -1L7.10642359064566
    ///     -0.30901699437494745L6.74315232664298 0.8090169943749475L5.567581822058034 0.8090169943749475L6.5186383383531865
    ///     1.5L6.155367074350506 2.618033988749895L4.97979656976556 2.618033988749895L5.930853086060713
    ///     3.3090169943749475L5.567581822058034 4.427050983124842L4.392011317473087 4.427050983124842L4.028740053470407
    ///     3.3090169943749475M3.4409548011779334 3.73606797749979L4.392011317473087 4.427050983124842L4.028740053470407
    ///     5.545084971874737L2.8531695488854605 5.545084971874737L2.48989828488278 4.427050983124842M1.902113032590307
    ///     4.854101966249685L2.8531695488854605 5.545084971874737L2.48989828488278 6.663118960624632L1.314327780297834
    ///     6.663118960624632L0.9510565162951535 5.545084971874737L0.5877852522924731 6.663118960624632L-0.5877852522924731
    ///     6.663118960624632L-0.9510565162951535 5.545084971874737L-1.314327780297834 6.663118960624632L-2.48989828488278
    ///     6.663118960624632L-2.8531695488854605 5.545084971874737L-1.902113032590307 4.854101966249685M-2.48989828488278
    ///     4.427050983124842L-2.8531695488854605 5.545084971874737L-4.028740053470407 5.545084971874737L-4.392011317473087
    ///     4.427050983124842L-3.4409548011779334 3.73606797749979M-4.028740053470407 3.3090169943749475L-4.392011317473087
    ///     4.427050983124842L-5.567581822058034 4.427050983124842L-5.930853086060713 3.3090169943749475L-4.97979656976556
    ///     2.618033988749895L-6.155367074350506 2.618033988749895L-6.5186383383531865 1.5L-5.567581822058034
    ///     0.8090169943749475L-6.74315232664298 0.8090169943749475L-7.10642359064566 -0.30901699437494745L-6.155367074350506
    ///     -1L-5.204310558055353 -0.30901699437494745M-4.97979656976556 -1L-6.155367074350506 -1L-6.5186383383531865
    ///     -2.118033988749895L-5.567581822058034 -2.8090169943749475L-4.61652530576288 -2.118033988749895M-4.392011317473087
    ///     -2.8090169943749475L-5.567581822058034 -2.8090169943749475L-5.930853086060713
    ///     -3.9270509831248424L-4.97979656976556 -4.618033988749895L-4.028740053470407 -3.9270509831248424L-4.392011317473087
    ///     -5.045084971874737L-3.4409548011779334 -5.73606797749979L-2.48989828488278 -5.045084971874737L-2.8531695488854605
    ///     -6.163118960624632L-1.902113032590307 -6.854101966249685L-0.9510565162951535 -6.163118960624632L-1.314327780297834
    ///     -5.045084971874737M-2.8531695488854605 -6.163118960624632L-3.804226065180614
    ///     -6.854101966249685L-3.4409548011779334 -5.73606797749979M3.4409548011779334 -5.73606797749979L3.804226065180614
    ///     -6.854101966249685L2.8531695488854605 -6.163118960624632M4.97979656976556 -4.618033988749895L5.343067833768241
    ///     -5.73606797749979L4.392011317473087 -5.045084971874737M6.5186383383531865 1.5L7.694208842938133
    ///     1.5L6.74315232664298 0.8090169943749475M5.930853086060713 3.3090169943749475L7.10642359064566
    ///     3.3090169943749475L6.155367074350506 2.618033988749895M0.5877852522924731 6.663118960624632L0.9510565162951535
    ///     7.781152949374527L1.314327780297834 6.663118960624632M-1.314327780297834 6.663118960624632L-0.9510565162951535
    ///     7.781152949374527L-0.5877852522924731 6.663118960624632M-6.155367074350506 2.618033988749895L-7.10642359064566
    ///     3.3090169943749475L-5.930853086060713 3.3090169943749475M-6.74315232664298 0.8090169943749475L-7.694208842938133
    ///     1.5L-6.5186383383531865 1.5M-4.392011317473087 -5.045084971874737L-5.343067833768241
    ///     -5.73606797749979L-4.97979656976556 -4.618033988749895' fill='none' stroke-width='.07' stroke='black'
    ///     /&gt;&lt;/svg&gt;</image>
    public struct PentaCell : IEquatable<PentaCell>, INeighbor<PentaCell>, INeighbor<object>, IHasSvgGeometry
    {
        /// <summary>Describes at which level the cell is (outwards from the center).</summary>
        public int Level { get; private set; }
        /// <summary>Identifies the element within one sector of a level in clockwise order.</summary>
        public int Element { get; private set; }
        /// <summary>Specifies which of the five sectors within a level contains this cell.</summary>
        public int Rotation { get; private set; }
        /// <summary>Specifies whether the cell is a pentagon (<c>false</c>) or a rhombus (<c>true</c>).</summary>
        public bool IsRhombus { get; private set; }

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="level">
        ///     Describes at which level the cell is (outwards from the center).</param>
        /// <param name="element">
        ///     Identifies the element within one sector of a level in clockwise order.</param>
        /// <param name="rotation">
        ///     Specifies which of the five sectors within a level contains this cell.</param>
        /// <param name="isRhombus">
        ///     Specifies whether the cell is a pentagon (<c>false</c>) or a rhombus (<c>true</c>).</param>
        public PentaCell(int level, int element, int rotation, bool isRhombus)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level), $"‘{nameof(level)}’ cannot be negative (received {level}).");

            if (isRhombus && (element < 0 || element >= (level >> 1)))
                throw new ArgumentOutOfRangeException(nameof(element), $"For a rhombus, ‘{nameof(element)}’ must be between 0 (inclusive) and ‘level >> 1’ (exclusive) (received {element}).");

            if (!isRhombus && (element < 0 || (level == 0 ? element > 0 : element >= level)))
                throw new ArgumentOutOfRangeException(nameof(element), $"For a pentagon, ‘{nameof(element)}’ must be between 0 (inclusive) and ‘level’ (exclusive), or 0 in level 0 (received {element}).");

            if (rotation < 0 || rotation >= 5)
                throw new ArgumentOutOfRangeException(nameof(rotation), $"‘{nameof(rotation)}’ must be between 0 (inclusive) and 5 (exclusive) (received {rotation}).");

            Level = level;
            Element = element;
            Rotation = rotation;
            IsRhombus = isRhombus;
        }

        /// <inheritdoc/>
        public readonly bool Equals(PentaCell other) => other.Level == Level && other.Element == Element && other.Rotation == Rotation && other.IsRhombus == IsRhombus;
        /// <inheritdoc/>
        public override readonly bool Equals(object obj) => obj is PentaCell other && other.Level == Level && other.Element == Element && other.Rotation == Rotation && other.IsRhombus == IsRhombus;
        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            var hashCode = -2003805146;
            hashCode = hashCode * -1521134295 + Level.GetHashCode();
            hashCode = hashCode * -1521134295 + Element.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + IsRhombus.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public readonly IEnumerable<Edge> Edges => Vertices.MakeEdges();

        /// <summary>
        ///     Returns the <see cref="Pentavector"/> corresponding to a pentagon cell’s center. Warning: throws if this cell
        ///     is not a pentagon.</summary>
        private readonly Pentavector PentagonCenter => !IsRhombus
            ? (((Level >> 1) * new Pentavector(2, -1, 0, -2) + (Level & 1) * new Pentavector(1, -1, 0, -1))
                + Element * new Pentavector(0, 1, 1, 0)
                + (Element > ((Level + 1) >> 1) ? new Pentavector(0, -1, 0, 1) * (Element - ((Level + 1) >> 1)) : default))
                    .Rotate(2 * Rotation)
            : throw new InvalidOperationException();

        private readonly Pentavector RhombusCorner => IsRhombus
            ? (((Level >> 1) - 1) * new Pentavector(2, -1, 0, -2)
                + ((Level & 1) != 0
                        ? new Pentavector(2, -1, 0, -1) + (Level >> 1) * new Pentavector(0, 1, 1, 0) + Element * new Pentavector(0, 0, 1, 1)
                        : new Pentavector(1, -1, 1, -1) + Element * new Pentavector(0, 1, 1, 0))).Rotate(2 * Rotation)
            : throw new InvalidOperationException();

        /// <inheritdoc/>
        public readonly PointD Center
        {
            get
            {
                if (!IsRhombus)
                    return PentagonCenter.Point;
                var c = RhombusCorner;
                var d = c + new Pentavector(1, 1, -1, -1).Rotate(2 * Rotation + (Level & 1));
                return (c.Point + d.Point) / 2;
            }
        }

        /// <summary>Returns the vertices along the perimeter of this <see cref="PentaCell"/> in clockwise order.</summary>
        public readonly Vertex[] Vertices
        {
            get
            {
                if (IsRhombus)
                {
                    var d = RhombusCorner;
                    return GridUtils.NewArray<Vertex>(
                        d,
                        d + new Pentavector(1, 0, -1, 0).Rotate((2 * Rotation + (Level & 1))),
                        d + new Pentavector(1, 1, -1, -1).Rotate((2 * Rotation + (Level & 1))),
                        d + new Pentavector(0, 1, 0, -1).Rotate((2 * Rotation + (Level & 1))));
                }
                else
                {
                    var c = PentagonCenter;
                    var upsideDown = (Level & 1) != 0;
                    return GridUtils.NewArray<Vertex>(
                        c + (upsideDown ? Pentavector.BaseA : -Pentavector.BaseE),
                        c + (upsideDown ? Pentavector.BaseC : Pentavector.BaseB),
                        c + (upsideDown ? Pentavector.BaseE : Pentavector.BaseD),
                        c + (upsideDown ? -Pentavector.BaseB : -Pentavector.BaseA),
                        c + (upsideDown ? -Pentavector.BaseD : -Pentavector.BaseC));
                }
            }
        }

        /// <inheritdoc/>
        public readonly IEnumerable<PentaCell> Neighbors
        {
            get
            {
                if (!IsRhombus && Level == 0)
                {
                    // Center pentagon
                    for (var l = 0; l < 5; l++)
                        yield return new PentaCell(1, 0, l, isRhombus: false);
                    yield break;
                }
                else if (!IsRhombus && Level == 1)
                {
                    // Pentagon, level 1
                    yield return new PentaCell(0, 0, 0, isRhombus: false);
                    yield return new PentaCell(2, 0, Rotation, isRhombus: true);
                    yield return new PentaCell(2, 0, (Rotation + 4) % 5, isRhombus: true);
                    yield return new PentaCell(2, 1, (Rotation + 4) % 5, isRhombus: false);
                    yield return new PentaCell(2, 0, Rotation, isRhombus: false);
                }
                else if (!IsRhombus && (Level & 1) == 0)
                {
                    // Pentagon, level is even
                    if (Element < Level >> 1)
                        yield return new PentaCell(Level, Element, Rotation, isRhombus: true);
                    if (Element > 0 && Element <= Level >> 1)
                        yield return new PentaCell(Level, Element - 1, Rotation, isRhombus: true);
                    if (Element == 0)
                        yield return new PentaCell(Level + 1, (Level >> 1) - 1, (Rotation + 4) % 5, isRhombus: true);
                    if (Element >= Level >> 1)
                        yield return new PentaCell(Level + 1, Element - (Level >> 1), Rotation, isRhombus: true);
                    if (Element > Level >> 1)
                        yield return new PentaCell(Level + 1, Element - (Level >> 1) - 1, Rotation, isRhombus: true);

                    if (Element > Level >> 1)
                        yield return new PentaCell(Level - 1, Element - 1, Rotation, isRhombus: false);
                    yield return new PentaCell(Level - 1, Element % (Level - 1), (Rotation + Element / (Level - 1)) % 5, isRhombus: false);
                    if (Element <= Level >> 1)
                        yield return new PentaCell(Level + 1, Element, Rotation, isRhombus: false);
                    yield return new PentaCell(Level + 1, Element + 1, Rotation, isRhombus: false);
                }
                else if (!IsRhombus)
                {
                    // Pentagon, level is odd
                    if (Element == 0)
                        yield return new PentaCell(Level, (Level >> 1) - 1, (Rotation + 4) % 5, isRhombus: true);
                    if (Element > 0 && Element <= (Level >> 1) + 1)
                        yield return new PentaCell(Level + 1, Element - 1, Rotation, isRhombus: true);
                    if (Element > Level >> 1)
                    {
                        yield return new PentaCell(Level, Element - (Level >> 1) - 1, Rotation, isRhombus: true);
                        yield return new PentaCell(Level + 1, Element + 1, Rotation, isRhombus: false);
                    }
                    if (Element > (Level >> 1) + 1)
                        yield return new PentaCell(Level, Element - (Level >> 1) - 2, Rotation, isRhombus: true);
                    if (Element <= Level >> 1)
                    {
                        yield return new PentaCell(Level + 1, Element, Rotation, isRhombus: true);
                        yield return new PentaCell(Level - 1, Element, Rotation, isRhombus: false);
                    }
                    if (Element > 0)
                        yield return new PentaCell(Level - 1, Element - 1, Rotation, isRhombus: false);
                    if (Element == 0)
                        yield return new PentaCell(Level + 1, Level, (Rotation + 4) % 5, isRhombus: false);
                    yield return new PentaCell(Level + 1, Element, Rotation, isRhombus: false);
                }
                else if (IsRhombus && Level == 2)
                {
                    // Rhombus, level 2
                    yield return new PentaCell(Level - 1, 0, Rotation, isRhombus: false);
                    yield return new PentaCell(Level - 1, 0, (Rotation + 1) % 5, isRhombus: false);
                    yield return new PentaCell(Level, 0, Rotation, isRhombus: false);
                    yield return new PentaCell(Level, 1, Rotation, isRhombus: false);
                }
                else if (IsRhombus && (Level & 1) == 0)
                {
                    // Rhombus, level is even
                    yield return new PentaCell(Level - 1, Element, Rotation, isRhombus: false);
                    yield return new PentaCell(Level - 1, Element + 1, Rotation, isRhombus: false);
                    yield return new PentaCell(Level, Element, Rotation, isRhombus: false);
                    yield return new PentaCell(Level, Element + 1, Rotation, isRhombus: false);
                }
                else if (IsRhombus)
                {
                    // Rhombus, level is odd
                    yield return new PentaCell(Level - 1, Element + (Level >> 1), Rotation, isRhombus: false);
                    yield return new PentaCell(Level - 1, (Element + (Level >> 1) + 1) % (Level - 1), (Rotation + (Element + (Level >> 1) + 1) / (Level - 1)) % 5, isRhombus: false);
                    yield return new PentaCell(Level, Element + (Level >> 1) + 1, Rotation, isRhombus: false);
                    yield return new PentaCell(Level, (Element + (Level >> 1) + 2) % Level, (Rotation + (Element + (Level >> 1) + 2) / Level) % 5, isRhombus: false);
                }
                else
                    throw new InvalidOperationException();
            }
        }

        readonly IEnumerable<object> INeighbor<object>.Neighbors => Neighbors.Cast<object>();

        /// <summary>
        ///     Returns a set of <see cref="PentaCell"/> cells that form a symmetric structure (actually a decagon if
        ///     <paramref name="levels"/> is large enough).</summary>
        /// <param name="levels">
        ///     The number of levels to generate.</param>
        public static IEnumerable<PentaCell> Structure(int levels)
        {
            yield return new PentaCell(0, 0, 0, isRhombus: false);
            for (var level = 1; level < levels; level++)
                for (var rotation = 0; rotation < 5; rotation++)
                {
                    for (var element = 0; element < level; element++)
                        yield return new PentaCell(level, element, rotation, isRhombus: false);
                    for (var element = 0; element < level >> 1; element++)
                        yield return new PentaCell(level, element, rotation, isRhombus: true);
                }
        }

        /// <inheritdoc/>
        public override readonly string ToString() => string.Format("{0}({1},{2},{3})", IsRhombus ? "e" : "E", Level, Element, Rotation);

        /// <summary>Describes a 2D grid of pentagonal cells with thin rhombuses filling the gaps.</summary>
        public class Grid : Structure<PentaCell>
        {
            /// <summary>
            ///     See <see cref="Structure{TCell}.Structure(IEnumerable{TCell}, IEnumerable{Link{TCell}}, Func{TCell,
            ///     IEnumerable{TCell}})"/>.</summary>
            public Grid(IEnumerable<PentaCell> cells, IEnumerable<Link<PentaCell>> links = null, Func<PentaCell, IEnumerable<PentaCell>> getNeighbors = null)
                : base(cells, links, getNeighbors)
            {
            }

            /// <summary>
            ///     Constructs a <see cref="Grid"/> that forms a symmetric structure (actually a decagon if <paramref
            ///     name="levels"/> is large enough).</summary>
            /// <param name="levels">
            ///     The number of levels to generate.</param>
            public Grid(int levels)
                : base(Structure(levels))
            {
            }

            /// <inheritdoc/>
            protected override Structure<PentaCell> makeModifiedStructure(IEnumerable<PentaCell> cells, IEnumerable<Link<PentaCell>> traversible) => new Grid(cells, traversible);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Random, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Random rnd = null, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rnd, bias);

            /// <summary>See <see cref="Structure{TCell}.GenerateMaze(Func{int, int, int}, MazeBias)"/>.</summary>
            public new Grid GenerateMaze(Func<int, int, int> rndNext, MazeBias bias = MazeBias.Default) => (Grid) base.GenerateMaze(rndNext, bias);
        }
    }
}
