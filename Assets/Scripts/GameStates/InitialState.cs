using Cysharp.Threading.Tasks;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using GUI.Views;

namespace GameStates
{
    public class InitialState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ISaveDataContainer _saveDataContainer;
        private readonly ScreenNavigator _screenNavigator;

        public InitialState(StateMachine stateMachine, ISaveDataContainer saveDataContainer, ScreenNavigator screenNavigator)
        {
            _stateMachine = stateMachine;
            _saveDataContainer = saveDataContainer;
            _screenNavigator = screenNavigator;
        }
        
        public async UniTask Enter()
        {
            await _screenNavigator.PushScreen<LoadingScreen>();
            
            _saveDataContainer.Load();
            
            _stateMachine.Enter<LoadingLevelState>();
        }

        public async UniTask Exit()
        {
            await _screenNavigator.CloseScreen<LoadingScreen>();
        }
    }
}