using Cysharp.Threading.Tasks;
using GUI.ViewModels;
using Ji2Core.UI.Screens;
using SwipeKiller;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GUI.Views
{
    public class GameScreen : BaseScreen
    {
        [Header("Menus")] 
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject loseMenu;

        [Header("Animation")] 
        [SerializeField] private Animator animator;
        [SerializeField] private float animationDelay = 1f;

        [Header("Particles")]
        [Tooltip("Calculated by taking the entered percent of animationDelay")]
        [Range(0, 1)] [SerializeField] private float particlesAnimationDelay;

        [Header("Game Info")] 
        [SerializeField] private TMP_Text enemyCountField;
        [SerializeField] private TMP_Text swipeCountField;
        [SerializeField] private GameObject swipeInfinityImage;
        [SerializeField] private TMP_Text moneyAmountField;
        [SerializeField] private Transform coinAnimationTarget;

        [FormerlySerializedAs("soundEnabledImage")]
        [Header("Settings")] 
        [SerializeField] private GameObject soundDisabledImage;
        [FormerlySerializedAs("vibrationEnabledImage")] [SerializeField] private GameObject vibrationDisabledImage;
        
        private GameScreenVM _viewModel;
        private ParticleSystem _winParticles;
        private Animator _coinAnimator;

        public Transform CoinAnimationTarget => coinAnimationTarget;

        public void Construct(GameScreenVM viewModel, ParticleSystem winParticles)
        {
            _winParticles = winParticles;
            _viewModel = viewModel;

            _viewModel.OnEnemyCountChange += OnEnemyCountChanged;
            _viewModel.OnSwipeCountChange += OnSwipeCountChanged;
            _viewModel.OnMoneyAmountChange += OnMoneyAmountChanged;
            _viewModel.OnWin += () => ShowWinMenu();
            _viewModel.OnLose += () => ShowLoseMenu();
            _viewModel.OnToggleSettings += isActive => settingsMenu.SetActive(isActive);
            _viewModel.OnSoundToggle += OnSoundToggled;
            _viewModel.OnVibrationToggle += OnVibrationToggled;
        }

        public void ClickSettingsButton()
        {
            _viewModel.ClickSettingsButton();
        }

        public void ClickCloseSettingsButton()
        {
            _viewModel.ClickCloseSettingsButton();
        }

        public void ClickReloadButton()
        {
            _viewModel.ClickReloadButton();
        }

        public void ClickVolumeButton()
        {
            _viewModel.ClickVolumeButton();
        }

        public void ClickVibrationButton()
        {
            _viewModel.ClickVibrationButton();
        }

        public void ClickNextLevelButton()
        {
            _viewModel.ClickNextLevelButton();
        }

        public void ClickMainMenuButton()
        {
            _viewModel.ClickMainMenuButton();
        }

        private void Start()
        {
            _coinAnimator = coinAnimationTarget.GetComponent<Animator>();
            
            animator.SetTrigger(Constants.GAMESCREEN_GAME_START_TRIGGER);
            
            moneyAmountField.text = _viewModel.MoneyAmount.ToString();
            OnSwipeCountChanged(_viewModel.SwipeCount);
        }

        private async UniTaskVoid ShowWinMenu()
        {
            winMenu.SetActive(true);

            await UniTask.Delay((int) (animationDelay * particlesAnimationDelay * 1000), ignoreTimeScale: true);

            _winParticles.Play();

            await UniTask.Delay((int) (animationDelay * (1 - particlesAnimationDelay) * 1000), ignoreTimeScale: true);

            animator.SetTrigger(Constants.GAMESCREEN_GAME_END_TRIGGER);
        }

        private async UniTask ShowLoseMenu()
        {
            loseMenu.SetActive(true);

            await UniTask.Delay((int) (animationDelay * 1000), ignoreTimeScale: true);

            animator.SetTrigger(Constants.GAMESCREEN_GAME_END_TRIGGER);
        }

        private void OnMoneyAmountChanged()
        {
            moneyAmountField.text = _viewModel.MoneyAmount.ToString();

            _coinAnimator.SetTrigger(Constants.COIN_EARN_TRIGGER);
        }

        private void OnEnemyCountChanged(int count)
        {
            enemyCountField.text = $"x{count}";
        }

        private void OnSwipeCountChanged(int count)
        {
            if (count == -1)
            {
                swipeCountField.gameObject.SetActive(false);
                swipeInfinityImage.SetActive(true);
            }
            else
            {
                swipeInfinityImage.SetActive(false);
                swipeCountField.gameObject.SetActive(true);
                swipeCountField.text = count.ToString();
            }
        }

        private void OnSoundToggled(bool isActive)
        {
            soundDisabledImage.SetActive(!isActive);
        }

        private void OnVibrationToggled(bool isActive)
        {
            vibrationDisabledImage.SetActive(!isActive);
        }
    }
}