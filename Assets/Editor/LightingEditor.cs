using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    public class LightingEditor : BaseEditor
    {
        [MenuItem("Tools/Lighting/Disable DO")]
        public static void DisableDO() =>
            ToggleDO(SelectComponents<MeshRenderer>(), false);

        private static IEnumerable<T> SelectComponents<T>()
        {
            List<T> components = new List<T>();
            
            foreach (GameObject target in Selection.gameObjects)
            {
                foreach(T component in target.GetComponentsInChildren<T>())
                    components.Add(component);

                T parentComponent;

                target.TryGetComponent<T>(out parentComponent);

                if (parentComponent != null)
                    components.Add(parentComponent);
            }

            return components;
        }

        private static void ToggleDO(IEnumerable<MeshRenderer> meshRenderers, bool enabled)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.allowOcclusionWhenDynamic = enabled;
            }
        }
    }
}