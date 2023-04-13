using Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ViewModels
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwipableViewModel : MonoBehaviour
    {
        public UnityEvent<Vector2> OnForceAdd = new UnityEvent<Vector2>();
        [SerializeField] private float swipeStrength = 5f;
        [SerializeField] private float maxVelocity = 5f;
        [SerializeField] private float minTimeInteraction = 0.001f;
        [SerializeField] private float maxTimeInteraction = 1f;
        [Range(0.1f, 0, order = -1)]
        [SerializeField] private float swipeDeadZone = 0.1f;
        
        private SwipableModel _model;
        private Rigidbody _rigidbody;
        private float _startZPosition;
        private Vector3 _startRotation;

        public float SwipeStrength
        {
            get => swipeStrength;
            set => swipeStrength = value;
        }
        public float MaxVelocity
        {
            get => maxVelocity;
            set => maxVelocity = value;
        }
        public float MinTimeInteraction
        {
            get => minTimeInteraction;
            set => minTimeInteraction = value;
        }
        public float MaxTimeInteraction => maxTimeInteraction;
        public Vector3 Velocity => _rigidbody.velocity;
        public float SwipeDeadZone => swipeDeadZone;

        public void Swipe(PointerEventData eventData, float swipeTime)
        {
            _model.Swipe(eventData, swipeTime);
        }

        private void Awake()
        {
            _model = new SwipableModel(this);

            _model.OnSwipe += OnSwiped;
        }

        private void OnDestroy()
        {
            _model.OnSwipe -= OnSwiped;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _startZPosition = transform.position.z;
            _startRotation = transform.eulerAngles;
        }

        private void FixedUpdate()
        {
            _model.FixedUpdate();
            
            /*Vector3 rotation = transform.eulerAngles;
            rotation.x = _startRotation.x;
            rotation.y = _startRotation.y;
            transform.eulerAngles = rotation;

            Vector3 position = transform.position;
            position.z = _startZPosition;
            transform.position = position;*/
        }

        private void OnSwiped(Vector2 delta)
        {
            _rigidbody.AddForce(delta, ForceMode.VelocityChange);
            OnForceAdd.Invoke(delta);
        }
    }
}