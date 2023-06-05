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
                    SceneManager.LoadScene("Bootstrap");
                }
            };
        }
    }
}