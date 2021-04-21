using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class MoveTileToSameSpaceException : Exception
    {
        public MoveTileToSameSpaceException()
        {
        }

        public MoveTileToSameSpaceException(string message) : base(message)
        {
        }

        public MoveTileToSameSpaceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoveTileToSameSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}