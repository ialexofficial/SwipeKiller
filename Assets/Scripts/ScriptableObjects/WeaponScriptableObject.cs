using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Weapon", order = 1)]
    public class WeaponScriptableObject : ScriptableObject
    {
        public MeshFilter WeaponMeshFilter;
        public Material WeaponMaterial;
        public int Damage = 1;
    }   
}