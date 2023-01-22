using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database.Interfaces;
using Database.Structures;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Database
{
    public class WeaponDatabase : IWeaponDatabase
    {
        private const string SavesDir = "/Saves";
        private const string WeaponFile = "/Weapon.json";
        
        private static IWeaponDatabase _instance;
        private readonly JsonSerializer _serializer;
        private readonly string _fullSavesPath = Application.persistentDataPath + SavesDir;
        private PlayerWeapon _playerWeapon;
        
        public static IWeaponDatabase GetInstance => _instance ??= new WeaponDatabase();

        public void AddBoughtWeapon(string weaponName)
        {
            _playerWeapon.BoughtWeapon ??= new HashSet<string>();
            _playerWeapon.BoughtWeapon.Add(weaponName);
            
            SaveData();
        }

        public void SetPlayerSelectedWeapon(string weaponName)
        {
            _playerWeapon.SelectedWeapon = weaponName;
            
            SaveData();
        }

        [CanBeNull]
        public IEnumerable<string> GetPlayerBoughtWeaponNames() =>
            _playerWeapon.BoughtWeapon is null
                ? null
                : from weapon in _playerWeapon.BoughtWeapon select weapon;

        public string GetPlayerSelectedWeaponName() =>
            _playerWeapon.SelectedWeapon;

        private WeaponDatabase()
        {
            _serializer = new JsonSerializer();

            if (!Directory.Exists(_fullSavesPath))
                Directory.CreateDirectory(_fullSavesPath);

            ReadData();
        }

        private void SaveData()
        {
            using StreamWriter writer = new StreamWriter(_fullSavesPath + WeaponFile);
            _serializer.Serialize(writer, _playerWeapon);
        }

        private void ReadData()
        {
            using StreamReader reader = new StreamReader(_fullSavesPath + WeaponFile);
            using JsonReader jsonReader = new JsonTextReader(reader);
            _playerWeapon = _serializer.Deserialize<PlayerWeapon?>(jsonReader) ?? new PlayerWeapon();
        }
    }
}