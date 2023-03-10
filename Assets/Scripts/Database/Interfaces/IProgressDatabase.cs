namespace Database.Interfaces
{
    public interface IProgressDatabase
    {
        public void SetCurrentLevel(int level);
        public int GetCurrentLevel();
        public void SetCoins(int coins);
        public int GetCoins();
    }
}