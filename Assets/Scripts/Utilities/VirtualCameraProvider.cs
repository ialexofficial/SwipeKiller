using System;
using Cinemachine;
using UnityEngine;

namespace Utilities
{
    public class VirtualCameraProvider : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CameraConstantFit cameraFit;

        private CinemachineFramingTransposer _transposer;

        public Camera MainCamera => mainCamera;

        private void Start()
        {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void SetCameraValues(
            float fieldOfView,
            Vector3 cameraPosition
        )
        {
            cameraFit.ChangeValues(initialFov: fieldOfView);
            _transposer.m_CameraDistance = Math.Abs(cameraPosition.z);
            virtualCamera.ForceCameraPosition(cameraPosition, virtualCamera.transform.rotation);
        }

        public void SetCamera(Transform followTarget)
        {
            virtualCamera.Follow = followTarget;
        }
    }
}