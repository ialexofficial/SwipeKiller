using System.Collections.Generic;

namespace Database.Interfaces
{
    public interface IWeaponDatabase
    {
        public string GetPlayerSelectedWeaponName();
        public void SetPlayerSelectedWeapon(string weaponName);
        public IEnumerable<string> GetPlayerBoughtWeaponNames();
        public void AddBoughtWeapon(string weaponName);
    }
}