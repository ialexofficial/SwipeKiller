using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GUI
{
    public class SettingsGUI : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject settingMenu;
        [SerializeField] private GameObject openSettingsButton;
        [SerializeField] private GameObject volumeDisableImage;
        [SerializeField] private GameObject vibrationDisableImage;

        private const string VolumePrefsKey = "volume";
        private const string VibrationPrefsKey = "vibration";
        
        private bool _isSettingsOpened = false;
        private bool _isVolumeEnabled = true;
        private bool _isVibrationEnabled = true;
        private EventSystem _eventSystem;
        private GraphicRaycaster _graphicRaycaster;

        public void OpenSettings()
        {
            settingMenu.SetActive(true);
            GameManager.Pause();
            _isSettingsOpened = true;
        }

        public void CloseSettings()
        {
            _isSettingsOpened = false;
            settingMenu.SetActive(false);
            GameManager.Confirm();
        }

        public void ToggleVolume()
        {
            _isVolumeEnabled = !_isVolumeEnabled;
            PlayerPrefs.SetInt(VolumePrefsKey, _isVolumeEnabled ? 1 : 0);
            volumeDisableImage.SetActive(!_isVolumeEnabled);
        }

        public void ToggleVibration()
        {
            _isVibrationEnabled = !_isVibrationEnabled;
            PlayerPrefs.SetInt(VibrationPrefsKey, _isVibrationEnabled ? 1 : 0);
            vibrationDisableImage.SetActive(!_isVibrationEnabled);
        }

        private void Start()
        {
            _eventSystem = EventSystem.current;
            _graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            
            ReadPrefs();
        }

        private void ReadPrefs()
        {
            _isVolumeEnabled = PlayerPrefs.GetInt(VolumePrefsKey, 1) == 1;
            _isVibrationEnabled = PlayerPrefs.GetInt(VibrationPrefsKey, 1) == 1;
        }
        
        private void Update()
        {
            if (!_isSettingsOpened)
                return;
            
            #if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                PointerEventData pointerEventData = new PointerEventData(_eventSystem)
                {
                    position = Input.mousePosition
                };
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                
                _graphicRaycaster.Raycast(pointerEventData, raycastResults);

                if (raycastResults.All(result =>
                    result.gameObject != settingMenu &&
                    result.gameObject != openSettingsButton
                ))
                    CloseSettings();
            }
            #endif

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended)
                    continue;

                PointerEventData pointerEventData = new PointerEventData(_eventSystem)
                {
                    position = touch.position
                };
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                
                _graphicRaycaster.Raycast(pointerEventData, raycastResults);

                if (raycastResults.All(result =>
                    result.gameObject != settingMenu &&
                    result.gameObject != openSettingsButton
                ))
                {
                    CloseSettings();
                    break;
                }
            }
        }
    }
}