using Cysharp.Threading.Tasks;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using GUI.Views;
using Ji2.Presenters.Tutorial;
using Utilities;

namespace GameStates
{
    public class InitialState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ISaveDataContainer _saveDataContainer;
        private readonly WeaponDataProvider _weaponDataProvider;
        private readonly LevelDataProvider _levelDataProvider;
        private readonly ScreenNavigator _screenNavigator;
        private readonly TutorialService _tutorialService;

        public InitialState(
            StateMachine stateMachine, 
            ISaveDataContainer saveDataContainer, 
            WeaponDataProvider weaponDataProvider,
            LevelDataProvider levelDataProvider,
            ScreenNavigator screenNavigator,
            TutorialService tutorialService
        )
        {
            _stateMachine = stateMachine;
            _saveDataContainer = saveDataContainer;
            _weaponDataProvider = weaponDataProvider;
            _levelDataProvider = levelDataProvider;
            _screenNavigator = screenNavigator;
            _tutorialService = tutorialService;
        }
        
        public async UniTask Enter()
        {
            await _screenNavigator.PushScreen<LoadingScreen>();
            
            _saveDataContainer.Load();
            _levelDataProvider.Load();
            _weaponDataProvider.Load();

            _tutorialService.TryRunSteps();
            
            _stateMachine.Enter<LoadLevelState>();
        }

        public async UniTask Exit()
        {
            await _screenNavigator.CloseScreen<LoadingScreen>();
        }
    }
}