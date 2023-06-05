using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Settings/SwipeSettings")]
    public class SwipeSettings : ScriptableObject
    {
        public float Strength = 5f;
        public float MaxVelocity = 5f;
        public float MinTimeInteraction = 0.001f;
        public float MaxTimeInteraction = 1f;
        [Range(0.1f, 0, order = -1)] public float DeadZone = 0.1f;
    }
}