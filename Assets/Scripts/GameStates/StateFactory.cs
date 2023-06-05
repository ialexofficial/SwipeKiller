using System;
using System.Collections.Generic;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;

namespace GameStates
{
    public class StateFactory : IStateFactory
    {
        private readonly Context _context;

        public StateFactory(Context context)
        {
            _context = context;
        }

        public Dictionary<Type, IExitableState> GetStates(StateMachine stateMachine)
        {
            ISaveDataContainer saveDataContainer = _context.SaveDataContainer;
            ScreenNavigator screenNavigator = _context.ScreenNavigator;

            var result = new Dictionary<Type, IExitableState>
            {
                [typeof(InitialState)] = new InitialState(stateMachine, saveDataContainer, screenNavigator),
                [typeof(LoadingLevelState)] = new LoadingLevelState(stateMachine, _context),
                [typeof(GameState)] = new GameState(stateMachine, _context),
                [typeof(LevelCompletedState)] = new LevelCompletedState(stateMachine, _context)
            };

            return result;
        }
    }
}