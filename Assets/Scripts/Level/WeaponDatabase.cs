using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Game Data/Weapon Database")]
    public class WeaponDatabase : ScriptableObject
    {
        public WeaponData DefaultWeapon;
        [Tooltip("Levels are saving by order in array. Changing level order can crash player saves!")]
        public List<WeaponData> Weapons;
    }
}