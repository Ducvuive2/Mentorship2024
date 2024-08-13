using Newtonsoft.Json.Linq;
using CardGameGunny.Models;

namespace CardGameTest
{
    public class CardTest
    {
        [Fact]
        public void CardInitialization_CheckProperties()
        {
            var card = new Card(Suit.Hearts, Value.Ace);
            Assert.Equal(Suit.Hearts, card.Suit);
            Assert.Equal(Value.Ace, card.Value);
        }
    }
}