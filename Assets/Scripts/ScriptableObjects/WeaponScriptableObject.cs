using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Weapon")]
    public class WeaponScriptableObject : ScriptableObject
    {
        public MeshFilter MeshFilter;
        public Material Material;
        public int Damage = 1;
    }   
}