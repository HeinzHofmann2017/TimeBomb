using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeBomb.Data
{
    public class Game
    {
        public Guid GameId { get; set; } = new Guid();
        public List<Player> Players { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool Started { get; set; } = false;
        public bool Ended { get; set; } = false;

        public bool IsRunning()
        {
            return Started == true && Ended == false;
        }

        public bool PlayerHoldsNipper(Guid playerId)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId)?.HoldsNipper == true;
        }

        public bool PlayerHasRemainingHiddenCards(string playerName)
        {
            return Players.FirstOrDefault(p => p.Name == playerName)?.HiddenPlayCards.Count > 0;
        }
    }
}