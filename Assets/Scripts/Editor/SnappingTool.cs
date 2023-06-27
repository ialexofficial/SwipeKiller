using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using Utilities.SnappingTool;

namespace Editor
{
    [EditorTool("Snapping Tool", typeof(SnappingToolItem))]
    public class SnappingTool : EditorTool
    {
        [SerializeField] private Texture2D toolIcon;

        private const float SnappingDistance = .5f;

        private Transform _itemTransform;
        private SnappingToolItem _selectedItem;
        private SnappingToolPoint[] _itemPoints;
        private SnappingToolPoint[] _otherPoints;

        public override GUIContent toolbarIcon => new GUIContent()
        {
            image = toolIcon,
            text = "Snapping Tool",
            tooltip = "Snaps items to each other by snap points"
        };

        private void OnEnable()
        {
            _selectedItem = (SnappingToolItem) target;
            _itemTransform = _selectedItem.transform;
            _itemPoints = _selectedItem.GetComponentsInChildren<SnappingToolPoint>();
            _otherPoints = FindObjectsOfType<SnappingToolPoint>()
                .Except(_itemPoints)
                .ToArray();
        }

        public override void OnToolGUI(EditorWindow window)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(_itemTransform.position, _itemTransform.rotation);

            if (!EditorGUI.EndChangeCheck())
                return;

            Undo.RecordObject(_itemTransform, "Snap item");
            MoveWithSnapping(newPosition);
        }

        private void MoveWithSnapping(Vector3 newPosition)
        {
            float closestDistance = float.PositiveInfinity;
            Vector3 direction = Vector3.zero;

            foreach (var itemPoint in _itemPoints)
            {
                foreach (var otherPoint in _otherPoints)
                {
                    Vector3 pointDelta = otherPoint.transform.position - itemPoint.transform.position;
                    
                    if (pointDelta.magnitude < closestDistance)
                    {
                        closestDistance = pointDelta.magnitude;
                        direction = pointDelta.normalized;
                    }
                }
            }

            if (
                (closestDistance * direction 
                    + newPosition - _itemTransform.position).magnitude <= SnappingDistance
            )
            {
                _itemTransform.position += closestDistance * direction;
            }
            else
            {
                _itemTransform.position = newPosition;
            }
        }

        [Shortcut("Activate Snapping Tool", KeyCode.I)]
        private static void ActivateTool()
        {
            if (
                Selection.gameObjects.Length != 1 ||
                !Selection.gameObjects[0].TryGetComponent<SnappingToolItem>(out var item)
            )
                return;

            ToolManager.SetActiveTool<SnappingTool>();
        }

        [MenuItem("Tools/Snapping Tool/Toggle Item Points")]
        private static void ToggleItemPoints()
        {
            if (
                Selection.gameObjects.Length != 1 ||
                !Selection.gameObjects[0].TryGetComponent<SnappingToolItem>(out var item)
            )
                return;

            foreach (var point in item.GetComponentsInChildren<SnappingToolPoint>(true))
            {
                point.gameObject.SetActive(point.gameObject.activeSelf);
            }
            
            ActivateTool();
        }

        [MenuItem("Tools/Snapping Tool/Disable All Points")]
        private static void DisableAllPoints()
        {
            foreach (var point in FindObjectsOfType<SnappingToolPoint>())
            {
                point.gameObject.SetActive(false);
            }
            
            ActivateTool();
        }
        
        [MenuItem("Tools/Snapping Tool/Enable All Points")]
        private static void EnableAllPoints()
        {
            foreach (var point in FindObjectsOfType<SnappingToolPoint>(true))
            {
                point.gameObject.SetActive(true);
            }
            
            ActivateTool();
        }
    }
}