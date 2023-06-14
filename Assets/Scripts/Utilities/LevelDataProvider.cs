using Level;
using Ji2.CommonCore.SaveDataContainer;
using Managers;
using SwipeKiller;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class LevelDataProvider
    {
        private readonly ISaveDataContainer _saveDataContainer;
        private readonly LevelDatabase _levelDatabase;
        private int _currentLevelKey;

        public LevelConfig CurrentLevel => _levelDatabase.Data[_currentLevelKey];
        public bool IsFirstLevel => CurrentLevel == _levelDatabase.FirstLevel;
        
        public LevelDataProvider(
            ISaveDataContainer saveDataContainer,
            LevelDatabase levelDatabase
        )
        {
            _saveDataContainer = saveDataContainer;
            _levelDatabase = levelDatabase;
        }

        public void Load()
        {
            _currentLevelKey = _saveDataContainer.GetValue(
                Constants.LEVEL_SAVE_DATA_KEY,
                _levelDatabase.Data.IndexOf(_levelDatabase.FirstLevel)
            );
        }

        public void IncreaseLevel()
        {
            if (_currentLevelKey == _levelDatabase.Data.Count - 1)
                return;
            
            ++_currentLevelKey;
            
            _saveDataContainer.SaveValue(Constants.LEVEL_SAVE_DATA_KEY, _currentLevelKey);
        }

        public void SetLevel(string levelName)
        {
            _currentLevelKey = _levelDatabase
                .Data
                .FindIndex(levelConfig => levelConfig.LevelSceneName == levelName);
            _saveDataContainer.SaveValue(Constants.LEVEL_SAVE_DATA_KEY, _currentLevelKey);
        }

#if UNITY_EDITOR
        [MenuItem("Tools/Player Saves/Clear Level Data")]
        public static void ClearPrefsSaves()
        {
            PlayerPrefsSaveDataContainer playerPrefsSaveDataContainer = new PlayerPrefsSaveDataContainer();
            playerPrefsSaveDataContainer.Load();
            playerPrefsSaveDataContainer.ResetKey(Constants.LEVEL_SAVE_DATA_KEY);
            Debug.Log("Cleared!");
        }
#endif
    }
}