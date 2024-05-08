/*
 * This class is used to check collisions with layermasks because Unity doesn't
 * have any way to easily check for collisions using layermasks. [Tegomlee]
 * 
 * Basically, Unity iterates through eack layer's binary notation and checks if a 1
 * is present in both that layer's binary and the binary of the layermask collection,
 * this is a fast process. (ex. 1 = 001 and 2 = 010) Using this information, we can
 * determine that 001 and 010 is 000 because a 1 is not present in both numbers at the
 * same place, where as 101 and 001 is 001, meaning that 001 != 000.
 */

using UnityEngine;

public static class LayerMaskExtensions
{
    /// <summary>
    /// Checks whether a layer is inside a layer mask by performing a bitwise AND operation.
    /// </summary>
    /// <param name="layerMask">This is the layer mask that we are checking.</param>
    /// <param name="layerToCheck">This is the layer we are checking against.</param>
    /// <returns>Whether the layer is inside the layer mask.</returns>
    public static bool IsLayerInMask(LayerMask layerMask, int layerToCheck)
    {
        return (layerMask & (1 << layerToCheck)) != 0;
    }
}
