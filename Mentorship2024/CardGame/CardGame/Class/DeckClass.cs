using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.Class.CardClass;

namespace CardGame.Class
{
    public class DeckClass
    {
        public class Deck
        {
            public List<Card> cards;

            public Deck()
            {
                cards = new List<Card>();
                // loop to create a deck of cards
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Value value in Enum.GetValues(typeof(Value)))
                    {
                        cards.Add(new Card(suit, value));
                    }
                }
            }

            public void Shuffle()
            {
                Random rng = new Random();
                int n = cards.Count;
                //Fisher-Yates Shuffle
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    Card value = cards[k];
                    cards[k] = cards[n];
                    cards[n] = value;
                }
            }

            public Card DealCard()
            {
                var card = cards.Last();
                cards.Remove(card);
                return card;
            }
        }

    }
}
