using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Enemy")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public GameObject Prefab;
        public int Health = 1;
    }
}