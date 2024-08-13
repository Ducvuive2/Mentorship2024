using CardGameFunny.Models;
using CardGameGunny.Models;

public class BlackjackPlayer : Player
{
    public override int CalculateScore()
    {
        int sum = Hand.Where(card => card.Value != Value.Ace).Sum(card => (int)card.Value);
        sum += Hand.Count(card => card.Value == Value.Ace) * 11;
        int aceCount = Hand.Count(card => card.Value == Value.Ace);

        while (sum > 21 && aceCount > 0)
        {
            sum -= 10; // Adjusting Ace value from 11 to 1
            aceCount--;
        }

        return sum;
    }
    public void AddCard(Card card)
    {
        Hand.Add(card);
    }
}
