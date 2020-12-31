using System;

namespace TimeBomb.Services
{
    public class NotAllowedMoveException : Exception
    {
        public NotAllowedMoveException(string message)
            : base(message)
        {
        }
    }
}