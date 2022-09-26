namespace RT.Coordinates
{
    /// <summary>Represents a vertex with an explicitly specified X and Y coordinate.</summary>
    public class GenericVertex : Vertex
    {
        private readonly object _baseObject;
        private readonly double _x, _y;

        /// <inheritdoc/>
        public override double X => _x;
        /// <inheritdoc/>
        public override double Y => _y;

        /// <summary>
        ///     Constructor.</summary>
        /// <param name="baseObject">
        ///     A base object used for equality comparison. Note that differing vertices must use different objects here.</param>
        /// <param name="x">
        ///     The X coordinate of this vertex.</param>
        /// <param name="y">
        ///     The Y coordinate of this vertex.</param>
        public GenericVertex(object baseObject, double x, double y)
        {
            _baseObject = baseObject;
            _x = x;
            _y = y;
        }

        /// <inheritdoc/>
        public override bool Equals(Vertex other) => other is GenericVertex gen && gen._baseObject.Equals(_baseObject);
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is GenericVertex gen && gen._baseObject.Equals(_baseObject);
        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(_baseObject.GetHashCode() + 3);
    }
}
