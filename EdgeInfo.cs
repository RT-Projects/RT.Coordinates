using System.Collections.Generic;

namespace RT.Coordinates;

/// <summary>
///     Encapsulates information returned by <see cref="Structure{TCell}.svgEdgeType(Link{Vertex}, List{TCell})"/>.</summary>
/// <typeparam name="TCell">
///     The type of cell used by the structure.</typeparam>
public struct EdgeInfo<TCell>
{
    /// <summary>The type of edge to use when drawing an edge in SVG.</summary>
    public EdgeType EdgeType;
    /// <summary>
    ///     Contains a cell adjacent to the edge. If <see cref="EdgeType"/> is <see cref="EdgeType.Outline"/>, this is the
    ///     only cell used.</summary>
    public TCell Cell1;
    /// <summary>
    ///     Contains the cell on the opposite side of the edge from <see cref="Cell1"/>. If <see cref="EdgeType"/> is <see
    ///     cref="EdgeType.Outline"/>, this field is unused.</summary>
    public TCell Cell2;

    /// <summary>Deconstructor.</summary>
    public readonly void Deconstruct(out EdgeType edgeType, out TCell cell1, out TCell cell2)
    {
        edgeType = EdgeType;
        cell1 = Cell1;
        cell2 = Cell2;
    }
}
