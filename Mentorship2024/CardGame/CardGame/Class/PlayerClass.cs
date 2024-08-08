using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.Class.CardClass;

namespace CardGame.Class
{
    public class PlayerClass
    {
        public class Player
        {
            public List<Card> Hand { get; private set; }

            public Player()
            {
                Hand = new List<Card>();
            }

            public void ReceiveCard(Card card)
            {
                Hand.Add(card);
            }
            public int CalculateScore()
            {
                int score = Hand.Sum(card => (int)card.Value);
                return score % 10;  // Baccarat score
            }
        }

    }
}
