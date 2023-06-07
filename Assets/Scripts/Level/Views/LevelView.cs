using Entities.Models;
using Entities.ViewModels;
using Entities.Views.Weapon;
using Ji2Core.Core;
using Level.Models;
using UnityEngine;
using Utilities;

namespace Level.Views
{
    public class LevelView : IBootstrapable
    {
        private readonly LevelModel _levelModel;
        private readonly MoneyDataModel _moneyDataModel;
        private readonly WeaponVM _weaponVM;
        private readonly SwipableModel _swipableModel;
        private readonly WeaponDataModel _weaponDataModel;
        private readonly VirtualCameraProvider _cameraProvider;
        private readonly SwipeConfiner _swipeConfiner;
        private BaseWeapon _weapon;

        public LevelView(
            LevelModel levelModel, 
            MoneyDataModel moneyDataModel,
            WeaponDataModel weaponDataModel,
            WeaponVM weaponVM,
            SwipableModel swipableModel,
            VirtualCameraProvider cameraProvider,
            SwipeConfiner swipeConfiner
        )
        {
            _levelModel = levelModel;
            _moneyDataModel = moneyDataModel;
            _weaponDataModel = weaponDataModel;
            _weaponVM = weaponVM;
            _swipableModel = swipableModel;
            _cameraProvider = cameraProvider;
            _swipeConfiner = swipeConfiner;

            _levelModel.OnLevelWin += OnLevelWon;
        }

        public void Bootstrap()
        {
            BootstrapWeapon();
        }

        private void BootstrapWeapon()
        {
            _weapon = GameObject.Instantiate(
                _weaponDataModel.SelectedWeapon,
                _weaponDataModel.WeaponSpawnPoint,
                Quaternion.identity
            );

            _weapon.OnWeaponDestroy += _weaponDataModel.OnWeaponDestroyed;
            _cameraProvider.SetCamera(_weapon.transform);
            
            _weapon.Construct(_weaponVM);
            
            _swipeConfiner.SetConfiner(_weapon.transform, _weaponDataModel.SwipeConfinerOffset);
            _swipeConfiner.OnSwipe += _weapon.Swipe;
        }

        private void OnLevelWon()
        {
            _moneyDataModel.SaveData();
        }

        public void Clear()
        {
            _swipableModel.Clear();
        }
    }
}