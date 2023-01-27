using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using ViewModels;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    [CustomEditor(typeof(EnemyViewModel))]
    public class EnemyPreviewer : BaseEditor
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Visualize enemy"))
                Visualize();
        }

        private void Visualize()
        {
            EnemyViewModel enemy = target as EnemyViewModel;
            
            Undo.SetCurrentGroupName("Visualizing enemy");

            foreach (Transform childTransform in enemy.GetComponentsInChildren<Transform>(true))
            {
                if (childTransform == null || childTransform.gameObject == enemy.gameObject)
                    continue;
                
                Undo.DestroyObjectImmediate(childTransform.gameObject);
            }
            
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            
            Instantiate(enemy.EnemyData.Prefab, enemy.transform);
        }
    }
}