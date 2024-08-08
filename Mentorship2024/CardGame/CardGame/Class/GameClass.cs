using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.Class.DeckClass;
using static CardGame.Class.PlayerClass;

namespace CardGame.Class
{
    public class GameClass
    {
        public Deck deck;
        public List<Player> players;

        public GameClass()
        {
            deck = new Deck();
            deck.Shuffle();
            // create two players
            players = new List<Player> { new Player(), new Player() }; 
        }

        public void DealInitialCards()
        {
            foreach (var player in players)
            {
                player.ReceiveCard(deck.DealCard());
                player.ReceiveCard(deck.DealCard());
                player.ReceiveCard(deck.DealCard());
            }
        }
    }

}
