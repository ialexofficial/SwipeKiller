using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Database.ScriptableObjects
{
    public class WeaponDatabaseInfo : ScriptableObject
    {
        [Tooltip(
            "Renaming WeaponScriptableObject files will lead to bugs in player's local saves!\n" +
            "All names must be unique!"
        )]
        [SerializeField] private List<WeaponScriptableObject> weapons;

        private Dictionary<string, WeaponScriptableObject> _weaponDictionary;

        public WeaponScriptableObject GetWeapon(string weaponName)
        {
            if (_weaponDictionary is null)
                Init();

            return _weaponDictionary[weaponName];
        }
        
        private void Init()
        {
            _weaponDictionary = new Dictionary<string, WeaponScriptableObject>();
            
            foreach (WeaponScriptableObject weapon in weapons)
            {
                _weaponDictionary[weapon.name] = weapon;
            }
        }
    }
}