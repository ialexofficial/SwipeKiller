using Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ViewModels
{
    [RequireComponent(typeof(Rigidbody))]
    public class SwipableViewModel : MonoBehaviour
    {
        [SerializeField] private float swipeStrength = 5f;
        [SerializeField] private float maxVelocity = 5f;
        [SerializeField] private float minTimeInteraction = 0.001f;
        [SerializeField] private float maxTimeInteraction = 1f;
        
        private SwipableModel _model;
        private Rigidbody _rigidbody;

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


        public void Swipe(PointerEventData eventData, float swipeTime)
        {
            Debug.Log($"Swipe time: {swipeTime}");
            _model.Swipe(eventData, swipeTime);
        }

        private void Awake()
        {
            _model = new SwipableModel(this);

            _model.OnSwipe += Swiped;
        }

        private void OnDestroy()
        {
            _model.OnSwipe -= Swiped;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Swiped(Vector2 delta)
        {
            _rigidbody.AddForce(delta, ForceMode.VelocityChange);
        }
    }
}