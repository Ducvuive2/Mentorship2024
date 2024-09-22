using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanToInteger
{
    public class ActionConvert
    {
        // Dictionary to store the integer values of Roman numerals
        private Dictionary<char, int> romanDict = new Dictionary<char, int>
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };
        private readonly List<char> invalidRepeatable = new List<char> { 'V', 'L', 'D' };
        public int RomanToInteger(string roman)
        {

            if (string.IsNullOrEmpty(roman)) return -1;

            int result = 0;

            for (int i = 0; i < roman.Length; i++)
            {
                if (!romanDict.ContainsKey(roman[i]))
                {
                    return -1; // Invalid Roman numeral
                }

                // Check for invalid repeatable numerals
                if(i > 0  && roman[i] == roman[i-1] && invalidRepeatable.Contains(roman[i]) )
                {
                    return -1;
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
}
