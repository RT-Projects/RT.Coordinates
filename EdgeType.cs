namespace RT.Coordinates;

/// <summary>
///     Describes the type of edge (line segment between two <see cref="Vertex"/> objects) to draw in SVG when dealing with an
///     edge in a 2D structure.</summary>
public enum EdgeType
{
    /// <summary>
    ///     The edge is part of the outer perimeter of the structure. The default stroke style is black with thickness 0.1.</summary>
    Outline,
    /// <summary>
    ///     The edge is within the structure, and is passable (there is a <see cref="Link{TCell}"/> connecting the cells on
    ///     either side). The default stroke style is #ccc with thickness 0.02 and dashed.</summary>
    Passage,
    /// <summary>
    ///     The edge is within the structure, but is not passable (there is no <see cref="Link{TCell}"/> connecting the cells
    ///     on either side). The default stroke style is black with thickness 0.05.</summary>
    Wall
}
