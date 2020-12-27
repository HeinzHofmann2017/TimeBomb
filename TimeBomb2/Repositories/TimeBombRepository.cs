using System;
using System.Collections.Generic;
using TimeBomb2.Data;
using TimeBomb2.Data.Access;

namespace TimeBomb2.Repositories
{
    public static class TimeBombRepository
    {
        public static Guid CreateGameAndGetItsId()
        { 
            var game = new Game()
            {
                GameId = Guid.NewGuid()
            };
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            session.Store(game, game.GameId.ToString());
            session.SaveChanges();
            return game.GameId;
        }

        public static Game GetGameById(Guid gameId)
        {
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            return session.Load<Game>(gameId.ToString());
        }

        public static bool GameExists(Guid gameId)
        {
            var game = GetGameById(gameId);
            return game != null;
        }

        public static Game UpdateGame(Guid gameId, List<Player> players, List<RevealedPlayCard> revealedCards, bool? started)
        {
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            var existingGame = session.Load<Game>(gameId.ToString());

            if (players != null)
            {
                existingGame.Players = players;
            }

            if (revealedCards != null)
            {
                existingGame.RevealedPlayCards = revealedCards;   
            }
            
            if (started != null)
            {
                existingGame.IsStarted = (bool)started;
            }
            
            session.SaveChanges();
            return existingGame;
        }
    }
}