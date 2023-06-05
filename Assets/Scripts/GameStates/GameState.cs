using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Ji2.CommonCore.SaveDataContainer;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Level.Models;
using Entities.Views;
using Level.Views;
using Utilities;

namespace GameStates
{
    public class GameState : IPayloadedState<GameStatePayload>
    {
        private readonly StateMachine _stateMachine;
        private readonly Context _context;
        private readonly ScreenNavigator _screenNavigator;
        private readonly TimeScaler _timeScaler;
        private readonly SwipeConfiner _swipeConfiner;
        private readonly ParticlesProvider _particleProvider;
        private LevelModel _levelModel;
        private LevelView _levelView;
        private readonly ISaveDataContainer _saveDataContainer;

        public GameState(StateMachine stateMachine, Context context)
        {
            _stateMachine = stateMachine;
            _context = context;
            
            _screenNavigator = _context.GetService<ScreenNavigator>();
            _timeScaler = _context.GetService<TimeScaler>();
            _swipeConfiner = _context.GetService<SwipeConfiner>();
            _particleProvider = _context.GetService<ParticlesProvider>();
            _saveDataContainer = _context.GetService<ISaveDataContainer>();
        }

        public async UniTask Enter(GameStatePayload payload)
        {
            _levelModel = payload.LevelModel;
            _levelView = payload.LevelView;
            
            GameScreenModel screenModel = new GameScreenModel(
                payload.Enemies,
                payload.SwipeCount,
                _timeScaler,
                _saveDataContainer
            );

            _levelModel.OnLevelWin += screenModel.Win;
            _levelModel.OnLevelLose += screenModel.Lose;
            
            GameScreenVM screenVM = new GameScreenVM(screenModel, _levelModel, payload.MoneyDataModel);
            GameScreen gameScreen = await _screenNavigator.PushScreen<GameScreen>();
            gameScreen.Construct(screenVM, _particleProvider.ParticleSystem);
            
            screenVM.Bootstrap();

            foreach (var coin in payload.Coins)
            {
                coin.Construct(gameScreen.CoinAnimationTarget);
                coin.OnAnimationEnd += screenVM.EarnMoney;
            }

            _levelModel.OnLevelLoad += LoadNextLevel;
        }

        public async UniTask Exit()
        {
            _particleProvider.ParticleSystem.Clear();
            _swipeConfiner.Clear();
            _levelView.Clear();
        }

        private void LoadNextLevel()
        {
            _levelModel.OnLevelLoad -= LoadNextLevel;
            
            _stateMachine.Enter<LevelCompletedState>();
        }
    }

    public class GameStatePayload
    {
        public MoneyDataModel MoneyDataModel { get; }
        public LevelModel LevelModel { get; }
        public LevelView LevelView { get; }
        public int SwipeCount { get; }
        public List<Enemy> Enemies { get; }
        public List<Coin> Coins { get; }

        public GameStatePayload(
            MoneyDataModel moneyDataModel,
            LevelModel levelModel,
            LevelView levelView,
            int swipeCount,
            List<Enemy> enemies,
            List<Coin> coins
        )
        {
            MoneyDataModel = moneyDataModel;
            LevelModel = levelModel;
            LevelView = levelView;
            SwipeCount = swipeCount;
            Enemies = enemies;
            Coins = coins;
        }
    }
}