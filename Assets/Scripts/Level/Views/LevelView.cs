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
        private readonly Context _context;
        private readonly LevelModel _levelModel;
        private readonly MoneyDataModel _moneyDataModel;
        private readonly WeaponVM _weaponVM;
        private readonly SwipableModel _swipableModel;
        private readonly WeaponDataModel _weaponDataModel;
        private readonly VirtualCameraProvider _cameraProvider;
        private BaseWeapon _weapon;

        public LevelView(
            Context context,
            LevelModel levelModel, 
            MoneyDataModel moneyDataModel,
            WeaponDataModel weaponDataModel,
            WeaponVM weaponVM,
            SwipableModel swipableModel,
            VirtualCameraProvider cameraProvider
        )
        {
            _context = context;
            _levelModel = levelModel;
            _moneyDataModel = moneyDataModel;
            _weaponDataModel = weaponDataModel;
            _weaponVM = weaponVM;
            _swipableModel = swipableModel;
            _cameraProvider = cameraProvider;

            _levelModel.OnLevelWin += OnLevelWon;
        }

        public void Bootstrap()
        {
            BootstrapWeapon();
        }

        private void BootstrapWeapon()
        {
            _weapon = Object.Instantiate(
                _weaponDataModel.SelectedWeapon,
                _weaponDataModel.WeaponSpawnPoint,
                Quaternion.identity
            );

            _weapon.OnWeaponDestroy += _levelModel.OnPlayerDead;
            _cameraProvider.SetCamera(_weapon.transform);

            var swipeLimiter = Object.Instantiate(
                _weaponDataModel.SwipeSettings.SwipeLimiterPrefab,
                _weapon.transform
            );
            swipeLimiter.SetLimiter(_weaponDataModel.SwipeLimiterOffset);
            swipeLimiter.OnSwipe += _weapon.Swipe;
            
            _weapon.Construct(_weaponVM);

            _context.Register(_weapon);
        }

        private void OnLevelWon()
        {
            _moneyDataModel.SaveData();
        }

        public void Clear()
        {
            _swipableModel.Clear();
            _context.Unregister<BaseWeapon>();
        }
    }
}