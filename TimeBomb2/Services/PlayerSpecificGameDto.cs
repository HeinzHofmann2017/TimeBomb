using System;
using System.Collections.Generic;
using TimeBomb.Data;
using TimeBomb2.Data.Dtos;

namespace TimeBomb2.Services
{
    public class PlayerSpecificGameDto
    {
        public Guid GameId { get; set; }
        public List<OtherPlayerDto> OtherPlayers { get; set; }
        public Player OwnPlayer { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool Started { get; set; }
    }
}