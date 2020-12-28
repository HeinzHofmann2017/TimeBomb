using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using TimeBomb2.Data;
using TimeBomb2.Repositories;
using TimeBomb2.Services;

namespace TimeBomb2UnitTests.Services
{
    [TestClass]
    public class TimeBombServiceTests
    {
        [TestMethod]
        public void PlayerWithThisNameAlreadyExists_RegisterNewPlayer_ReturnsNull()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerName = "TestName";
            var game = new Game
            {
                IsStarted = false,
                GameId = gameId,
                Players = new List<Player>
                {
                    new Player
                    {
                        Name = playerName
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock.Setup(m => m.GetGameById(It.IsAny<Guid>())).Returns(game);

            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var createdGame = timeBombService.RegisterNewPlayer(gameId, playerName);

            // Assert
            createdGame.ShouldBeNull();
        }

        [TestMethod]
        public void GameAlreadyStarted_RegisterNewPlayer_ReturnsNull()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                IsStarted = true,
                GameId = gameId
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock.Setup(m => m.GetGameById(It.IsAny<Guid>())).Returns(game);

            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var createdGame = timeBombService.RegisterNewPlayer(gameId, "TestName");

            // Assert
            createdGame.ShouldBeNull();
        }

        [TestMethod]
        public void TooManyPlayersRegistered_RegisterNewPlayer_ReturnsNull()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                IsStarted = false,
                GameId = gameId,
                Players = new List<Player>
                {
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player()
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock.Setup(m => m.GetGameById(It.IsAny<Guid>())).Returns(game);

            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var createdGame = timeBombService.RegisterNewPlayer(gameId, "TestName");

            // Assert
            createdGame.ShouldBeNull();
        }

        [TestMethod]
        public void EverythingFine_RegisterNewPlayer_ReceivePlayerSpecificGameDto()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerName = "TestName";
            var game = new Game
            {
                IsStarted = false,
                GameId = gameId,
                Players = new List<Player>()
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(It.IsAny<Guid>()))
                .Returns(game);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(gameId, It.Is<List<Player>>(y => y[0].Name == playerName), null, null))
                .Returns(game);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(It.IsAny<Guid>(), It.Is<List<Player>>(y => y[0].Name != playerName), null,
                    null))
                .Returns((Game) null);

            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var createdGame = timeBombService.RegisterNewPlayer(gameId, playerName);

            // Assert
            createdGame.ShouldNotBeNull();
        }

        [TestMethod]
        public void PlayersRegistered3_StartGame_ShouldThrowException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var game = new Game
            {
                GameId = gameId,
                IsStarted = false,
                Players = new List<Player>
                {
                    new Player(),
                    new Player(),
                    new Player
                    {
                        PlayerId = playerId
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act / Assert
            Should.Throw<WrongAmountOfPlayerException>(() => timeBombService.StartGame(gameId, playerId));
        }

        [TestMethod]
        public void PlayersRegistered7_StartGame_ShouldThrowException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var game = new Game
            {
                GameId = gameId,
                IsStarted = false,
                Players = new List<Player>
                {
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player
                    {
                        PlayerId = playerId
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act / Assert
            Should.Throw<WrongAmountOfPlayerException>(() => timeBombService.StartGame(gameId, playerId));
        }

        [TestMethod]
        public void PlayersRegistered6_StartGame_ShouldReturnValidStartedGame()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var game1 = new Game
            {
                GameId = gameId,
                IsStarted = false,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>
                {
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player(),
                    new Player
                    {
                        PlayerId = playerId
                    }
                }
            };
            
            var game2 = new Game
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>
                {
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        PlayerId = playerId,
                        HiddenPlayCards = new List<PlayCard>()
                    }
                }
            };

            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game1);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(gameId, It.IsAny<List<Player>>(), It.IsAny<List<RevealedPlayCard>>(), true))
                .Returns(game2);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var startedGame = timeBombService.StartGame(gameId, playerId);

            // Assert
            startedGame.IsStarted.ShouldBeTrue();
            timeBombRepositoryMock.Verify(m => m.UpdateGame(gameId,
                It.Is<List<Player>>(p => p[0].HiddenPlayCards.Count == 5),
                It.Is<List<RevealedPlayCard>>(l => l.Count == 0),
                true), 
                Times.Once);
        }

        [TestMethod]
        public void AlreadyStarted_StartGame_ShouldReturnValidStartedGame()
        {
            // Arrange
            // Arrange
            var gameId = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var game = new Game
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>
                {
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player
                    {
                        PlayerId = playerId,
                        HiddenPlayCards = new List<PlayCard>()
                    }
                }
            };

            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act
            var startedGame = timeBombService.StartGame(gameId, playerId);
            
            // Assert
            startedGame.IsStarted.ShouldBeTrue();
            startedGame.IsFinished.ShouldBeFalse();
            timeBombRepositoryMock.Verify(
                m => m.UpdateGame(gameId, 
                    It.IsAny<List<Player>>(),
                    It.IsAny<List<RevealedPlayCard>>(),
                    It.IsAny<bool?>()),
                Times.Never);

        }
    }
}