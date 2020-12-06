using System.Collections.Generic;
using TimeBomb.Data;

namespace TimeBomb2.Data.Dtos
{
    public class OtherPlayerDto
    {
        public string Name { get; set; }
        public bool HoldsNipper { get; set; }
        public int NumberOfHiddenPlayCards { get; set; }
    }
}