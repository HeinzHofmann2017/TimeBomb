using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeBomb.Data;

namespace TimeBomb2.Data
{
    public class Game
    {
        public Guid GameId { get; set; } = new Guid();
        public List<Player> Players { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool Started { get; set; } = false;

        public bool IsRunning()
        {
            return Started
                   && RevealedPlayCards.Count < 4 * Players.Count
                   && RevealedPlayCards.All(p => p.Card != PlayCard.Bomb)
                   && RevealedPlayCards.Select(p => p.Card == PlayCard.Success).Count() < Players.Count;
        }

        public bool PlayerHoldsNipper(Guid playerId)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId)?.HoldsNipper == true;
        }

        public bool PlayerHasRemainingHiddenCards(string playerName)
        {
            return Players.FirstOrDefault(p => p.Name == playerName)?.HiddenPlayCards.Count > 0;
        }

        public bool PlayerIdMatchesWithName(Guid playerId, string playerName)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId)?.Name == playerName;
        }

        public bool NewRoundStartsNow()
        {
            return RevealedPlayCards.Count % Players.Count == 0;
        }
        
        public int GetGameRound()
        {
            var nrOfPlayers = Players.Count;
            var nrOfRevealedCards = RevealedPlayCards.Count;
            if (nrOfRevealedCards >= 0 * nrOfPlayers
                && nrOfRevealedCards < 1 * nrOfPlayers)
                return 1;
            if (nrOfRevealedCards >= 1 * nrOfPlayers
                && nrOfRevealedCards < 2 * nrOfPlayers)
                return 2;
            if (nrOfRevealedCards >= 2 * nrOfPlayers
                && nrOfRevealedCards < 3 * nrOfPlayers)
                return 3;
            if (nrOfRevealedCards >= 3 * nrOfPlayers
                && nrOfRevealedCards < 4 * nrOfPlayers)
                return 4;
            if (nrOfRevealedCards == 4 * nrOfPlayers)
                return 5;
            throw new ArgumentOutOfRangeException(
                $"With {nrOfPlayers} Players and {nrOfRevealedCards} Revealed Cards, no round can be calculated.");
        }

        public bool IsItTimeToMixCardsAgain()
        {
            // Todo: Implement Method
        }
    }
}