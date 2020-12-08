using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using TimeBomb.Data;
using TimeBomb2.Data;
using TimeBomb2.Repositories;

namespace TimeBomb2.Services
{
    public static class TimeBombService
    {
        public static Guid CreateGame()
        {
            return TimeBombRepository.CreateGameAndGetItsId();
        }

        public static PlayerSpecificGameDto RegisterNewPlayer(Guid gameId, string name)
        {
            var game = TimeBombRepository.GetGameById(gameId);
            var playerId = Guid.NewGuid();
            if (game.Started || game.Players.Count >= 6)
                return null;

            var players = game.Players;
            players.Add(new Player()
            {
                PlayerId = playerId,
                Name = name
            });
            
            var newGame = TimeBombRepository.UpdateGame(gameId, players, null, null);

            return new PlayerSpecificGameDto(newGame, playerId);
        }

        public static PlayerSpecificGameDto StartGame(Guid gameId, Guid playerId)
        {
            var game = TimeBombRepository.GetGameById(gameId);
            if (game.Started) return new PlayerSpecificGameDto(game,playerId);

            var players = game.Players;
            var roleCards = GetRoleCardsForSpecificAmountOfPlayers(players.Count).ToList();
            var playCards = GetPlayCardsForSpecificAmountOfPlayers(players.Count).ToList();

            players.ForEach(player =>
            {
                player.RoleCard = roleCards.RemoveAndGetRandomCardFromList();
                for (var i = 0; i < 5; ++i) player.HiddenPlayCards.Add(playCards.RemoveAndGetRandomCardFromList());
            });
            players.AssignNipperRandomToOnePlayer();

            var startedGame = TimeBombRepository.UpdateGame(gameId, players, new List<RevealedPlayCard>(), true);
            return new PlayerSpecificGameDto(startedGame, playerId);
        }

        public static PlayerSpecificGameDto NipCard(Guid gameId, Guid nippingPlayerId, string toBeNippedPlayerName)
        {
            var game = TimeBombRepository.GetGameById(gameId);
            if (!game.IsRunning()
                || !game.PlayerHoldsNipper(nippingPlayerId)
                || !game.PlayerHasRemainingHiddenCards(toBeNippedPlayerName)
                || game.PlayerIdMatchesWithName(nippingPlayerId, toBeNippedPlayerName))
                return new PlayerSpecificGameDto(game, nippingPlayerId);

            var revealedPlayCards = game.RevealedPlayCards;
            var players = game.Players;
            var revealedPlayCard = players
                .First(p => p.Name == toBeNippedPlayerName)
                .HiddenPlayCards
                .RemoveAndGetRandomCardFromList();

            revealedPlayCards.Add(new RevealedPlayCard
            {
                Round = game.GetGameRound(),
                Card = revealedPlayCard,
                NameOfPlayerWhichHadThisCard = toBeNippedPlayerName
            });
            var updatedGame = TimeBombRepository.UpdateGame(game.GameId, players, revealedPlayCards, null);

            if (!updatedGame.NewRoundStartsNow())
            {
                return new PlayerSpecificGameDto(updatedGame, nippingPlayerId);
            }
            
            var playersWithMixedCards = MixHiddenCards(updatedGame.Players);
            
            var mixedGame = TimeBombRepository.UpdateGame(updatedGame.GameId, playersWithMixedCards, null, null);
            return new PlayerSpecificGameDto(mixedGame, nippingPlayerId);
        }

        private static List<Player> MixHiddenCards(List<Player> players)
        {
            var listOfAllHiddenCards = players.SelectMany(p => p.HiddenPlayCards).ToList();
            players.ForEach(p => p.HiddenPlayCards = new List<PlayCard>());
            var numberOfPlayCardsPerPlayer = listOfAllHiddenCards.Count / players.Count;
            players.ForEach(p =>
            {
                for (var i = 0; i < numberOfPlayCardsPerPlayer; ++i)
                {
                    p.HiddenPlayCards.Add(listOfAllHiddenCards.RemoveAndGetRandomCardFromList());
                }
            });
            return players;
        }

        private static void AssignNipperRandomToOnePlayer(this IList<Player> players)
        {
            var rnd = new Random();
            var index = rnd.Next(players.Count);
            players[index].HoldsNipper = true;
        }

        private static TCard RemoveAndGetRandomCardFromList<TCard>(this IList<TCard> listOfCards)
        {
            var rnd = new Random();
            var cardIndex = rnd.Next(listOfCards.Count);
            var card = listOfCards[cardIndex];
            listOfCards.RemoveAt(cardIndex);
            return card;
        }

        private static IEnumerable<PlayCard> GetPlayCardsForSpecificAmountOfPlayers(int numberOfPlayers)
        {
            var playCards = new List<PlayCard>
            {
                PlayCard.Bomb
            };

            switch (numberOfPlayers)
            {
                case 4:
                    playCards.AddRange(GetNCards(15, PlayCard.Safe));
                    playCards.AddRange(GetNCards(4, PlayCard.Success));
                    return playCards;
                case 5:
                    playCards.AddRange(GetNCards(19, PlayCard.Safe));
                    playCards.AddRange(GetNCards(5, PlayCard.Success));
                    return playCards;
                case 6:
                    playCards.AddRange(GetNCards(23, PlayCard.Safe));
                    playCards.AddRange(GetNCards(6, PlayCard.Success));
                    return playCards;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Number of Players has to be 4, 5 or 6, but was {numberOfPlayers}");
            }
        }

        private static IEnumerable<RoleCard> GetRoleCardsForSpecificAmountOfPlayers(int numberOfPlayers)
        {
            var roleCards = new List<RoleCard>();
            switch (numberOfPlayers)
            {
                case 4:
                    roleCards.AddRange(GetNCards(3, RoleCard.Swat));
                    roleCards.AddRange(GetNCards(2, RoleCard.Terrorist));
                    return roleCards;
                case 5:
                    roleCards.AddRange(GetNCards(3, RoleCard.Swat));
                    roleCards.AddRange(GetNCards(2, RoleCard.Terrorist));
                    return roleCards;
                case 6:
                    roleCards.AddRange(GetNCards(4, RoleCard.Swat));
                    roleCards.AddRange(GetNCards(2, RoleCard.Terrorist));
                    return roleCards;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Number of Players has to be 4, 5 or 6, but was {numberOfPlayers}");
            }
        }

        private static IEnumerable<TCardType> GetNCards<TCardType>(int numberOfCards, TCardType cardSubType)
        {
            var cards = new List<TCardType>();
            for (var i = 0; i < numberOfCards; i++) cards.Add(cardSubType);

            return cards;
        }
    }
}