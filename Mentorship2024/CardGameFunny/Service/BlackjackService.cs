using CardGameFunny.Models;
using System;
using System.Collections.Generic;

namespace CardGameFunny.Services
{
    public class BlackjackService : IGameService
    {
        private Deck deck;
        private List<BlackjackPlayer> players = new List<BlackjackPlayer>();
        private BlackjackPlayer banker;
        private const int MAX_PLAYERS = 4;  // This does not include the banker

        public void InitializeGame()
        {
            deck = new Deck();
            int numberOfPlayers = GetNumberOfPlayers();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new BlackjackPlayer());
            }
            banker = new BlackjackPlayer();  // Separate banker
        }

        private int GetNumberOfPlayers()
        {
            Console.WriteLine("Enter the number of players (2-4):");
            int count;
            while (!int.TryParse(Console.ReadLine(), out count) || count < 2 || count > MAX_PLAYERS)
            {
                Console.WriteLine("Invalid number, please enter a value between 2 and 4:");
            }
            return count;
        }

        public void StartGame()
        {
            Console.WriteLine("Starting Blackjack...");
            DealInitialCards();
            foreach (var player in players)
            {
                PlayerTurn($"Player {players.IndexOf(player) + 1}", player);
            }
            PlayerTurn("Banker", banker);
            DetermineWinner();
        }

        public void DealInitialCards()
        {
            foreach (var player in players)
            {
                player.AddCard(deck.DealCard());
                player.AddCard(deck.DealCard());
                DisplayHand(player);
            }
            // Deal cards to the banker
            banker.AddCard(deck.DealCard());
            banker.AddCard(deck.DealCard());
            DisplayHand(banker);
        }

        private void DisplayHand(BlackjackPlayer player)
        {
            Console.WriteLine($"Current hand of {player.GetType().Name}:");
            foreach (var card in player.Hand)
            {
                Console.WriteLine($"{card.Value} of {card.Suit}");
            }
            Console.WriteLine($"Current score: {player.CalculateScore()}");
            Console.WriteLine();
        }

        private void PlayerTurn(string who, BlackjackPlayer currentPlayer)
        {
            Console.WriteLine($"{who}'s turn...");
            while (true)
            {
                DisplayHand(currentPlayer);
                Console.WriteLine($"{who}, do you want to (H)it or (S)tand?");
                string choice = Console.ReadLine().ToUpper();

                if (choice == "H")
                {
                    currentPlayer.AddCard(deck.DealCard());
                    if (currentPlayer.CalculateScore() > 21)
                    {
                        DisplayHand(currentPlayer);
                        Console.WriteLine($"{who} busts!");
                        break;
                    }
                }
                else if (choice == "S")
                {
                    Console.WriteLine($"{who} stands.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice, please choose (H)it or (S)tand.");
                }
            }
        }

        public void DetermineWinner()
        {
            int bankerScore = banker.CalculateScore();
            Console.WriteLine($"Banker Score: {bankerScore}");

            foreach (var player in players)
            {
                int playerScore = player.CalculateScore();
                Console.WriteLine($"Player {players.IndexOf(player) + 1} Score: {playerScore}");

                if (playerScore > 21)
                {
                    if(bankerScore > 21)
                    {
                        Console.WriteLine($"Player {players.IndexOf(player) + 1} busts and ties with the banker.");
                    }
                    else
                    {
                        Console.WriteLine($"Player {players.IndexOf(player) + 1} loses.");
                    }
                }
                else if (playerScore > bankerScore)
                {
                    Console.WriteLine($"Player {players.IndexOf(player) + 1} wins!");
                }
                else if (playerScore < bankerScore)
                {
                    Console.WriteLine($"Player {players.IndexOf(player) + 1} loses.");
                }
                else
                {
                    Console.WriteLine($"Player {players.IndexOf(player) + 1} ties with the banker.");
                }
            }
        }
    }
}
