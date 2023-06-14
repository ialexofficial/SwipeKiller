using System.Linq;
using UnityEditor;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    public class ColliderEditor : BaseEditor
    {
        [MenuItem("Tools/Collider/Disable Colliders")]
        public static void DisableColliders()
        {
            foreach (Collider collider in GetRecordedColliders("Disable colliders"))
            {
                collider.enabled = false;
            }
        }

        private static Collider[] GetRecordedColliders(string recordName)
        {
            Collider[] colliders = Selection.objects
                .SelectMany(
                    target => (target as GameObject)?.GetComponentsInChildren<Collider>()
                )
                .ToArray();
            
            Undo.RecordObjects(colliders, recordName);

            return colliders;
        }
    }
}