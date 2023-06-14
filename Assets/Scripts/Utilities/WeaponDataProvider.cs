using System.Collections.Generic;
using System.Linq;
using Ji2.CommonCore.SaveDataContainer;
using Level;
using SwipeKiller;

namespace Utilities
{
    public class WeaponDataProvider
    {
        private readonly Dictionary<int, WeaponData> _boughtWeapons = new Dictionary<int, WeaponData>();
        private readonly ISaveDataContainer _saveDataContainer;
        private readonly WeaponDatabase _weaponDatabase;
        private int _selectedWeaponKey;

        public List<WeaponData> BoughtWeapons => _boughtWeapons.Values.ToList();

        public WeaponData SelectedWeapon => _boughtWeapons[_selectedWeaponKey];

        public WeaponDataProvider(
            ISaveDataContainer saveDataContainer,
            WeaponDatabase weaponDatabase
        )
        {
            _saveDataContainer = saveDataContainer;
            _weaponDatabase = weaponDatabase;
        }

        public void Load()
        {
            foreach (int key in _saveDataContainer.GetValue(
                         Constants.BOUGHT_WEAPON_SAVE_DATA_KEY,
                         Enumerable.Empty<int>()
                     ))
            {
                _boughtWeapons[key] = _weaponDatabase.Weapons[key];
            }

            if (_boughtWeapons.Count == 0)
            {
                WeaponData defaultWD = _weaponDatabase.DefaultWeapon;
                _selectedWeaponKey = _weaponDatabase.Weapons.IndexOf(defaultWD);
                _boughtWeapons[_selectedWeaponKey] = defaultWD;
            }
            else
            {
                _selectedWeaponKey = _saveDataContainer.GetValue<int>(
                    Constants.SELECTED_WEAPON_SAVE_DATA_KEY);
            }
        }
    }
}