using System;
using TimeBomb.Data;
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

        public static void UpdateGame(Game updatedGame)
        {
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            var existingGame = session.Load<Game>(updatedGame.GameId.ToString());
            existingGame.Started = updatedGame.Started;
            existingGame.RevealedCards = updatedGame.RevealedCards;
            existingGame.Players = updatedGame.Players;
            session.SaveChanges();
        }
    }
}