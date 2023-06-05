using System.Collections.Generic;
using System.Linq;
using Entities.Views.Weapon;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Level;
using SwipeKiller;
using UnityEngine;

namespace Utilities
{
    public class WeaponDataProvider : MonoBehaviour, IBootstrapable
    {
        [SerializeField] private WeaponDatabase weaponDatabase;

        private readonly Context _context = Context.GetInstance();
        private readonly Dictionary<int, WeaponData> _boughtWeapon = new Dictionary<int, WeaponData>();
        private int _selectedWeaponKey;
        private ISaveDataContainer _saveDataContainer;

        public List<WeaponData> BoughtWeapon => _boughtWeapon.Values.ToList();

        public WeaponData SelectedWeapon => _boughtWeapon[_selectedWeaponKey];

        public void Bootstrap()
        {
            _saveDataContainer = _context.GetService<ISaveDataContainer>();

            foreach (int key in _saveDataContainer.GetValue(
                         Constants.BOUGHT_WEAPON_SAVE_DATA_KEY,
                         Enumerable.Empty<int>()
                     ))
            {
                _boughtWeapon[key] = weaponDatabase.Weapons[key];
            }

            if (_boughtWeapon.Count == 0)
            {
                WeaponData defaultWD = weaponDatabase.DefaultWeapon;
                _selectedWeaponKey = weaponDatabase.Weapons.IndexOf(defaultWD);
                _boughtWeapon[_selectedWeaponKey] = defaultWD;
            }
            else
            {
                _selectedWeaponKey = _saveDataContainer.GetValue<int>(
                    Constants.SELECTED_WEAPON_SAVE_DATA_KEY);
            }
        }
    }
}