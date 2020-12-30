using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Resources;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TimeBomb2.Data;

namespace TimeBomb2UnitTests.Data
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void TimeIsUp_IsFinished_ReturnsTrueWinnerIsTerrorist()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Safe),
                    CreateRevealedPlayCard(4, PlayCard.Safe),
                    CreateRevealedPlayCard(4, PlayCard.Safe),
                    CreateRevealedPlayCard(4, PlayCard.Safe),
                    CreateRevealedPlayCard(4, PlayCard.Safe)
                },
                IsStarted = true
            };
            
            // Act
            var isFinished = game.IsFinished();
            var winner = game.GetWinner();
            var isRunning = game.IsRunning();

            // Assert
            isFinished.ShouldBeTrue();
            winner.ShouldBe(RoleCard.Terrorist);
            isRunning.ShouldBeFalse();
        }

        [TestMethod]
        public void BombIsRevealed_IsFinished_ReturnsTrueWinnerIsTerrorist()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Bomb)
                },
                IsStarted = true
            };
            
            // Act
            var isFinished = game.IsFinished();
            var winner = game.GetWinner();
            var isRunning = game.IsRunning();
            
            // Assert
            isFinished.ShouldBeTrue();
            winner.ShouldBe(RoleCard.Terrorist);
            isRunning.ShouldBeFalse();
        }

        [TestMethod]
        public void AllSuccessCardsAreRevealedWith4Players_IsFinished_ReturnsTrueWinnerIsSWAT()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(3, PlayCard.Success)
                },
                IsStarted = true
            };
            
            // Act
            var isFinished = game.IsFinished();
            var winner = game.GetWinner();
            var isRunning = game.IsRunning();
            
            // Assert
            isFinished.ShouldBeTrue();
            winner.ShouldBe(RoleCard.Swat);
            isRunning.ShouldBeFalse();
        }

        [TestMethod]
        public void AllSuccessCardsAreRevealedWith6Players_IsFinished_ReturnsTrueWinnerIsSWAT()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(3, PlayCard.Success)
                },
                IsStarted = true
            };
            
            // Act
            var isFinished = game.IsFinished();
            var winner = game.GetWinner();
            var isRunning = game.IsRunning();
            
            // Assert
            isFinished.ShouldBeTrue();
            winner.ShouldBe(RoleCard.Swat);
            isRunning.ShouldBeFalse();
        }

        [TestMethod]
        public void NotAllSuccessCardsAreYetRevealed_IsFinished_ReturnsFalseWinnerIsNull()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Safe),
                    CreateRevealedPlayCard(2, PlayCard.Success),
                    CreateRevealedPlayCard(2, PlayCard.Success)
                },
                IsStarted = true
            };
            
            // Act
            var isFinished = game.IsFinished();
            var winner = game.GetWinner();
            var isRunning = game.IsRunning();
            
            // Assert
            isFinished.ShouldBeFalse();
            winner.ShouldBeNull();
            isRunning.ShouldBeTrue();
        }
        
        [TestMethod]
        public void RevealedCards3Players4_GetGameRound_ShouldReturn1()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                },
                IsStarted = true
            };
            
            // Act
            var gameRound = game.GetGameRound();
            var newRoundStartsNow = game.NewRoundStartsNow();
            
            // Assert
            gameRound.ShouldBe(1);
            newRoundStartsNow.ShouldBeFalse();
        }
        
        [TestMethod]
        public void RevealedCards4Players4_GetGameRound_ShouldReturn2()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe)
                },
                IsStarted = true
            };
            
            // Act
            var gameRound = game.GetGameRound();
            var newRoundStartsNow = game.NewRoundStartsNow();
            
            // Assert
            gameRound.ShouldBe(2);
            newRoundStartsNow.ShouldBeTrue();
        }
        
        [TestMethod]
        public void RevealedCards5Players6_GetGameRound_ShouldReturn1()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                },
                IsStarted = true
            };
            
            // Act
            var gameRound = game.GetGameRound();
            var newRoundStartsNow = game.NewRoundStartsNow();
            
            // Assert
            gameRound.ShouldBe(1);
            newRoundStartsNow.ShouldBeFalse();
        }
        
        [TestMethod]
        public void RevealedCards23Players6_GetGameRound_ShouldReturn4()
        {
            // Arrange
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer(), CreateRandomPlayer()
                    },
                RevealedPlayCards = new List<RevealedPlayCard>()
                {
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                    CreateRevealedPlayCard(1, PlayCard.Safe),
                },
                IsStarted = true
            };
            
            // Act
            var gameRound = game.GetGameRound();
            var newRoundStartsNow = game.NewRoundStartsNow();
            
            // Assert
            gameRound.ShouldBe(4);
            newRoundStartsNow.ShouldBeFalse();
        }

        [TestMethod]
        public void PlayerWithName_PlayerIdMatchesWithName_ReturnsTrue()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            const string name = "TestName";
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        new Player
                        {
                            PlayerId = playerId,
                            Name = name
                        }
                    }
            };
            
            // Act
            var nameAndIdMatched = game.PlayerIdMatchesWithName(playerId, name);
            
            // Assert
            nameAndIdMatched.ShouldBeTrue();
        }

        [TestMethod]
        public void PlayerWithWrongName_PlayerIdMatchesWithName_ReturnsFalse()
        {
            // Arrange
            var playerId1 = Guid.NewGuid();
            const string name1 = "TestName1";
            var playerId2 = Guid.NewGuid();
            const string name2 = "TestName2";
            
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        new Player
                        {
                            PlayerId = playerId1,
                            Name = name1
                        },
                        new Player
                        {
                            PlayerId = playerId2,
                            Name = name2
                        }
                    }
            };
            
            // Act
            var nameAndIdMatched1 = game.PlayerIdMatchesWithName(playerId1, name2);
            var nameAndIdMatched2 = game.PlayerIdMatchesWithName(playerId2, name1);
            
            // Assert
            nameAndIdMatched1.ShouldBeFalse();
            nameAndIdMatched2.ShouldBeFalse();
        }

        [TestMethod]
        public void PlayerHasRemainingHiddenCards_PlayerHasRemainingHiddenCards_ReturnsTrue()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            const string name = "TestName";
            
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        new Player
                        {
                            PlayerId = playerId,
                            Name = name,
                            HiddenPlayCards = new List<PlayCard>()
                            {
                                PlayCard.Safe,
                                PlayCard.Success
                            }
                        }
                    }
            };
            
            // Act
            var playerHasRemainingHiddenPlayCards = game.PlayerHasRemainingHiddenCards(name);
            
            // Assert
            playerHasRemainingHiddenPlayCards.ShouldBeTrue();
        }

        [TestMethod]
        public void PlayerHasNoRemainingHiddenCards_PlayerHasRemainingHiddenCards_ReturnsFalse()
        {
            // Arrange
            const string name1 = "TestName1";
            const string name2 = "TestName2";
            
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        new Player
                        {
                            PlayerId = Guid.NewGuid(),
                            Name = name1,
                            HiddenPlayCards = new List<PlayCard>()
                        },
                        new Player
                        {
                            PlayerId = Guid.NewGuid(),
                            Name = name2
                        }
                    }
            };
            
            // Act
            var playerHasRemainingHiddenPlayCards1 = game.PlayerHasRemainingHiddenCards(name1);
            var playerHasRemainingHiddenPlayCards2 = game.PlayerHasRemainingHiddenCards(name2);
            
            // Assert
            playerHasRemainingHiddenPlayCards1.ShouldBeFalse();
            playerHasRemainingHiddenPlayCards2.ShouldBeFalse();
        }

        [TestMethod]
        public void OnePlayerHoldsNipperAnotherNot_PlayerHoldsNipper_ReturnsTrueForHoldingPlayerAndFalseForTheOtherOne()
        {
            // Arrange
            var playerId1 = Guid.NewGuid();
            var playerId2 = Guid.NewGuid();
            const string name1 = "TestName1";
            const string name2 = "TestName2";
            
            var game = new Game
            {
                GameId = Guid.NewGuid(),
                Players =
                    new List<Player>()
                    {
                        new Player
                        {
                            PlayerId = playerId1,
                            Name = name1,
                            HoldsNipper = true
                        },
                        new Player
                        {
                            PlayerId = playerId2,
                            Name = name2,
                            HoldsNipper = false
                        }
                    }
            };
            
            // Act
            var player1HoldsNipper = game.PlayerHoldsNipper(playerId1);
            var player2HoldsNipper = game.PlayerHoldsNipper(playerId2);
            
            // Assert
            player1HoldsNipper.ShouldBeTrue();
            player2HoldsNipper.ShouldBeFalse();
        }

        private static Player CreateRandomPlayer()
        {
            return new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = GetRandomString(10)
            };
        }

        private static RevealedPlayCard CreateRevealedPlayCard(int round, PlayCard card)
        {
            return new RevealedPlayCard()
            {
                PlayCard = card,
                NameOfPlayerWhichHadThisCard = GetRandomString(10),
                Round = round
            };
        }

        private static string GetRandomString(int size)
        {
            var rnd = new Random();
            var builder = new StringBuilder(size);
            for (var i = 0; i < size; i++)
            {
                var @char = (char) rnd.Next(33, 126);
                builder.Append(@char);
            }

            return builder.ToString();
        }
    }
}