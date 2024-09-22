using System;
using System.Collections.Generic;
using RomanToInteger;
class Program
{
    static void Main()
    {
        // Get input from the user
        Console.Write("Enter a Roman numeral to convert to an integer: ");
        string roman = Console.ReadLine().ToUpper();
        ActionConvert converter = new ActionConvert();
        int number = converter.RomanToInteger(roman);
        if (number != -1)
        {
            Console.WriteLine($"The integer value of {roman} is {number}");
        }
        else
        {
            Console.WriteLine("Invalid Roman numeral.");
        }
    }
}
