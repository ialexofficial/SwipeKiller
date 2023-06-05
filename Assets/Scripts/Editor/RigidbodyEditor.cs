using System.Linq;
using UnityEditor;
using UnityEngine;
using BaseEditor = UnityEditor.Editor;

namespace Editor
{
    public class RigidbodyEditor : BaseEditor
    {
        [MenuItem("Tools/Rigidbody/Lock Z")]
        public static void LockZ()
        {
            foreach (Rigidbody rigidbody in GetRecordedRigidbodies("Locked rigidbody z positions"))
            {
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
            }
        }

        [MenuItem("Tools/Rigidbody/Unlock Z")]
        public static void UnlockZ()
        {
            foreach (Rigidbody rigidbody in GetRecordedRigidbodies("Unlocked rigidbody z positions"))
            {
                rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
        }

        [MenuItem("Tools/Rigidbody/Enable Kinematic")]
        public static void EnableKinematic()
        {
            foreach (Rigidbody rigidbody in GetRecordedRigidbodies("Enabled kinematic"))
            {
                rigidbody.isKinematic = true;
            }
        }
        
        [MenuItem("Tools/Rigidbody/Disable Kinematic")]
        public static void DisableKinematic()
        {
            foreach (Rigidbody rigidbody in GetRecordedRigidbodies("Enabled kinematic"))
            {
                rigidbody.isKinematic = false;
            }
        }

        [MenuItem("Tools/Rigidbody/Remove Ragdoll")]
        public static void RemoveRagdoll()
        {
            foreach (Rigidbody rigidbody in GetRecordedRigidbodies("Remove ragdoll"))
            {
                foreach(Joint joint in rigidbody.GetComponents<Joint>())
                    DestroyImmediate(joint);
                DestroyImmediate(rigidbody.GetComponent<Collider>());
                DestroyImmediate(rigidbody);
            }
        }

        private static Rigidbody[] GetRecordedRigidbodies(string recordName)
        {
            Rigidbody[] rigidbodies = Selection.objects
                .SelectMany(
                    target => (target as GameObject)?.GetComponentsInChildren<Rigidbody>()
                )
                .ToArray();
            
            Undo.RecordObjects(rigidbodies, recordName);

            return rigidbodies;
        }
    }
}