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
        [SerializeField] private Transform animationTarget;
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
            Vector3 targetPosition = _camera.ScreenToWorldPoint(new Vector3(
                animationTarget.position.x,
                animationTarget.position.y,
                transform.position.z - _camera.transform.position.z
            ));
            float passedTime = 0;

            while (passedTime < animationTime)
            {
                passedTime += Time.unscaledDeltaTime;

                transform.position =
                    startPosition + (Vector3) (animationCurve.Evaluate(passedTime) * (targetPosition - startPosition));

                yield return null;
            }
            
            gameObject.SetActive(false);
            OnCollect.Invoke(cost);
        }
    }
}