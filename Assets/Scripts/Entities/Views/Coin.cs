using System;
using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Entities.Views
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int cost;
        [SerializeField] private LayerMask weaponLayer;
        
        [Header("Animation of collecting")]
        [SerializeField] private float animationTime;
        [SerializeField] private Vector3 animationOffset = new Vector3(-1f, -1f, 0);
        [SerializeField] private AnimationCurve animationCurve;

        private Camera _camera;
        private bool _isCollected = false;
        private Transform _animationTarget;

        public event Action<int> OnCollect;
        public event Action OnAnimationEnd;

        public void Construct(Transform animationTarget)
        {
            _animationTarget = animationTarget;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isCollected || !LayerMasker.CheckLayer(weaponLayer, other.gameObject.layer))
                return;

            _isCollected = true;
            OnCollect?.Invoke(cost);

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(_animationTarget.position);
            targetPosition.z = transform.position.z;

            transform
                .DOMove(targetPosition + animationOffset, animationTime)
                .SetEase(animationCurve)
                .onComplete += () =>
            {
                gameObject.SetActive(false);
                OnAnimationEnd?.Invoke();
            };
        }
    }
}