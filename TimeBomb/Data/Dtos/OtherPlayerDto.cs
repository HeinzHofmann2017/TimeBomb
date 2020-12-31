using System.Collections.Generic;

namespace TimeBomb.Data.Dtos
{
    public class OtherPlayerDto
    {
        public OtherPlayerDto(Player player, bool isGameFinished)
        {
            Name = player.Name;
            HoldsNipper = player.HoldsNipper;
            NumberOfHiddenPlayCards = player.HiddenPlayCards?.Count ?? 0;
            if (isGameFinished)
            {
                RoleCard = player.RoleCard;
            }
            else
            {
                RoleCard = null;
            }
        }
        
        public string Name { get; set; }
        public bool HoldsNipper { get; set; }
        public int NumberOfHiddenPlayCards { get; set; }
        public RoleCard? RoleCard { get; set; }
    }
}