namespace CardGameFunny.Services
{
    public interface IGameService
    {
        void InitializeGame();
        void StartGame();
        void DetermineWinner();
    }
}
