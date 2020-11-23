namespace TimeBomb.Data
{
    public class RevealedPlayCard
    {
        public int Round { get; set; }
        public string NameOfPlayerWhichHadThisCard { get; set; }
        public PlayCard Card { get; set; }
    }
}