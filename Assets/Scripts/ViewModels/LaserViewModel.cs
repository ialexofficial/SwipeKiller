using Components;
using Models;
using UnityEngine;
using Utilities;

namespace ViewModels
{
    public class LaserViewModel : MonoBehaviour
    {
        [SerializeField] private float workTime = 5f;
        [SerializeField] private float sleepTime = 10f;
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private bool isControlledByTime = true;
        [SerializeField] private GameObject laserRay;
        [SerializeField] private LayerMask destroyableLayers;

        private LaserModel _model;

        public float WorkTime => workTime;
        public float SleepTime => sleepTime;
        public bool IsEnabled => isEnabled;
        public bool IsControlledByTime => isControlledByTime;

        public void Switch()
        {
            _model.Switch();
        }

        public void Switch(bool isEnabled)
        {
            _model.Switch(isEnabled);
        }

        private void Awake()
        {
            _model = new LaserModel(this);
            
            _model.OnSwitch += OnSwitched;
        }

        private void Start()
        {
            Switch(isEnabled);
        }

        private void Update()
        {
            _model.TickOnTime(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _model.OnSwitch -= OnSwitched;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!LayerMasker.CheckLayer(destroyableLayers, other.gameObject.layer))
                return;

            other.GetComponent<ICombustible>().BurnDown();
        }

        private void OnSwitched(bool isEnabled)
        {
            laserRay.SetActive(isEnabled);
        }
    }
}