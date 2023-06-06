using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entities.Models;
using Entities.ViewModels;
using Entities.Views;
using GUI.Views;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Level;
using Level.Models;
using Level.Views;
using UnityEngine;
using Utilities;

namespace GameStates
{
    public class LoadingLevelState : IState
    {
        private const int LoadingExitDelay = 1500;
        private readonly StateMachine _stateMachine;
        private readonly Context _context;
        private readonly ScreenNavigator _screenNavigator;
        private readonly LevelDataProvider _levelDataProvider;
        private readonly WeaponDataProvider _weaponDataProvider;
        private readonly SceneLoader _sceneLoader;
        private readonly UpdateService _updateService;
        private readonly VirtualCameraProvider _cameraProvider;
        private readonly TimeScaler _timeScaler;

        public LoadingLevelState(StateMachine stateMachine, Context context)
        {
            _stateMachine = stateMachine;
            _context = context;

            _screenNavigator = _context.GetService<ScreenNavigator>();
            _levelDataProvider = _context.GetService<LevelDataProvider>();
            _weaponDataProvider = _context.GetService<WeaponDataProvider>();
            _sceneLoader = _context.GetService<SceneLoader>();
            _updateService = _context.GetService<UpdateService>();
            _cameraProvider = _context.GetService<VirtualCameraProvider>();
            _timeScaler = _context.GetService<TimeScaler>();
        }

        public async UniTask Enter()
        {
            await _screenNavigator.PushScreen<LoadingScreen>();
            
            _levelDataProvider.Bootstrap();
            _weaponDataProvider.Bootstrap();

            LevelConfig levelConfig = _levelDataProvider.CurrentLevel;
            await _sceneLoader.LoadScene(levelConfig.LevelSceneName);
            
            GameStatePayload payload = BuildLevel(levelConfig);

            _stateMachine.Enter<GameState, GameStatePayload>(payload);
        }

        public async UniTask Exit()
        {
            await UniTask.Delay(LoadingExitDelay);
            await _screenNavigator.CloseScreen<LoadingScreen>();
        }

        private GameStatePayload BuildLevel(LevelConfig levelConfig)
        {
            List<Enemy> enemies = Object.FindObjectsOfType<Enemy>()
                .ToList();
            List<Coin> coins = Object.FindObjectsOfType<Coin>()
                .ToList();

            foreach (var enemy in enemies)
            {
                enemy.Construct(new EnemyVM(new EnemyModel(
                    enemy.Health,
                    enemy.HeadCollider
                )));
            }

            _cameraProvider.SetCameraValues(
                levelConfig.FieldOfView,
                levelConfig.CameraPosition
            );

            LevelModel levelModel = new LevelModel(
                _context.GetService<TimeScaler>(), 
                levelConfig.SwipeCount,
                enemies,
                _levelDataProvider
            );
            WeaponDataModel weaponDataModel = new WeaponDataModel(_weaponDataProvider, levelConfig.WeaponSpawnPoint);
            MoneyDataModel moneyDataModel = new MoneyDataModel(_context.GetService<ISaveDataContainer>(), coins);

            SwipableModel swipableModel = new SwipableModel(weaponDataModel.SwipeSettings, _updateService);
            SlowMotionModel slowMotionModel = new SlowMotionModel(weaponDataModel.SlowMotionSettings, _timeScaler, swipableModel);
            WeaponVM weaponVM = new WeaponVM(slowMotionModel, swipableModel);

            LevelView level = new LevelView(
                levelModel,
                moneyDataModel,
                weaponDataModel,
                weaponVM,
                swipableModel,
                _cameraProvider,
                _context.GetService<SwipeConfiner>()
            );
            level.Bootstrap();

            return new GameStatePayload(
                moneyDataModel,
                levelModel,
                level,
                levelConfig.SwipeCount,
                enemies,
                coins
            );
        }
    }
}