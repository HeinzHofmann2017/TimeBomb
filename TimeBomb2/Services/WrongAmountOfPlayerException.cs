using System;

namespace TimeBomb2.Services
{
    public class WrongAmountOfPlayerException : Exception
    {
        public WrongAmountOfPlayerException(string message)
            : base(message)
        {
        }
    }
}