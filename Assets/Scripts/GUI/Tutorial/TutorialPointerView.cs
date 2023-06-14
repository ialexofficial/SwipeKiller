using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GUI.Tutorial
{
    public class TutorialPointerView : MonoBehaviour
    {
        [SerializeField] private Image pointerImage;
        [SerializeField] private float animationDuration;
        [SerializeField] private Vector2 startOffset;
        [SerializeField] private Vector3 animationOffset;
        [SerializeField] private AnimationCurve animationCurve;
        
        private void Start()
        {
            Hide();
        }

        public async UniTask PlayAnimation(Vector2 startPosition, CancellationToken cancellationToken)
        {
            pointerImage.rectTransform.anchoredPosition = startPosition + startOffset;
            Show();

            var sequence = DOTween.Sequence()
                .Append(
                    pointerImage.transform
                        .DOScale(.85f, animationDuration * .5f)
                )
                .Append(
                    pointerImage.transform
                        .DOMove(
                            pointerImage.transform.position + animationOffset,
                            animationDuration)
                        .SetEase(animationCurve)
                )
                .Append(
                    pointerImage.transform
                        .DOScale(1.15f, animationDuration * .1f)
                )
                .SetUpdate(true);

            if (await UniTask.WaitUntil(
                        () => !sequence.IsActive() || sequence.IsComplete(),
                        cancellationToken: cancellationToken
                    )
                    .SuppressCancellationThrow()
               )
            {
                sequence.Kill();
            }

            await UniTask.Delay(
                    TimeSpan.FromSeconds(.5d), 
                    cancellationToken: cancellationToken
                )
                .SuppressCancellationThrow();

            Hide();
        }

        private void Show()
        {
            pointerImage.color = Color.white;
        }

        private void Hide()
        {
            pointerImage.color = new Color(0, 0, 0, 0);
        }
    }
}