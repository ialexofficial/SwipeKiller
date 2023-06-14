using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class SwipeLimiter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private float _startTime = 0;
        private Camera _camera;

        public Action<PointerEventData, float> OnSwipe;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void SetLimiter(Vector2 offset)
        {
            transform.localPosition = offset;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTime = Time.unscaledTime;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnSwipe?.Invoke(eventData, Time.unscaledTime - _startTime);
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
        }
    }
}