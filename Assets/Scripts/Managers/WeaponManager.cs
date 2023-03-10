using Components;
using Database;
using Database.Interfaces;
using Database.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        public UnityEvent OnWeaponDestroy = new UnityEvent();
        
        #if UNITY_EDITOR
        public Weapon PreviewingWeapon;
        public Vector3 WeaponStartPosition => weaponStartPosition;
        public Quaternion WeaponStartRotation => Quaternion.Euler(weaponStartRotation);
        #endif
        
        [SerializeField] private Weapon defaultWeapon;
        [SerializeField] private WeaponDatabaseInfo weaponDatabase;
        [SerializeField] private Vector3 weaponStartPosition;
        [SerializeField] private Vector3 weaponStartRotation;

        private Weapon _selectedWeapon;
        private Weapon _weapon;

        private void Start()
        {
            SetSelectedWeapon();
            UpdateWeaponOnScene();
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

        private void UpdateWeaponOnScene()
        {
            if (_weapon != null)
            {
                weaponStartPosition = _weapon.transform.position;
                weaponStartRotation = _weapon.transform.eulerAngles;
                Destroy(_weapon.gameObject);
            }
            
            Quaternion weaponRotation = Quaternion.Euler(weaponStartRotation);

            _weapon = Instantiate(_selectedWeapon, weaponStartPosition, weaponRotation);
            _weapon.OnWeaponDestroy.AddListener(() => OnWeaponDestroy.Invoke());
        }
    }
}