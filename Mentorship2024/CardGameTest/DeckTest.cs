using CardGameFunny.Models;
using CardGameGunny.Models;
using Newtonsoft.Json.Linq;

namespace CardGameTest
{
    public class DeskTest
    {
        [Fact]
        public void DeckInitialization_CreatesFullDeck()
        {
            var deck = new Deck();
            Assert.Equal(52, deck.cards.Count);
        }
        [Fact]
        public void DeckInitialization_CheckNumberCardFive()
        {
            var deck = new Deck();
            int queenCount = deck.cards.Count(card => card.Value == Value.Five);
            Assert.Equal(4, queenCount);
        }
    }
}