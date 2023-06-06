using Level;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Managers;
using SwipeKiller;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class LevelDataProvider : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private LevelDatabase levelDatabase;

        private readonly Context _context = Context.GetInstance();
        private ISaveDataContainer _saveDataContainer;
        private int _currentLevelKey;

        public LevelConfig CurrentLevel => levelDatabase.Data[_currentLevelKey];
        
        public void Bootstrap()
        {
            _saveDataContainer = _context.GetService<ISaveDataContainer>();

            _currentLevelKey = _saveDataContainer.GetValue(
                Constants.LEVEL_SAVE_DATA_KEY,
                levelDatabase.Data.IndexOf(levelDatabase.FirstLevel)
            );
        }

        public void IncreaseLevel()
        {
            if (_currentLevelKey == levelDatabase.Data.Count - 1)
                return;
            
            ++_currentLevelKey;
            
            _saveDataContainer.SaveValue(Constants.LEVEL_SAVE_DATA_KEY, _currentLevelKey);
        }

        [MenuItem("Tools/Player Saves/Clear Level Data")]
        public static void ClearPrefsSaves()
        {
            PlayerPrefsSaveDataContainer playerPrefsSaveDataContainer = new PlayerPrefsSaveDataContainer();
            playerPrefsSaveDataContainer.Load();
            playerPrefsSaveDataContainer.ResetKey(Constants.LEVEL_SAVE_DATA_KEY);
        }
    }
}