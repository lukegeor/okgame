using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class NoTilesLeftException : Exception
    {
        public NoTilesLeftException()
        {
        }

        public NoTilesLeftException(string message) : base(message)
        {
        }

        public NoTilesLeftException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoTilesLeftException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}