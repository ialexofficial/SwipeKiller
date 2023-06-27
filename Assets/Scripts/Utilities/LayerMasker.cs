using System.Linq;
using UnityEngine;

namespace Utilities
{
    public static class LayerMasker
    {
        public static bool CheckLayer(LayerMask mask, int layer) => 
            (mask.value & (1 << layer)) != 0;

        public static LayerMask MergeLayerMasks(params LayerMask[] masks) =>
            new LayerMask
            {
                value = masks.Aggregate((mask1, mask2) => mask1.value | mask2.value)
            };
    }
}