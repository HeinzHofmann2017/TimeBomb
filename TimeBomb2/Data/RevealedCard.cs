﻿namespace TimeBomb2.Data
{
    public class RevealedPlayCard
    {
        public int Round { get; set; }
        public string NameOfPlayerWhichHadThisCard { get; set; }
        public PlayCard PlayCard { get; set; }
    }
}