using CardGame.Class;
using System;
using static CardGame.Class.PlayerClass;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Console Baccarat Game!");
        GameClass game = new GameClass();
        game.DealInitialCards();

        DisplayPlayerHands(game);

        Console.ReadKey();
    }

    static void DisplayPlayerHands(GameClass game)
    {
        int playerNumber = 1;
        foreach (var player in game.players)
        {
            Console.WriteLine($"Player {playerNumber++}'s hand:");
            foreach (var card in player.Hand)
            {
                Console.WriteLine($"- {card.Value} of {card.Suit}");
            }
            Console.WriteLine($"Score: {player.CalculateScore()}\n");
        }
    }
}
