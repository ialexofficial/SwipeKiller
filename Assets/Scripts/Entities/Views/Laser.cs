using Entities.ViewModels;
using UnityEngine;

namespace Entities.Views
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private float workTime = 5f;
        [SerializeField] private float sleepTime = 10f;
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private bool isControlledByTime = true;
        [SerializeField] private GameObject laserRay;
        [SerializeField] private LayerMask destroyableLayers;

        private LaserVM _viewModel;

        public void Construct(LaserVM viewModel)
        {
            _viewModel = viewModel;
        }
    }
}