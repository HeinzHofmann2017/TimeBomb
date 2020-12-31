using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeBomb.Data.Dtos;
using TimeBomb.Repositories;
using TimeBomb.Services;

namespace TimeBomb.Controllers
{
    [ApiController]
    [Route("timebomb")]
    public class TimeBombController : ControllerBase
    {
        private readonly ILogger<TimeBombController> _logger;
        private readonly TimeBombService _timeBombService;

        public TimeBombController(ILogger<TimeBombController> logger)
        {
            _logger = logger;
            _timeBombService = new TimeBombService(new TimeBombRepository());
        }

        [HttpGet]
        [Route("creategame")]
        public Guid CreateGame()
        {
            return _timeBombService.CreateGame();
        }

        [HttpGet]
        [Route("registernewplayer")]
        public PlayerSpecificGameDto RegisterNewPlayer(Guid gameId, string name)
        {
            return _timeBombService.RegisterNewPlayer(gameId, name);
        }

        [HttpGet]
        [Route("startgame")]
        public PlayerSpecificGameDto StartGame(Guid gameId, Guid playerId)
        {
            return _timeBombService.StartGame(gameId, playerId);
        }

        [HttpGet]
        [Route("nipcard")]
        public PlayerSpecificGameDto NipCard(Guid gameId, Guid nippingPlayerId, string toBeNippedPlayerName)
        {
            return _timeBombService.NipCard(gameId, nippingPlayerId, toBeNippedPlayerName);
        }

        [HttpGet]
        [Route("getactualgamestate")]
        public PlayerSpecificGameDto GetActualGameState(Guid gameId, Guid playerId)
        {
            return _timeBombService.GetActualGameState(gameId, playerId);
        }

    }
}