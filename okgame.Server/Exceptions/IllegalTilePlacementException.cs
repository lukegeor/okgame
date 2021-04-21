using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class IllegalTilePlacementException : Exception
    {
        public IllegalTilePlacementException()
        {
        }

        public IllegalTilePlacementException(string message) : base(message)
        {
        }

        public IllegalTilePlacementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalTilePlacementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}