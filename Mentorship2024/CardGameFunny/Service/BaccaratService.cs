using CardGameFunny.Models;
using CardGameFunny.Services;
using System;

namespace CardGameFunny.Services
{
    public class BaccaratService : IGameService
    {
        private Deck deck;
        private Player player;
        private Player banker;

        public void InitializeGame()
        {
            deck = new Deck();
            player = new BaccaratPlayer();  
            banker = new BaccaratPlayer();  
        }

        public void StartGame()
        {
            Console.WriteLine("Starting Baccarat...");

            // Deal cards to player and banker
            for(int i = 0; i < 2; i++)
            {
                player.AddCard(deck.DealCard());
            }
            for (int i = 0; i < 2; i++)
            {
                banker.AddCard(deck.DealCard());
            }

            DisplayHands();
            DetermineWinner();
        }

        private void DisplayHands()
        {
            Console.WriteLine("Player's hand:");
            foreach (var card in player.Hand)
            {
                Console.WriteLine($"{card.Value} of {card.Suit}");
            }

            Console.WriteLine("Banker's hand:");
            foreach (var card in banker.Hand)
            {
                Console.WriteLine($"{card.Value} of {card.Suit}");
            }
        }
        public void DetermineWinner()
        {
            int playerScore = player.CalculateScore();
            int bankerScore = banker.CalculateScore();

            Console.WriteLine($"Player Score: {playerScore}, Banker Score: {bankerScore}");

            if (playerScore > bankerScore)
            {
                Console.WriteLine("Player wins!");
            }
            else if (playerScore < bankerScore)
            {
                Console.WriteLine("Banker wins!");
            }
            else
            {
                Console.WriteLine("Tie game!");
            }
        }
    }
}
