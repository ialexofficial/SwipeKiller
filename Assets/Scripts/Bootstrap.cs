using GameStates;
using GUI.Tutorial;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Level;
using Managers;
using UnityEngine;
using Utilities;

namespace SwipeKiller
{
    public class Bootstrap : BootstrapBase
    {
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private UpdateService updateService;
        [SerializeField] private VirtualCameraProvider cameraProvider;
        [SerializeField] private WeaponDatabase weaponDatabase;
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private ParticlesProvider particlesProvider;
        [SerializeField] private TutorialPointerView tutorialPointerView;
        
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
            InstallSceneLoader();
            InstallScreenNavigator();
            InstallTimeScaler();
            InstallParticlesProvider();
            InstallStateMachine();
            InstallTutorial();
            
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
            _context.Register(new WeaponDataProvider(
                _context.SaveDataContainer,
                weaponDatabase
            ));
        }

        private void InstallUpdateService()
        {
            _context.Register(updateService);
        }

        private void InstallLevelDataProvider()
        {
            _context.Register(new LevelDataProvider(
                _context.SaveDataContainer,
                levelDatabase
            ));
        }

        private void InstallSceneLoader()
        {
            _context.Register(new SceneLoader(updateService));
        }

        private void InstallCameraProvider()
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

        private void InstallTutorial()
        {
            _context.Register(tutorialPointerView);
            
            ITutorialFactory tutorialFactory = new TutorialFactory(_context);
            TutorialService tutorialService = new TutorialService(
                _context.GetService<ISaveDataContainer>(),
                new[]
                {
                    tutorialFactory.Create<SwipeTutorialStep>()
                }
            );
            
            _context.Register(tutorialService);
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