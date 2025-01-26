using UnityEngine;

namespace tdk.Utilities
{
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// Checks if the given layer number is contained in the LayerMask.
        /// </summary>
        public static bool Contains(this LayerMask mask, int layerNumber)
        {
            return mask == (mask | (1 << layerNumber));
        }
    }
}