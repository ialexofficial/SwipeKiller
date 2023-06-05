using Cinemachine;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Keeps constant camera width instead of height, works for both Orthographic & Perspective cameras
    /// Made for tutorial https://youtu.be/0cmxFjP375Y
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraConstantFit : MonoBehaviour
    {
        [SerializeField] private Vector2 defaultResolution = new Vector2(1920, 1080);
        [Range(0f, 1f)] [SerializeField] private float widthOrHeight = 0;
        [Header("Initial size for orthographic")]
        [SerializeField] private float initialFov = 60;
#if UNITY_EDITOR
        [SerializeField] private bool disable = false;
        [SerializeField] private bool init = false;
#endif

        private CinemachineVirtualCamera _componentCamera;

        private float _targetAspect;

        public void ChangeValues(
            Vector2? defaultResolution = null,
            float? widthOrHeight = null,
            float? initialFov = null
        )
        {
            if (!(defaultResolution is null))
                this.defaultResolution = (Vector2) defaultResolution;
            
            if (!(widthOrHeight is null))
                this.widthOrHeight = (float) widthOrHeight;
            
            if (!(initialFov is null))
                this.initialFov = (float) initialFov;

            Init();
        }

        private void Start()
        {
            _componentCamera = GetComponent<CinemachineVirtualCamera>();
            
            Init();
        }

        private void Init()
        {
            _targetAspect = defaultResolution.x / defaultResolution.y;
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
                _componentCamera.m_Lens.OrthographicSize = initialFov * currentAspect / _targetAspect;
            }
            else
            {
                _componentCamera.m_Lens.FieldOfView = initialFov * currentAspect / _targetAspect;
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