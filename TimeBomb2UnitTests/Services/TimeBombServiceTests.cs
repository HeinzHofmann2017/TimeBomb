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
            var game = new Game()
            {
                IsStarted = false,
                GameId = gameId,
                Players = new List<Player>()
                {
                    new Player()
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
            var game = new Game()
            {
                IsStarted = true,
                GameId = gameId,
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
            var game = new Game()
            {
                IsStarted = false,
                GameId = gameId,
                Players = new List<Player>()
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
        
    }
}