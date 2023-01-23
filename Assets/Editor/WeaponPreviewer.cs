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
        private WeaponScriptableObject _previewingWeapon;
        private MeshFilter _weaponMeshFilter;
        private MeshRenderer _weaponMeshRenderer;
        
        private void OnSceneGUI()
        {
            if (Application.isPlaying)
                return;
            
            Weapon weapon = target as Weapon;

            if (_previewingWeapon != null && _previewingWeapon == weapon.PreviewingWeapon)
                return;

            _previewingWeapon = weapon.PreviewingWeapon;
            
            _weaponMeshFilter ??= weapon.GetComponent<MeshFilter>();
            _weaponMeshRenderer ??= weapon.GetComponent<MeshRenderer>();

            _weaponMeshFilter.mesh = _previewingWeapon.WeaponMeshFilter.sharedMesh;
            _weaponMeshRenderer.material = _previewingWeapon.WeaponMaterial;
        }
    }
}