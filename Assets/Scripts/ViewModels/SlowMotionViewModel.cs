using System.Collections;
using Models;
using UnityEngine;

namespace ViewModels
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlowMotionViewModel : MonoBehaviour
    {
        [SerializeField] private float scaleFactor = 0.1f;
        [SerializeField] private float startSpeed = 1f;
        [SerializeField] private float slowingTime = 1f;
        [SerializeField] private AnimationCurve slowingTimeline;

        private SlowMotionModel _model;
        private Rigidbody _rigidbody;
        private Coroutine _scaleTimeCoroutine;

        public float ScaleFactor => scaleFactor;
        public float StartSpeed => startSpeed;
        public float SlowingTime => slowingTime;
        public AnimationCurve SlowingTimeline => slowingTimeline;
        

        public void OnForceAdded()
        {
            _model.OnForceAdded();
        }

        private void Awake()
        {
            _model = new SlowMotionModel(this);

            _model.OnSlowMotionStart += OnSlowMotionStarted;
            _model.OnTimeScale += OnTimeScaled;
            _model.OnSlowMotionEnd += OnSlowMotionEnded;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Destroy()
        {
            _model.OnSlowMotionStart -= OnSlowMotionStarted;
            _model.OnTimeScale -= OnTimeScaled;
            _model.OnSlowMotionEnd -= OnSlowMotionEnded;
        }

        private void FixedUpdate()
        {
            Vector2 currentVelocity = _rigidbody.velocity;

            _model.CheckSpeed(currentVelocity.magnitude);
        }

        private void OnSlowMotionStarted()
        {
            _scaleTimeCoroutine = StartCoroutine(ScaleTime());
        }

        private IEnumerator ScaleTime()
        {
            while (true)
            {
                if (GameManager.IsGamePaused)
                    yield return new WaitUntil(() => !GameManager.IsGamePaused);

                _model.ScaleTime();
                yield return null;
            }
        }

        private void OnTimeScaled(float timeScale, float fixedDeltaTime)
        {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = fixedDeltaTime;
        }

        private void OnSlowMotionEnded()
        {
            StopCoroutine(_scaleTimeCoroutine);
        }
    }
}