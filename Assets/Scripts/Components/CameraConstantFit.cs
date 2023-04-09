using Cinemachine;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Keeps constant camera width instead of height, works for both Orthographic & Perspective cameras
    /// Made for tutorial https://youtu.be/0cmxFjP375Y
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraConstantFit : MonoBehaviour
    {
        [SerializeField] private Vector2 defaultResolution = new Vector2(720, 1280);
        [Range(0f, 1f)] [SerializeField] private float widthOrHeight = 0;
        [SerializeField] private float initialFov = 60;
#if UNITY_EDITOR
        [SerializeField] private bool disable = false;
        [SerializeField] private bool init = false;
#endif

        private CinemachineVirtualCamera _componentCamera;

        private float _initialSize;
        private float _initialFov;
        private float _targetAspect;

        private float _horizontalFov = 120f;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _componentCamera = GetComponent<CinemachineVirtualCamera>();
            _initialSize = _componentCamera.m_Lens.OrthographicSize;
            _initialFov = _componentCamera.m_Lens.FieldOfView;

            _targetAspect = defaultResolution.x / defaultResolution.y;

            _horizontalFov = CalcVerticalFov(initialFov, 1 / _targetAspect);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (init)
                Init();

            if (disable)
                if (!Application.isPlaying)
                        return;
#endif

            float currentAspect = Screen.orientation == ScreenOrientation.Portrait
                ? (float) Screen.height / Screen.width
                : (float) Screen.width / Screen.height;
            
            if (_componentCamera.m_Lens.Orthographic)
            {
                _componentCamera.m_Lens.OrthographicSize = _initialSize * currentAspect / _targetAspect;
            }
            else
            {
                _componentCamera.m_Lens.FieldOfView = _initialFov * currentAspect / _targetAspect;
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}