using GameStates;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using UnityEngine;
using Utilities;

namespace SwipeKiller
{
    public class Bootstrap : BootstrapBase
    {
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private UpdateService updateService;
        [SerializeField] private VirtualCameraProvider cameraProvider;
        [SerializeField] private WeaponDataProvider weaponDataProvider;
        [SerializeField] private LevelDataProvider levelDataProvider;
        [SerializeField] private SwipeConfiner swipeConfiner;
        [SerializeField] private ParticlesProvider particlesProvider;
        
        private readonly Context _context = Context.GetInstance();

        private StateMachine _gameStateMachine;

        protected override void Start()
        {
            DontDestroyOnLoad(this);
            
            InstallCameraProvider();
            InstallSaveDataContainer();
            InstallWeaponDataProvider();
            InstallUpdateService();
            InstallLevelDataProvider();
            InstallSwipeConfiner();
            InstallSceneLoader();
            InstallScreenNavigator();
            InstallTimeScaler();
            InstallParticlesProvider();
            InstallStateMachine();
            
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;

            _gameStateMachine.Load();
            _gameStateMachine.Enter<InitialState>();
        }

        private void InstallSaveDataContainer()
        {
            _context.Register<ISaveDataContainer>(new PlayerPrefsSaveDataContainer());
        }

        private void InstallWeaponDataProvider()
        {
            _context.Register(weaponDataProvider);
        }

        private void InstallUpdateService()
        {
            _context.Register(updateService);
        }

        private void InstallLevelDataProvider()
        {
            _context.Register(levelDataProvider);
        }

        private void InstallSwipeConfiner()
        {
            _context.Register(swipeConfiner);
        }

        private void InstallSceneLoader()
        {
            _context.Register(new SceneLoader(updateService));
        }

        public void InstallCameraProvider()
        {
            _context.Register(cameraProvider);
        }

        private void InstallScreenNavigator()
        {
            screenNavigator.Bootstrap();
            _context.Register(screenNavigator);
        }

        private void InstallStateMachine()
        {
            _gameStateMachine = new StateMachine(new StateFactory(_context));
            _context.Register(_gameStateMachine);
        }

        private void InstallTimeScaler()
        {
            _context.Register(new TimeScaler());
        }

        private void InstallParticlesProvider()
        {
            _context.Register(particlesProvider);
        }
    }
}