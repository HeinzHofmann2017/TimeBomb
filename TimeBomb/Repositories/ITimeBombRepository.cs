using System;
using System.Collections.Generic;
using TimeBomb.Data;

namespace TimeBomb.Repositories
{
    public interface ITimeBombRepository
    {
        public Guid CreateGameAndGetItsId();
        public Game GetGameById(Guid gameId);
        public bool GameExists(Guid gameId);
        public Game UpdateGame(Guid gameId, List<Player> players, List<RevealedPlayCard> revealedCards, bool? started);
    }
}