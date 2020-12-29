using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeBomb2.Repositories;
using TimeBomb2.Services;

namespace TimeBomb2.Controllers
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
    }
}