using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Database.ScriptableObjects
{
    public class WeaponDatabaseInfo : ScriptableObject
    {
        [Tooltip(
            "Renaming WeaponScriptableObject files will lead to bugs in player's local saves!\n" +
            "All names must be unique!"
        )]
        [SerializeField] private List<Weapon> weapons;

        private Dictionary<string, Weapon> _weaponDictionary;

        public Weapon GetWeapon(string weaponName)
        {
            if (_weaponDictionary is null)
                Init();

            return _weaponDictionary[weaponName];
        }
        
        private void Init()
        {
            _weaponDictionary = new Dictionary<string, Weapon>();
            
            foreach (Weapon weapon in weapons)
            {
                _weaponDictionary[weapon.name] = weapon;
            }
        }
    }
}