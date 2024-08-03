using RomanToInteger;
using System;

namespace TestRomanToInteger_
{
    public class RomanToIntegerTest
    {
        [Theory]
        [InlineData("III", 3)]
        [InlineData("IV", 4)]
        [InlineData("IX", 9)]
        [InlineData("LVIII", 58)]
        [InlineData("MCMXCIV", 1994)]
        [InlineData("VV", -1)]
        [InlineData("", -1)]
        public void RomanToInteger_ValidRomanNumber_ReturnsExpectedInteger(string roman, int expected)
        {
            // Arrange
            ActionConvert _converter = new ActionConvert();

            // Act
            int result = _converter.RomanToInteger(roman);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}

// https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices