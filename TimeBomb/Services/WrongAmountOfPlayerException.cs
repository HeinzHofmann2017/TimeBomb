using System;

namespace TimeBomb.Services
{
    public class WrongAmountOfPlayerException : Exception
    {
        public WrongAmountOfPlayerException(string message)
            : base(message)
        {
        }
    }
}