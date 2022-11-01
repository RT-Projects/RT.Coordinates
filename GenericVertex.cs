namespace RT.Coordinates
{
    /// <summary>Represents a vertex with an explicitly specified X and Y coordinate.</summary>
    public class GenericVertex : Vertex
    {
        private readonly object _baseObject;
        private readonly PointD _point;

        /// <inheritdoc/>
        public override PointD Point => _point;

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="baseObject">
        ///     A base object used for equality comparison. Note that differing vertices must use different objects here.</param>
        /// <param name="point">
        ///     The coordinates of this vertex.</param>
        public GenericVertex(object baseObject, PointD point)
        {
            _baseObject = baseObject;
            _point = point;
        }

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is GenericVertex gen && gen._baseObject.Equals(_baseObject);
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is GenericVertex gen && gen._baseObject.Equals(_baseObject);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(_baseObject.GetHashCode() + 3);
    }
}
