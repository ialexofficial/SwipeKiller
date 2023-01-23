namespace Database.Interfaces
{
    public interface IProgressDatabase
    {
        public void SetCurrentLevel(int level);
        public int GetCurrentLevel();
    }
}