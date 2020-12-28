using System;
using System.Collections.Generic;
using TimeBomb2.Data;

namespace TimeBomb2.Services
{
    public static class ListExtensions
    {
        public static void AssignNipperRandomToOnePlayer(this IList<Player> players)
        {
            var rnd = new Random();
            var index = rnd.Next(players.Count);
            players[index].HoldsNipper = true;
        }

        public static TCard RemoveAndGetRandomCardFromList<TCard>(this IList<TCard> listOfCards)
        {
            var rnd = new Random();
            var cardIndex = rnd.Next(listOfCards.Count);
            var card = listOfCards[cardIndex];
            listOfCards.RemoveAt(cardIndex);
            return card;
        }
        
    }
}