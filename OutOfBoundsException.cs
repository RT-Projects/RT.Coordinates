using System;
using System.Runtime.Serialization;

namespace RT.Coordinates
{
    [Serializable]
    internal class OutOfBoundsException : Exception
    {
        public OutOfBoundsException() : this("An attempt was made to move beyond the edge of the grid.")
        {
        }

        public OutOfBoundsException(string message) : base(message)
        {
        }

        public OutOfBoundsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfBoundsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}