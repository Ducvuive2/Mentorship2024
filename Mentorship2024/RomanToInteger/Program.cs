using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Get input from the user
        Console.Write("Enter a Roman numeral to convert to an integer: ");
        string roman = Console.ReadLine().ToUpper();
        // Dictionary to store the integer values of Roman numerals
        Dictionary<char, int> romanDict = new Dictionary<char, int>
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };
        int number = RomanToInteger(roman, romanDict);
        if (number != -1)
        {
            Console.WriteLine($"The integer value of {roman} is {number}");
        }
        else
        {
            Console.WriteLine("Invalid Roman numeral.");
        }
    }

    static int RomanToInteger(string roman, Dictionary<char, int> romanDict)
    {

        int result = 0;
        for (int i = 0; i < roman.Length; i++)
        {
            if (!romanDict.ContainsKey(roman[i]))
            {
                return -1; // Invalid Roman numeral
            }

            // If the current numeral is less than the next numeral, subtract it from the result
            if (i + 1 < roman.Length && romanDict[roman[i]] < romanDict[roman[i + 1]])
            {
                result -= romanDict[roman[i]];
            }
            else
            {
                result += romanDict[roman[i]];
            }
        }

        return result;
    }
}
