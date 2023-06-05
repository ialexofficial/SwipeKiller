using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    public class CustomEditorShortcuts
    {
        private static EditorWindow _activeProject;

        [MenuItem("Hot actions/Toggle Lock %SPACE")]
        static void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

        [MenuItem("Hot actions/Toggle Project Lock %#SPACE")]
        static void ToggleProjectLock()
        {
            if (_activeProject == null)
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.ProjectBrowser");
                Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
                _activeProject = (EditorWindow) findObjectsOfTypeAll[0];
            }

            if (_activeProject != null && _activeProject.GetType().Name == "ProjectBrowser")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.ProjectBrowser");
                PropertyInfo propertyInfo = type.GetProperty("isLocked", BindingFlags.Instance |
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.Public);

                bool value = (bool) propertyInfo.GetValue(_activeProject, null);

                propertyInfo.SetValue(_activeProject, !value, null);

                _activeProject.Repaint();
            }
        }

        [MenuItem("Hot actions/Add Inspector Tab %A")]
        public static void AddInspectorTab()
        {
            EditorWindow inspectorWindow = EditorWindow.GetWindow(
                typeof(UnityEditor.Editor)
                    .Assembly
                    .GetType("UnityEditor.InspectorWindow")
            );
            inspectorWindow = UnityEditor.Editor.Instantiate(inspectorWindow);
            inspectorWindow.Show();
            inspectorWindow.Focus();
        }

        [MenuItem("Hot actions/Close Focused Tab %Q")]
        public static void CloseFocusedTab()
        {
            EditorWindow.focusedWindow?.Close();
        }
    }
}