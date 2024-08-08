using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Class
{
    public class CardClass
    {
        public enum Suit
        {
            Clubs, Diamonds, Hearts, Spades
        }

        public enum Value
        {
            Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
            J = 10, Q = 10, K = 10, A = 1
        }

        public class Card
        {
            public Suit Suit { get; set; }
            public Value Value { get; set; }

            public Card(Suit suit, Value value)
            {
                Suit = suit;
                Value = value;
            }
        }

    }
}
