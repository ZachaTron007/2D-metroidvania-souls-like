using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions {
    public static Dictionary<string, int> layers =
        new Dictionary<string, int>() { { "Default", 0 }, { "TransparentFX", 1 }, { "Ignore Raycast", 2 }, { "Player", 3 }, { "Water", 4 }, { "UI", 5 }, { "Level", 6 }, { "Enemys", 7 }, { "Enemy Attacks", 8 }, { "Player Attacks", 9 }, };

    public static float PointToDistance(Vector2 point1, Vector2 point2) {
        float distance = Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
        return distance;
    }

    public static LayerMask LayerMaskCreator(int[] layers) {
        LayerMask binaryLayers = 0;
        if (layers.Length > 0) {
            for (int i = 0; i < layers.Length; i++) {
                binaryLayers += 1 << layers[i];
            }
        }
        return binaryLayers;
    }

}
