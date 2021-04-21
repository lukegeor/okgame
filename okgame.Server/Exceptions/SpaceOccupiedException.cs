using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class SpaceOccupiedException : Exception
    {
        public SpaceOccupiedException()
        {
        }

        public SpaceOccupiedException(string message) : base(message)
        {
        }

        public SpaceOccupiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SpaceOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}