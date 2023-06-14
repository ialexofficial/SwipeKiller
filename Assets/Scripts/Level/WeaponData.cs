using Entities;
using Entities.Views.Weapon;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Game Data/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public int Cost;
        public BaseWeapon Prefab;
        public SlowMotionSettings SlowMotionSettings;
        public SwipeSettings SwipeSettings;
        public Vector2 SwipeLimiterOffset;
    }
}