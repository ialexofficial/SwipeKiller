using Entities.Views.Weapon;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    // [CustomEditor(typeof(WeaponManager))]
    public class WeaponPreviewer : BaseEditor
    {
        private BaseWeapon _previewed;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Visualize weapon"))
                Visualize();
        }

        private void OnDisable()
        {
            if (_previewed is null)
                return;
            
            DestroyImmediate(_previewed.gameObject);
        }

        private void Visualize()
        {
            if (_previewed != null)
            {
                DestroyImmediate(_previewed.gameObject);
            }
            
            // WeaponManager manager = target as WeaponManager;

            // _previewed = Instantiate(manager.PreviewingWeapon, manager.WeaponStartPosition,
                // manager.WeaponStartRotation);
        }
    }
}