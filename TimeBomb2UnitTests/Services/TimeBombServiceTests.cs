using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
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
        public void PlayerWithThisNameAlreadyExists_RegisterNewPlayer_ThrowsException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            const string playerName = "TestName";
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

            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.RegisterNewPlayer(gameId, playerName))
                .Message.ShouldBe("A Player with this Name already exists for this game. Please choose another name");
        }

        [TestMethod]
        public void GameAlreadyStarted_RegisterNewPlayer_ThrowsException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                IsStarted = true,
                GameId = gameId,
                Players = new List<Player>()
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock.Setup(m => m.GetGameById(It.IsAny<Guid>())).Returns(game);

            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.RegisterNewPlayer(gameId, "TestName"))
                .Message.ShouldBe("Joining this game isn't allowed anymore, because it already started");
        }

        [TestMethod]
        public void TooManyPlayersRegistered_RegisterNewPlayer_ThrowsException()
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

            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.RegisterNewPlayer(gameId, "TestName"))
                .Message.ShouldBe("Already the maximum of 6 Players joined this game. Therefore no more Players are allowed");
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

        [TestMethod]
        public void NotNipperHoldingPlayerWantsToNipCard_NipCard_ShouldReturnException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        },
                        HoldsNipper = true
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = false
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName))
                .Message.ShouldBe("This Player doesn't hold the nipper right now, so he isn't allowed to nip anybody.");
        }

        [TestMethod]
        public void GameDidntStartYet_NipCard_ShouldReturnException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game = new Game()
            {
                GameId = gameId,
                IsStarted = false,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName))
                .Message.ShouldBe("Game didn't start yet, so this move isn't allowed yet.");
        }

        [TestMethod]
        public void GameAlreadyFinished_NipCard_ShouldReturnException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Bomb,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1
                    }
                },
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName))
                .Message.ShouldBe("Game is already finished (either bomb exploded, all success-cards are revealed or 4 rounds are fully played), so nipping cards isn't allowed anymore.");
        }

        [TestMethod]
        public void PlayerWantsToNipHimself_NipCard_ShouldReturnException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName))
                .Message.ShouldBe("Player tries to nip himself. This isn't allowed in this game");
        }

        [TestMethod]
        public void ToBeNippedPlayerDoesntHaveRemainingHiddenCardsAnymore_NipCard_ShouldReturnException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);
            
            // Act / Assert
            Should.Throw<NotAllowedMoveException>(() => timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName))
                .Message.ShouldBe("To be nipped player doesn't have any hidden cards anymore, so nipping this player isn't allowed.");
        }

        [TestMethod]
        public void NipperHoldingPlayerNipsCard_NipCard_ReturnsValidDto()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game1 = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>(),
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var game2 = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        Round = 1,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName
                    }
                },
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success,
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game1);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(
                    gameId,
                    It.IsAny<List<Player>>(),
                    It.IsAny<List<RevealedPlayCard>>(),
                    null))
                .Returns(game2);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var nippedGameDto = timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName);
            
            // Assert
            timeBombRepositoryMock.Verify(m => m.UpdateGame(
                gameId, 
                It.Is<List<Player>>(l => l.Any(p => p.Name == toBeNippedPlayerName && p.HiddenPlayCards.Count == 4)),
                It.Is<List<RevealedPlayCard>>(l => l.Any(r => r.PlayCard == PlayCard.Success)),
                null),
                Times.Once);
            nippedGameDto.OtherPlayers.Single(p => p.Name == toBeNippedPlayerName).NumberOfHiddenPlayCards.ShouldBe(4);
            nippedGameDto.RevealedPlayCards.Count.ShouldBe(1);
            nippedGameDto.RevealedPlayCards.Any(c => c.PlayCard == PlayCard.Success).ShouldBeTrue();
        }

        [TestMethod]
        public void NipperHoldingPlayerNipsLastCardOfRound_NipCard_ReturnsValidDtoAndCardsAreBeingMixed()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var nippingPlayerId = Guid.NewGuid();
            const string toBeNippedPlayerName = "ToBeNippedPlayerName";
            var game1 = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                },
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Success,
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var game2 = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        Round = 1,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                },
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            var game3 = new Game()
            {
                GameId = gameId,
                IsStarted = true,
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        Round = 1,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                    new RevealedPlayCard()
                    {
                        PlayCard = PlayCard.Success,
                        NameOfPlayerWhichHadThisCard = toBeNippedPlayerName,
                        Round = 1,
                    },
                },
                Players = new List<Player>()
                {
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        }
                    },
                    new Player()
                    {
                        Name = toBeNippedPlayerName,
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Success,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Bomb
                        }
                    },
                    new Player()
                    {
                        HiddenPlayCards = new List<PlayCard>()
                        {
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe,
                            PlayCard.Safe
                        },
                        PlayerId = nippingPlayerId,
                        HoldsNipper = true
                    }
                }
            };
            
            var timeBombRepositoryMock = new Mock<ITimeBombRepository>();
            timeBombRepositoryMock
                .Setup(m => m.GetGameById(gameId))
                .Returns(game1);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(
                    gameId,
                    It.Is<List<Player>>(l => l.Any(p => p.Name == toBeNippedPlayerName && p.HiddenPlayCards.Count == 1)),
                    It.Is<List<RevealedPlayCard>>(l => l.All(r => r.PlayCard == PlayCard.Success)),
                    null))
                .Returns(game2);
            timeBombRepositoryMock
                .Setup(m => m.UpdateGame(
                    gameId,
                    It.Is<List<Player>>(l => l.All(p => p.HiddenPlayCards.Count == 4)),
                    null,
                    null))
                .Returns(game3);
            var timeBombService = new TimeBombService(timeBombRepositoryMock.Object);

            // Act
            var nippedGameDto = timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName);
            
            // Assert
            timeBombRepositoryMock.Verify(m => m.UpdateGame(
                gameId, 
                It.Is<List<Player>>(l => l.Any(p => p.Name == toBeNippedPlayerName && p.HiddenPlayCards.Count == 1)),
                It.Is<List<RevealedPlayCard>>(l => l.All(r => r.PlayCard == PlayCard.Success)),
                null),
                Times.Once);
            timeBombRepositoryMock.Verify(m => m.UpdateGame(
                    gameId, 
                    It.Is<List<Player>>(l => l.All(p => p.HiddenPlayCards.Count == 4)),
                    null,
                    null),
                Times.Once);
            nippedGameDto.RevealedPlayCards.Count.ShouldBe(4);
            nippedGameDto.RevealedPlayCards.All(c => c.PlayCard == PlayCard.Success).ShouldBeTrue();
            nippedGameDto.IsFinished.ShouldBeTrue();
            nippedGameDto.Winner.ShouldBe(RoleCard.Swat);
            nippedGameDto.OtherPlayers.All(p => p.NumberOfHiddenPlayCards == 4).ShouldBeTrue();
        }
    }
}