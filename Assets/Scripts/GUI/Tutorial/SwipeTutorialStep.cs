using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Entities.Views.Weapon;
using GameStates;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core.States;
using UnityEngine;
using Utilities;

namespace GUI.Tutorial
{
    public class SwipeTutorialStep : ITutorialStep
    {
        private const int TipDelay = 15;
        private readonly StateMachine _stateMachine;
        private readonly LevelDataProvider _levelDataProvider;
        private readonly TutorialPointerView _pointerView;
        private readonly Camera _camera;
        private bool _isGameState;
        
        public event Action Completed;
        
        public string SaveKey => "";

        public SwipeTutorialStep(
            StateMachine stateMachine, 
            LevelDataProvider levelDataProvider,
            TutorialPointerView pointerView,
            Camera camera
        )
        {
            _stateMachine = stateMachine;
            _levelDataProvider = levelDataProvider;
            _pointerView = pointerView;
            _camera = camera;
        }
        
        public void Run()
        {
            _stateMachine.StateEntered += state => OnStateEntered(state);
        }

        private async UniTaskVoid OnStateEntered(IExitableState state)
        {
            if (state is LevelCompletedState)
            {
                _isGameState = false;
            }

            if (state is not GameState gameState)
                return;

            BaseWeapon weapon = gameState.GameStatePayload.Weapon;

            await UniTask.WaitUntil(() => gameState.IsReady);
            await UniTask.Yield(PlayerLoopTiming.Update);

            _isGameState = true;

            if (_levelDataProvider.IsFirstLevel)
            {
                await LoopTipShowing(weapon);
            }

            DelayBeforeTip(weapon);
        }

        private async UniTaskVoid DelayBeforeTip(BaseWeapon weapon)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            weapon.OnForceAdd += cancellationTokenSource.Cancel;

            while (_isGameState)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TipDelay),
                    cancellationToken: cancellationTokenSource.Token
                );

                await LoopTipShowing(weapon);
            }

            weapon.OnForceAdd -= cancellationTokenSource.Cancel;
        }

        private async UniTask LoopTipShowing(BaseWeapon weapon)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            weapon.OnForceAdd += cancellationTokenSource.Cancel;

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await ShowTip(weapon, cancellationTokenSource.Token);
            }

            weapon.OnForceAdd -= cancellationTokenSource.Cancel;
        }

        private async UniTask ShowTip(BaseWeapon weapon, CancellationToken cancellationToken)
        {
            await _pointerView.PlayAnimation(
                _camera.WorldToScreenPoint(weapon.transform.position),
                cancellationToken
            );
        }
    }
}