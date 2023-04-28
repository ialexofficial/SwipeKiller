using GUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Components
{
    public class Swiping : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UnityEvent<PointerEventData, float> OnSwipe = new UnityEvent<PointerEventData, float>();

        private float _startTime = 0;
        private Camera _camera;
        private LevelGUI _levelGUI;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTime = Time.unscaledTime;
            _levelGUI.Started();
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnSwipe.Invoke(eventData, Time.unscaledTime - _startTime);
        }

        private void Start()
        {
            _camera = Camera.main;
            _levelGUI = FindObjectOfType<LevelGUI>();
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
        }
    }
}