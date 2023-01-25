using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public MeshFilter MeshFilter;
        public Material Material;
        public int Health = 1;
    }
}