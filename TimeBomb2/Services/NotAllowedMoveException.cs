using System;

namespace TimeBomb2.Services
{
    public class NotAllowedMoveException : Exception
    {
        public NotAllowedMoveException(string message)
            : base(message)
        {
        }
    }
}