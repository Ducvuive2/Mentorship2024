using CardGameGunny.Models;
using System.Collections.Generic;

namespace CardGameFunny.Models
{
    public abstract class Player
    {
        public List<Card> Hand { get; private set; } = new List<Card>();

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }
        public abstract int CalculateScore();
    }
}
