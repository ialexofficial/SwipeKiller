using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Components
{
    public class Swiping : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UnityEvent<PointerEventData, float> OnSwipe = new UnityEvent<PointerEventData, float>();

        private float _startTime = 0;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTime = Time.unscaledTime;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnSwipe.Invoke(eventData, Time.unscaledTime - _startTime);
        }
    }
}