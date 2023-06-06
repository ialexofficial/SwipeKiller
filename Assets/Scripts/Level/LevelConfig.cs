using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Game Data/Level Data")]
    public class LevelConfig : ScriptableObject
    {
        public string LevelSceneName;
        [Tooltip("0 is equivalent to infinity")]
        public int SwipeCount;
        public Vector3 WeaponSpawnPoint;
        [Header("Cinemachine propertis")]
        public Vector3 CameraPosition;
        public float FieldOfView;
    }
}