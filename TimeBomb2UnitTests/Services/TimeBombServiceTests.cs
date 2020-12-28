using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeBomb2.Repositories;

namespace TimeBomb2UnitTests.Services
{
    [TestClass]
    public class TimeBombServiceTests
    {
        [TestMethod]
        public void TooManyPlayersRegistered_RegisterNewPlayer_ReturnsNull()
        {
            // Arrange
            var timeBombRepositoryMock = new Mock<TimeBombRepository>();
        }
        
    }
}