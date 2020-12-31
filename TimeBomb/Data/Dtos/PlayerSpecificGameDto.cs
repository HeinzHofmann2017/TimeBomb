using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeBomb.Data.Dtos
{
    public class PlayerSpecificGameDto
    {
        public PlayerSpecificGameDto(Game game, Guid playerId)
        {
            GameId = game.GameId;
            OwnPlayer = game.Players.First(p => p.PlayerId == playerId);
            RevealedPlayCards = game.RevealedPlayCards;
            IsStarted = game.IsStarted;
            OtherPlayers = GetOtherPlayers(game, playerId);
            IsFinished = game.IsFinished();
            Winner = game.GetWinner();
        }
        
        public Guid GameId { get; set; }
        public List<OtherPlayerDto> OtherPlayers { get; set; }
        public Player OwnPlayer { get; set; }
        public List<RevealedPlayCard> RevealedPlayCards { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public RoleCard? Winner { get; set; }

        private List<OtherPlayerDto> GetOtherPlayers(Game game, Guid playerId)
        {
            var otherPlayerDtos = new List<OtherPlayerDto>();
            game.Players.ForEach(p =>
            {
                if (p.PlayerId != playerId)
                {
                    otherPlayerDtos.Add(new OtherPlayerDto(p, game.IsFinished()));
                }
            });
            return otherPlayerDtos;
        }
    }
}