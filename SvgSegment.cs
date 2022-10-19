using System.Collections.Generic;

namespace RT.Coordinates
{
    internal struct SvgSegment
    {
        public List<Vertex> Vertices;
        public bool Closed;

        public SvgSegment(List<Vertex> vertices, bool closed)
        {
            Vertices = vertices;
            Closed = closed;
        }

        public override bool Equals(object obj) => obj is SvgSegment other && EqualityComparer<List<Vertex>>.Default.Equals(Vertices, other.Vertices) && Closed == other.Closed;

        public override int GetHashCode()
        {
            var hashCode = 574668419;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Vertex>>.Default.GetHashCode(Vertices);
            hashCode = hashCode * -1521134295 + Closed.GetHashCode();
            return hashCode;
        }

        public void Deconstruct(out List<Vertex> vertices, out bool closed)
        {
            vertices = this.Vertices;
            closed = this.Closed;
        }
    }
}
