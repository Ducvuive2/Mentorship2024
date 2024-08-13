using System.Collections.Generic;
using CardGameFunny.Utilities;
using CardGameGunny.Models;

namespace CardGameFunny.Models
{
    public class Deck
    {
        public List<Card> cards = new List<Card>();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    cards.Add(new Card(suit, value));
                }
            }
            DeckShuffler.Shuffle(cards);
        }

        public Card DealCard()
        {
            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }
}
