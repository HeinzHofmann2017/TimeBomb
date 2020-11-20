﻿using System;
using System.Collections.Generic;

namespace TimeBomb.Data
{
    public class Game
    {
        public Guid GameId { get; set; } = new Guid();
        public List<Player> Players { get; set; }
        public List<PlayCard> RevealedCards { get; set; }
        public bool Started { get; set; } = false;
    }
}