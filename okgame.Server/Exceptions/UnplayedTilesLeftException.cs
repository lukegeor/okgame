using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class UnplayedTilesLeftException : Exception
    {
        public UnplayedTilesLeftException()
        {
        }

        public UnplayedTilesLeftException(string message) : base(message)
        {
        }

        public UnplayedTilesLeftException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnplayedTilesLeftException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}