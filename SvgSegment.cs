using System.Collections.Generic;

namespace RT.Coordinates;

internal struct SvgSegment(List<Vertex> vertices, bool closed)
{
    public List<Vertex> Vertices = vertices;
    public bool Closed = closed;

    public override readonly bool Equals(object obj) => obj is SvgSegment other && EqualityComparer<List<Vertex>>.Default.Equals(Vertices, other.Vertices) && Closed == other.Closed;

    public override readonly int GetHashCode()
    {
        var hashCode = 574668419;
        hashCode = hashCode * -1521134295 + EqualityComparer<List<Vertex>>.Default.GetHashCode(Vertices);
        hashCode = hashCode * -1521134295 + Closed.GetHashCode();
        return hashCode;
    }

    public readonly void Deconstruct(out List<Vertex> vertices, out bool closed)
    {
        vertices = Vertices;
        closed = Closed;
    }
}
