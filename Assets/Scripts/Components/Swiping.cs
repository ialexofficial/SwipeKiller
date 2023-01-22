using UnityEngine;
using UnityEngine.EventSystems;
using ViewModels;

namespace Components
{
    public class Swiping : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private SwipableViewModel[] swipables;

        private float _startTime = 0;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTime = Time.time;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            foreach (var swipable in swipables)
            {
                swipable.Swipe(eventData, Time.time - _startTime);
            }
        }
    }
}