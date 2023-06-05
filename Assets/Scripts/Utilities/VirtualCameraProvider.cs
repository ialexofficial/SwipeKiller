using Cinemachine;
using UnityEngine;

namespace Utilities
{
    public class VirtualCameraProvider : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CameraConstantFit cameraFit;

        private CinemachineFramingTransposer _transposer;

        public void SetCameraValues(
            float fieldOfView,
            Vector3 cameraPosition
        )
        {
            cameraFit.ChangeValues(initialFov: fieldOfView);
            virtualCamera.ForceCameraPosition(cameraPosition, virtualCamera.transform.rotation);
        }

        public void SetCamera(Transform followTarget)
        {
            virtualCamera.Follow = followTarget;
        }

        private void Start()
        {
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
}