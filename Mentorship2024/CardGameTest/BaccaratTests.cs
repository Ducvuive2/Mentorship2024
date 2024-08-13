using CardGameGunny.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameTest
{
    public class BaccaratTests
    {
        [Fact]
        public void Baccarat_ScoringCalculation_ShouldBeCorrect()
        {
            var player = new BaccaratPlayer();
            player.AddCard(new Card(Suit.Hearts, Value.Nine));  // 9
            player.AddCard(new Card(Suit.Clubs, Value.Four));   // 4
            player.AddCard(new Card(Suit.Spades, Value.Ace));   // 1

            var score = player.CalculateScore();

            Assert.Equal(4, score);
        }
        [Fact]
        public void Baccarat_ScoringCalculationFullFaceCard_ShouldBeCorrect()
        {
            var player = new BaccaratPlayer();
            player.AddCard(new Card(Suit.Spades, Value.Jack));  
            player.AddCard(new Card(Suit.Clubs, Value.Queen));   
            player.AddCard(new Card(Suit.Spades, Value.King));   

            var score = player.CalculateScore();

            Assert.Equal(10, score);
        }
        [Fact]
        public void Baccarat_ScoringCalculationTwoFaceCard_ShouldBeCorrect()
        {
            var player = new BaccaratPlayer();
            player.AddCard(new Card(Suit.Spades, Value.Jack));
            player.AddCard(new Card(Suit.Clubs, Value.Queen));
            player.AddCard(new Card(Suit.Spades, Value.Eight));

            var score = player.CalculateScore();

            Assert.Equal(8, score);
        }
    }
}
