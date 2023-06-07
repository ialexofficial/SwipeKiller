using System.Collections.Generic;
using Entities;
using Entities.Views.Weapon;
using UnityEngine;
using Utilities;

namespace Level.Models
{
    public class WeaponDataModel
    {
        private readonly Vector3 _weaponSpawnPoint;
        private readonly WeaponDataProvider _weaponDataProvider;
        private readonly List<WeaponData> _boughtWeapon;
        private WeaponData _selectedWeapon;

        public Vector3 WeaponSpawnPoint => _weaponSpawnPoint;
        public BaseWeapon SelectedWeapon => _selectedWeapon.Prefab;
        public SwipeSettings SwipeSettings => _selectedWeapon.SwipeSettings;
        public SlowMotionSettings SlowMotionSettings => _selectedWeapon.SlowMotionSettings;
        public Vector2 SwipeConfinerOffset => _selectedWeapon.SwipeConfinerOffset;

        public WeaponDataModel(
            WeaponDataProvider weaponDataProvider,
            Vector3 weaponSpawnPoint
        )
        {
            _weaponSpawnPoint = weaponSpawnPoint;
            _weaponDataProvider = weaponDataProvider;
            
            _boughtWeapon = _weaponDataProvider.BoughtWeapon;
            _selectedWeapon = _weaponDataProvider.SelectedWeapon;
        }

        public void OnWeaponDestroyed()
        {
            
        }
    }
}