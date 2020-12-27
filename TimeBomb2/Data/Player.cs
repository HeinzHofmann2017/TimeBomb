using System;
using System.Collections.Generic;

namespace TimeBomb2.Data
{
    public class Player
    {
        public Guid PlayerId { get; set; } = new Guid();
        public string Name { get; set; }
        public bool HoldsNipper { get; set; } = false;
        public RoleCard RoleCard { get; set; }
        public List<PlayCard> HiddenPlayCards { get; set; }
    }
}