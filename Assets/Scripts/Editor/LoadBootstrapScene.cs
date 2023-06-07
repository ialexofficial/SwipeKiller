using Ji2.CommonCore.SaveDataContainer;
using Managers;
using SwipeKiller;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class LoadBootstrapScene : UnityEditor.Editor
    {
        [InitializeOnEnterPlayMode]
        public static void OnPlayMode(EnterPlayModeOptions options)
        {
            if (SceneManager.GetActiveScene().name == "Bootstrap")
                return;
            
            Debug.Log("Achtung scheiBe!");

            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    LevelDatabase levelDatabase = AssetDatabase
                        .LoadMainAssetAtPath(AssetDatabase
                            .GUIDToAssetPath(
                                AssetDatabase.FindAssets("_LevelDatabase", new []
                                {
                                    "Assets/Scenes/Levels/Level Data"
                                })[0]
                            )
                        ) as LevelDatabase;
                    
                    PlayerPrefsSaveDataContainer dataContainer = new PlayerPrefsSaveDataContainer();
                    dataContainer.Load();
                    dataContainer.SaveValue(
                        Constants.LEVEL_SAVE_DATA_KEY,
                        levelDatabase.Data.FindIndex(levelConfig => 
                            levelConfig.LevelSceneName == SceneManager.GetActiveScene().name
                        )
                    );
                    SceneManager.LoadScene("Bootstrap");
                }
            };
        }
    }
}