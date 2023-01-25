using Components;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using EditorBase = UnityEditor.Editor;

namespace Editor
{
    [CustomEditor(typeof(Weapon))]
    public class WeaponPreviewer : EditorBase
    {
        private MeshFilter _weaponMeshFilter;
        private MeshRenderer _weaponMeshRenderer;
        private MeshCollider _collider;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("Visualize weapon"))
                Visualize();
        }

        private void Visualize()
        {
            Weapon weapon = target as Weapon;

            WeaponScriptableObject previewingWeapon = weapon.PreviewingWeapon;
            
            _weaponMeshFilter ??= weapon.GetComponent<MeshFilter>();
            _weaponMeshRenderer ??= weapon.GetComponent<MeshRenderer>();
            _collider ??= weapon.GetComponent<MeshCollider>();

            _collider.sharedMesh = _weaponMeshFilter.mesh = previewingWeapon.MeshFilter.sharedMesh;
            _weaponMeshRenderer.material = previewingWeapon.Material;
        }
    }
}