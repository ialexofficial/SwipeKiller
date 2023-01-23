using Database.Interfaces;
using Database.Structures;

namespace Database
{
    public class ProgressDatabase : Database<PlayerProgress>, IProgressDatabase
    {
        private const string ProgressFile = "/Progress.json";
        
        private static IProgressDatabase _instance;

        public static IProgressDatabase GetInstance => _instance ??= new ProgressDatabase();

        public int GetCurrentLevel() =>
            data.CurrentLevel;

        public void SetCurrentLevel(int level)
        {
            data.CurrentLevel = level;
            
            Serialize();
        }

        private ProgressDatabase()
            : base(ProgressFile)
        {}
    }
}