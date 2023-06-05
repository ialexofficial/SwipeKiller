using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class SwipeConfiner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private float _startTime = 0;
        private Camera _camera;

        public Action<PointerEventData, float> OnSwipe;

        public void SetConfiner(Transform parent)
        {
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
        }

        public void Clear()
        {
            transform.parent = null;
            DontDestroyOnLoad(this);
            OnSwipe = null;
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

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!(_camera is null))
            {
                transform.LookAt(_camera.transform);
            }
        }
    }
}