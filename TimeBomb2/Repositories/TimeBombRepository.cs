using System;
using TimeBomb.Data;
using TimeBomb2.Data.Access;

namespace TimeBomb2.Repositories
{
    public class TimeBombRepository
    {
        public Guid CreateGameAndGetItsId()
        {
            var game = new Game()
            {
                GameId = Guid.NewGuid()
            };
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            session.Store(game);
            return game.GameId;
        }
    }
}