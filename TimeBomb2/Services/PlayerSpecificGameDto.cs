using System;
using System.Collections.Generic;
using System.Linq;
using TimeBomb.Data;
using TimeBomb2.Data;
using TimeBomb2.Data.Dtos;

namespace TimeBomb2.Services
{
    public class PlayerSpecificGameDto
    {
        public PlayerSpecificGameDto(Game game, Guid playerId)
        {
            GameId = game.GameId;
            OwnPlayer = game.Players.First(p => p.PlayerId == playerId);
            RevealedPlayCards = game.RevealedPlayCards;
            Started = game.Started;
            OtherPlayers = GetOtherPlayers(game, playerId);
        }
        
        public Guid GameId { get; set; }
        public List<OtherPlayerDto> OtherPlayers { get; set; }
        public Player OwnPlayer { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool Started { get; set; }

        private List<OtherPlayerDto> GetOtherPlayers(Game game, Guid playerId)
        {
            var otherPlayerDtos = new List<OtherPlayerDto>();
            game.Players.ForEach(p =>
            {
                if (p.PlayerId != playerId)
                {
                    otherPlayerDtos.Add(new OtherPlayerDto(p));
                }
            });
            return otherPlayerDtos;
        }
    }
}