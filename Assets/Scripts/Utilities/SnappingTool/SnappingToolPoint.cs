#if UNITY_EDITOR
using UnityEngine;

namespace Utilities.SnappingTool
{
    public class SnappingToolPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, .1f);
        }
    }
}

#endif