using UnityEngine;

namespace Utilities
{
    public static class LayerMasker
    {
        public static bool CheckLayer(LayerMask mask, int layer) => 
            (mask.value & (1 << layer)) != 0;
    }
}