using CardGameFunny.Models;
using System;
using System.Collections.Generic;

namespace CardGameFunny.Utilities
{
    public static class DeckShuffler
    {
        public static void Shuffle<T>(List<T> items)
        {
            Random rng = new Random();
            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = items[k];
                items[k] = items[n];
                items[n] = value;
            }
        }
    }
}
