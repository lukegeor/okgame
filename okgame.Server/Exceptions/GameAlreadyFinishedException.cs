using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class GameAlreadyFinishedException : Exception
    {
        public GameAlreadyFinishedException()
        {
        }

        public GameAlreadyFinishedException(string message) : base(message)
        {
        }

        public GameAlreadyFinishedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameAlreadyFinishedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}