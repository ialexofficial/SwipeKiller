using System;
using Entities.Models;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Entities.ViewModels
{
    public class WeaponVM
    {
        private readonly SlowMotionModel _slowMotionModel;
        private readonly SwipableModel _swipableModel;

        public event Action<Vector2> OnForceAdd;

        public WeaponVM(
            SlowMotionModel slowMotionModel,
            SwipableModel swipableModel
        )
        {
            _slowMotionModel = slowMotionModel;
            _swipableModel = swipableModel;

            _swipableModel.OnForceAdd += delta => OnForceAdd?.Invoke(delta);
        }

        public void OnUpdate(float currentSpeed)
        {
            _slowMotionModel.OnUpdate(currentSpeed);
        }

        public void Swipe(
            PointerEventData pointerEventData, 
            float swipeTime,
            Vector3 position
        )
        {
            _swipableModel.Swipe(pointerEventData, swipeTime, position);
        }
    }
}