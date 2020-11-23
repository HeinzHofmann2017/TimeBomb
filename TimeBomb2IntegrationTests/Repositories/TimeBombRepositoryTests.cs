using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TimeBomb.Data;
using TimeBomb2.Data.Access;
using TimeBomb2.Repositories;

namespace TimeBomb2IntegrationTests.Repositories
{
    [TestClass]
    public class TimeBombRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DocumentStoreHolder.CreateTimeBombDatabaseIfNotYetExists();
        }

        [TestMethod]
        public void Nothing_CreateGameAndGetItsId_GameWithThisIdIsInDataBase()
        {
            // Arrange
            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();

            // Act
            var gameId = TimeBombRepository.CreateGameAndGetItsId();

            // Assert
            Thread.Sleep(1000);
            session.Query<Game>().FirstOrDefault(g => g.GameId == gameId).ShouldNotBeNull();
        }

        [TestMethod]
        public void GameExists_GameExists_True()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            session.Store(new Game {GameId = gameId}, gameId.ToString());
            session.SaveChanges();
            Thread.Sleep(1000);

            // Act
            var doesGameExist = TimeBombRepository.GameExists(gameId);

            // Assert
            doesGameExist.ShouldBeTrue();
        }

        [TestMethod]
        public void GameDoesntExist_GameExists_False()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            // Act
            var doesGameExist = TimeBombRepository.GameExists(gameId);

            // Assert
            doesGameExist.ShouldBeFalse();
        }

        [TestMethod]
        public void GameDoesExist_GetGameById_ReceivedGame()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            var store = DocumentStoreHolder.Store;
            using var session = store.OpenSession();
            session.Store(new Game {GameId = gameId}, gameId.ToString());
            session.SaveChanges();
            Thread.Sleep(1000);

            // Act
            var game = TimeBombRepository.GetGameById(gameId);

            // Assert
            game.ShouldNotBeNull();
        }

        [TestMethod]
        public void GameDoesntExist_GetGameById_DoesntReceiveGame()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            // Act
            var game = TimeBombRepository.GetGameById(gameId);

            // Assert
            game.ShouldBeNull();
        }

        [TestMethod]
        public void OldGameInDb_UpdateGame_GameUpdated()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerId1 = Guid.NewGuid();
            var playerId2 = Guid.NewGuid();

            var store = DocumentStoreHolder.Store;
            using (var session = store.OpenSession())
            {
                session.Store(new Game {GameId = gameId}, gameId.ToString());
                session.SaveChanges();
            }

            Thread.Sleep(1000);

            var newGame = new Game
            {
                GameId = gameId,
                Players = new List<Player>
                {
                    new Player
                    {
                        PlayerId = playerId1,
                        Name = "Heinz",
                        HoldsNipper = false,
                        RoleCard = RoleCard.Swat,
                        HiddenPlayCards = new List<PlayCard>
                        {
                            PlayCard.Bomb
                        },
                        RevealedPlayCards = new List<PlayCard>
                        {
                            PlayCard.Success
                        }
                    },
                    new Player
                    {
                        PlayerId = playerId2,
                        Name = "Fabi",
                        HoldsNipper = true,
                        RoleCard = RoleCard.Terrorist,
                        HiddenPlayCards = new List<PlayCard>
                        {
                            PlayCard.Success
                        },
                        RevealedPlayCards = new List<PlayCard>
                        {
                            PlayCard.Safe
                        }
                    }
                },
                RevealedCards = new List<PlayCard>
                {
                    PlayCard.Success,
                    PlayCard.Safe,
                    PlayCard.Success
                },
                Started = true
            };

            // Act
            TimeBombRepository.UpdateGame(newGame.GameId, newGame.Players, newGame.RevealedCards, newGame.Started);

            // Assert
            Thread.Sleep(1000);
            using (var session = store.OpenSession())
            {
                var loadedGame = session.Query<Game>().FirstOrDefault(g => g.GameId == gameId).ShouldNotBeNull();
                var fabiPlayer = loadedGame.Players.FirstOrDefault(p => p.Name == "Fabi");
                var heinzPlayer = loadedGame.Players.FirstOrDefault(p => p.Name == "Heinz");
                fabiPlayer.ShouldNotBeNull();
                heinzPlayer.ShouldNotBeNull();
                fabiPlayer.HoldsNipper.ShouldBeTrue();
                heinzPlayer.HoldsNipper.ShouldBeFalse();
                fabiPlayer.RoleCard.ShouldBe(RoleCard.Terrorist);
                heinzPlayer.RoleCard.ShouldBe(RoleCard.Swat);
                
                fabiPlayer.HiddenPlayCards.ShouldContain(PlayCard.Success);
                fabiPlayer.HiddenPlayCards.ShouldNotContain(PlayCard.Bomb);
                fabiPlayer.HiddenPlayCards.ShouldNotContain(PlayCard.Safe);
                fabiPlayer.RevealedPlayCards.ShouldContain(PlayCard.Safe);
                fabiPlayer.RevealedPlayCards.ShouldNotContain(PlayCard.Bomb);
                fabiPlayer.RevealedPlayCards.ShouldNotContain(PlayCard.Success);
                heinzPlayer.HiddenPlayCards.ShouldContain(PlayCard.Bomb);
                heinzPlayer.HiddenPlayCards.ShouldNotContain(PlayCard.Success);
                heinzPlayer.HiddenPlayCards.ShouldNotContain(PlayCard.Safe);
                heinzPlayer.RevealedPlayCards.ShouldContain(PlayCard.Success);
                heinzPlayer.RevealedPlayCards.ShouldNotContain(PlayCard.Bomb);
                heinzPlayer.RevealedPlayCards.ShouldNotContain(PlayCard.Safe);
                
                loadedGame.RevealedCards.ShouldContain(PlayCard.Safe);
                loadedGame.RevealedCards.ShouldContain(PlayCard.Success);
                loadedGame.RevealedCards.ShouldNotContain(PlayCard.Bomb);

                loadedGame.Started.ShouldBeTrue();
            }
        }

        [TestMethod]
        public void OldGameInDbNoNewValues_UpdateGame_GameNotUpdated()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            var store = DocumentStoreHolder.Store;
            using (var session = store.OpenSession())
            {
                session.Store(new Game {GameId = gameId}, gameId.ToString());
                session.SaveChanges();
            }

            Thread.Sleep(1000);

            // Act
            TimeBombRepository.UpdateGame(gameId, null, null, null);

            // Assert
            Thread.Sleep(1000);
            using (var session = store.OpenSession())
            {
                var loadedGame = session.Query<Game>().FirstOrDefault(g => g.GameId == gameId).ShouldNotBeNull();
                loadedGame.Players.ShouldBeNull();
                loadedGame.Started.ShouldBeFalse();
                loadedGame.RevealedCards.ShouldBeNull();
            }
        }
    }
}