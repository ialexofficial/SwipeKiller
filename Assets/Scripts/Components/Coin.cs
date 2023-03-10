using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Components
{
    public class Coin : MonoBehaviour
    {
        public UnityEvent<int> OnCollect = new UnityEvent<int>();
        [SerializeField] private int cost;
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private LayerMask weaponLayer;
        
        [Header("Animation of collecting")]
        [SerializeField] private RectTransform animationTarget;
        [SerializeField] private float animationTime;
        [SerializeField] private AnimationCurve animationCurve;

        private Camera _camera;
        private bool _isCollected = false;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isCollected || !LayerMasker.CheckLayer(weaponLayer, other.gameObject.layer))
                return;

            _isCollected = true;
            StartCoroutine(AnimateCollection());
        }

        private IEnumerator AnimateCollection()
        {
            Vector3 startPosition = transform.position;
            float passedTime = 0;

            while (passedTime < animationTime)
            {
                Vector2 targetPosition = _camera.ScreenToWorldPoint(animationTarget.transform.position);
                passedTime += Time.unscaledDeltaTime;

                transform.position =
                    startPosition + (Vector3) (animationCurve.Evaluate(passedTime) * (targetPosition - (Vector2) startPosition));

                yield return null;
            }
            
            gameObject.SetActive(false);
            OnCollect.Invoke(cost);
        }
    }
}