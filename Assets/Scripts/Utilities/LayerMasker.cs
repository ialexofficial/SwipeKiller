using UnityEngine;

namespace Utilities
{
    public static class LayerMasker
    {
        public static bool CheckLayer(LayerMask mask, int layer) => 
            (mask.value & (1 << layer)) != 0;

        public static LayerMask MergeLayerMasks(LayerMask mask1, LayerMask mask2) =>
            new LayerMask
            {
                value = mask1.value | mask2.value
            };

        public static LayerMask MergeLayerMasks(LayerMask mask1, LayerMask mask2, LayerMask mask3) =>
            new LayerMask
            {
                value = mask1.value | mask2.value | mask3.value
            };
    }
}