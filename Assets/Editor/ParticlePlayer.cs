using GUI;
using UnityEditor;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    [CustomEditor(typeof(LevelGUI))]
    public class ParticlePlayer : BaseEditor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Win particles"))
                PlayWinParticles();
        }

        private void PlayWinParticles()
        {
            LevelGUI manager = target as LevelGUI;

            foreach (var particles in manager?.WinParticles)
            {
                particles.Play();
            }
        }
    }
}