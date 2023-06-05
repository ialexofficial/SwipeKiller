using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Settings/SlowMotionSettings")]
    public class SlowMotionSettings : ScriptableObject
    {
        public float StartSpeed = 1f;
        public float ScaleFactor = 0.1f;
        public float Duration = 1f;
        public AnimationCurve Timeline;
    }
}