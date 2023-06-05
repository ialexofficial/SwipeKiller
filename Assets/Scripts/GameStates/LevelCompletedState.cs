using Cysharp.Threading.Tasks;
using Ji2Core.Core;
using Ji2Core.Core.States;

namespace GameStates
{
    public class LevelCompletedState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly Context _context;

        public LevelCompletedState(StateMachine stateMachine, Context context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }
        
        public async UniTask Enter()
        {
            _stateMachine.Enter<LoadingLevelState>();
        }

        public async UniTask Exit()
        {
        }
    }
}