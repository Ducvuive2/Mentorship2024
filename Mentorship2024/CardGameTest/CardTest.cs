using Newtonsoft.Json.Linq;
using static CardGame.Class.CardClass;

namespace CardGameTest
{
    public class CardTest
    {
        [Fact]
        public void CardInitialization_CheckProperties()
        {
            var card = new Card(Suit.Hearts, Value.A);
            Assert.Equal(Suit.Hearts, card.Suit);
            Assert.Equal(Value.A, card.Value);
        }
    }
}