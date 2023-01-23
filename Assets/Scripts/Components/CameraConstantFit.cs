using UnityEngine;

namespace Components
{
    /// <summary>
    /// Keeps constant camera width instead of height, works for both Orthographic & Perspective cameras
    /// Made for tutorial https://youtu.be/0cmxFjP375Y
    /// </summary>
    [ExecuteAlways]
    public class CameraConstantFit : MonoBehaviour
    {
        [SerializeField] private Vector2 defaultResolution = new Vector2(720, 1280);
        [Range(0f, 1f)] 
        [SerializeField] private float widthOrHeight = 0;
        [SerializeField] private float initialFov = 60;

        private Camera _componentCamera;
    
        private float _initialSize;
        private float _targetAspect;
        
        private float _horizontalFov = 120f;

        private void Start()
        {
            _componentCamera = GetComponent<Camera>();
            _initialSize = _componentCamera.orthographicSize;

            _targetAspect = defaultResolution.x / defaultResolution.y;

            _horizontalFov = CalcVerticalFov(initialFov, 1 / _targetAspect);
        }

        private void Update()
        {
            if (_componentCamera.orthographic)
            {
                float constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
                _componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, widthOrHeight);
            }
            else
            {
                float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.aspect);
                _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, widthOrHeight);
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