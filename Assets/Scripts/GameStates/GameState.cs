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
using Entities.Views.Weapon;
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
        private readonly ParticlesProvider _particleProvider;
        private LevelModel _levelModel;
        private LevelView _levelView;
        private readonly ISaveDataContainer _saveDataContainer;
        private GameStatePayload _payload;

        public GameStatePayload GameStatePayload => _payload;
        public bool IsReady { get; private set; }

        public GameState(StateMachine stateMachine, Context context)
        {
            _stateMachine = stateMachine;
            _context = context;
            
            _screenNavigator = _context.GetService<ScreenNavigator>();
            _timeScaler = _context.GetService<TimeScaler>();
            _particleProvider = _context.GetService<ParticlesProvider>();
            _saveDataContainer = _context.GetService<ISaveDataContainer>();
        }

        public async UniTask Enter(GameStatePayload payload)
        {
            _payload = payload;
            
            _levelModel = GameStatePayload.LevelModel;
            _levelView = GameStatePayload.LevelView;
            
            GameScreenModel screenModel = new GameScreenModel(
                GameStatePayload.Enemies,
                GameStatePayload.SwipeCount,
                _timeScaler,
                _saveDataContainer
            );

            _levelModel.OnLevelWin += screenModel.Win;
            _levelModel.OnLevelLose += screenModel.Lose;
            
            GameScreenVM screenVM = new GameScreenVM(screenModel, _levelModel, GameStatePayload.MoneyDataModel);
            GameScreen gameScreen = await _screenNavigator.PushScreen<GameScreen>();
            gameScreen.Construct(screenVM, _particleProvider.ParticleSystem);
            
            screenVM.Bootstrap();

            foreach (var coin in GameStatePayload.Coins)
            {
                coin.Construct(gameScreen.CoinAnimationTarget);
                coin.OnAnimationEnd += screenVM.EarnMoney;
            }

            _levelModel.OnLevelLoad += LoadNextLevel;

            IsReady = true;
        }

        public async UniTask Exit()
        {
            _particleProvider.ParticleSystem.Clear();
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
        public BaseWeapon Weapon { get; }
        public MoneyDataModel MoneyDataModel { get; }
        public LevelModel LevelModel { get; }
        public LevelView LevelView { get; }
        public int SwipeCount { get; }
        public List<Enemy> Enemies { get; }
        public List<Coin> Coins { get; }

        public GameStatePayload(
            BaseWeapon weapon,
            MoneyDataModel moneyDataModel,
            LevelModel levelModel,
            LevelView levelView,
            int swipeCount,
            List<Enemy> enemies,
            List<Coin> coins
        )
        {
            Weapon = weapon;
            MoneyDataModel = moneyDataModel;
            LevelModel = levelModel;
            LevelView = levelView;
            SwipeCount = swipeCount;
            Enemies = enemies;
            Coins = coins;
        }
    }
}