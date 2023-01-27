using Components;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    [CustomEditor(typeof(Weapon))]
    public class WeaponPreviewer : BaseEditor
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
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

            _meshFilter ??= weapon.GetComponent<MeshFilter>();
            _meshRenderer ??= weapon.GetComponent<MeshRenderer>();
            _collider ??= weapon.GetComponent<MeshCollider>();

            Undo.SetCurrentGroupName("Weapon visualizing");
            Undo.RecordObject(_meshFilter, "Edited MeshFilter");
            Undo.RecordObject(_meshRenderer, "Edited Material");
            Undo.RecordObject(_collider, "Edited Collider");
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            
            _collider.sharedMesh = _meshFilter.mesh = previewingWeapon.MeshFilter.sharedMesh;
            _meshRenderer.material = previewingWeapon.Material;
        }
    }
}