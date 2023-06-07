using Cinemachine;
using Level;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelDataPreviewer : UnityEditor.Editor
    {
        [SerializeField] private Vector2Int defaultResolution = new Vector2Int(1920, 1080);
        [SerializeField] private float widthOrHeight = .4f;
        [SerializeField] private float initialFoV = 90f;
        
        private CinemachineVirtualCamera _vcam;
        private CinemachineFramingTransposer _vcamTransposer;
        private Camera _camera;
        private GameObject _weapon;
        private LevelConfig _levelConfig;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (_levelConfig is null)
                return;

            if (GUILayout.Button("Preview"))
                Preview();

            if (_vcam is null)
                return;

            _weapon.transform.position = _levelConfig.WeaponSpawnPoint;
            _vcam.m_Lens.FieldOfView = _levelConfig.FieldOfView;
            _vcam.ForceCameraPosition(_levelConfig.CameraPosition, _vcam.transform.rotation);
        }

        private void Preview()
        {
            _camera = new GameObject(
                    "previewingCamera",
                    typeof(Camera),
                    typeof(CinemachineBrain)
                )
                .GetComponent<Camera>();
            _camera.tag = "MainCamera";
            
            _vcam = new GameObject(
                    "previewingVcam", 
                    typeof(CinemachineVirtualCamera),
                    typeof(CameraConstantFit)
                )
                .GetComponent<CinemachineVirtualCamera>();
            _vcam.GetComponent<CameraConstantFit>().ChangeValues(
                defaultResolution: defaultResolution,
                widthOrHeight: widthOrHeight,
                initialFov: initialFoV
            );

            _weapon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _vcamTransposer = _vcam.AddCinemachineComponent<CinemachineFramingTransposer>();
            _vcamTransposer.m_UnlimitedSoftZone = true;
            _vcamTransposer.m_DeadZoneWidth = 2f;
            _vcamTransposer.m_DeadZoneHeight = 1.5f;
        }

        private void OnEnable()
        {
            _levelConfig = target as LevelConfig;
        }

        private void OnDisable()
        {
            _levelConfig = null;
            DestroyImmediate(_weapon.gameObject);
            DestroyImmediate(_vcam.gameObject);
            DestroyImmediate(_camera.gameObject);
        }
    }
}