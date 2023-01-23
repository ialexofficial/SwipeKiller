using System.Collections.Generic;
using System.Linq;
using Database.Interfaces;
using Database.Structures;
using JetBrains.Annotations;

namespace Database
{
    public class WeaponDatabase : Database<PlayerWeapon>, IWeaponDatabase
    {
        private const string WeaponFile = "/Weapon.json";
        
        private static IWeaponDatabase _instance;

        public static IWeaponDatabase GetInstance => _instance ??= new WeaponDatabase();

        public void AddBoughtWeapon(string weaponName)
        {
            data.BoughtWeapon ??= new HashSet<string>();
            data.BoughtWeapon.Add(weaponName);
            
            Serialize();
        }

        public void SetPlayerSelectedWeapon(string weaponName)
        {
            data.SelectedWeapon = weaponName;
            
            Serialize();
        }

        [CanBeNull]
        public IEnumerable<string> GetPlayerBoughtWeaponNames() =>
            data.BoughtWeapon is null
                ? null
                : from weapon in data.BoughtWeapon select weapon;

        public string GetPlayerSelectedWeaponName() =>
            data.SelectedWeapon;

        private WeaponDatabase()
            : base(WeaponFile)
        {
        }
    }
}