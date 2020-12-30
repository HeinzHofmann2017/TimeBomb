using System.Collections.Generic;

namespace TimeBomb2.Data.Dtos
{
    public class OtherPlayerDto
    {
        public OtherPlayerDto(Player player)
        {
            Name = player.Name;
            HoldsNipper = player.HoldsNipper;
            NumberOfHiddenPlayCards = player.HiddenPlayCards?.Count ?? 0;
        }
        
        public string Name { get; set; }
        public bool HoldsNipper { get; set; }
        public int NumberOfHiddenPlayCards { get; set; }
    }
}