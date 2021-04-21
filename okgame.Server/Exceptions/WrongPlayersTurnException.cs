using System;
using System.Runtime.Serialization;

namespace okgame.Server.Exceptions
{
    [Serializable]
    internal class WrongPlayersTurnException : Exception
    {
        public WrongPlayersTurnException()
        {
        }

        public WrongPlayersTurnException(string message) : base(message)
        {
        }

        public WrongPlayersTurnException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongPlayersTurnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}