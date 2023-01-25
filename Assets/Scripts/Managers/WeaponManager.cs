using Components;
using Database;
using Database.Interfaces;
using Database.ScriptableObjects;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private WeaponScriptableObject defaultWeapon;
        [SerializeField] private WeaponDatabaseInfo weaponDatabase;
        [SerializeField] private Weapon weapon;

        private WeaponScriptableObject _selectedWeapon;

        private void Start()
        {
            SetSelectedWeapon();
            weapon.UpdateData(_selectedWeapon);
        }

        private void SetSelectedWeapon()
        {
            IWeaponDatabase database = WeaponDatabase.GetInstance;
            string selectedWeaponName = database.GetPlayerSelectedWeaponName();

            if (selectedWeaponName is null)
            {
                _selectedWeapon = defaultWeapon;

                database.SetPlayerSelectedWeapon(_selectedWeapon.name);
                database.AddBoughtWeapon(_selectedWeapon.name);
            }
            else
            {
                _selectedWeapon = weaponDatabase.GetWeapon(selectedWeaponName);
            }
        }
    }
}