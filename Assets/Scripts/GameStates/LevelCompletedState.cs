using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entities.Views;
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
            _context.Unregister<List<Enemy>>();
            _context.Unregister<List<Coin>>();
    
            _stateMachine.Enter<LoadLevelState>();
        }

        public async UniTask Exit()
        {
        }
    }
}