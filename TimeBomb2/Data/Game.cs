using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeBomb2.Data
{
    public class Game
    {
        public Guid GameId { get; set; } = new Guid();
        public List<Player> Players { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool IsStarted { get; set; } = false;

        public bool IsRunning()
        {
            return IsStarted
                   && !IsBombExploded()
                   && !AreAllSuccessCardsRevealed();
        }

        public bool IsFinished()
        {
            return IsStarted 
                   && (IsBombExploded() || AreAllSuccessCardsRevealed());
        }

        public RoleCard? GetWinner()
        {
            if (IsFinished())
            {
                if (IsBombExploded())
                {
                    return RoleCard.Terrorist;
                }

                if (AreAllSuccessCardsRevealed())
                {
                    return RoleCard.Swat;
                }
            }

            return null;
        }

        public bool PlayerHoldsNipper(Guid playerId)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId)?.HoldsNipper == true;
        }

        public bool PlayerHasRemainingHiddenCards(string playerName)
        {
            return Players.FirstOrDefault(p => p.Name == playerName)?.HiddenPlayCards?.Count > 0;
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

        private bool IsBombExploded()
        {
            return RevealedPlayCards.Count >= 4 * Players.Count
                   || RevealedPlayCards.Any(p => p.PlayCard == PlayCard.Bomb);
        }

        private bool AreAllSuccessCardsRevealed()
        {
            return RevealedPlayCards.Count(p => p.PlayCard == PlayCard.Success) >= Players.Count;
        }
    }
}