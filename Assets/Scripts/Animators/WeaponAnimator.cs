using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Animators
{
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimator : MonoBehaviour
    {
        private Animator _animator;
        private EventSystem _eventSystem;
        private PhysicsRaycaster _physicsRaycaster;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended)
                    continue;

                _eventSystem ??= EventSystem.current;
                _physicsRaycaster ??= Camera.main.GetComponent<PhysicsRaycaster>();

                PointerEventData pointerEventData = new PointerEventData(_eventSystem)
                {
                    position = touch.position
                };
                List<RaycastResult> raycastResults = new List<RaycastResult>();

                _physicsRaycaster.Raycast(pointerEventData, raycastResults);

                if (raycastResults.Any(result => result.gameObject == gameObject))
                {
                    _animator.SetTrigger("Flip");
                    break;
                }
            }
        }
    }
}