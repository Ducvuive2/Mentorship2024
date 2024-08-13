using CardGameFunny.Models;
using CardGameGunny.Models;

public class BaccaratPlayer : Player
{
    public override int CalculateScore()
    {
        if (Hand.All(card => card.Value == Value.Jack || card.Value == Value.Queen ||
                     card.Value == Value.King || card.Value == Value.Ten))
        {
            return 10;
        }        int sum = Hand.Sum(card => (int)card.Value);
        return sum % 10;
    }
}
