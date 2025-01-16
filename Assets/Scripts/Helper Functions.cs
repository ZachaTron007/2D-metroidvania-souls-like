using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static T getParentTransfromComponent<T>(Transform baseTransform) where T : class{
        T goalScript = null;
        while (goalScript == null) {
            baseTransform.TryGetComponent(out goalScript);
            if (!baseTransform.parent) {
                break;
            }
            baseTransform = baseTransform.parent;
        }
        return goalScript;
    }
    public static Transform getParentTransformlayer(Transform baseTransform,string layerName) {
        while (baseTransform.gameObject.layer != layers[layerName]) {
            if (!baseTransform.parent) {
                break;
            }
            baseTransform = baseTransform.parent;
        }
        if (baseTransform.gameObject.layer != layers[layerName]) return null;
        return baseTransform;
    }
    public static Transform getParentTransformlayer(Transform baseTransform, int layerNum) {
        while (baseTransform.gameObject.layer != layerNum) {
            if (!baseTransform.parent) {
                break;
            }
            baseTransform = baseTransform.parent;
        }
        if (baseTransform.gameObject.layer != layerNum) return null;
        return baseTransform;
    }
    public static Transform getParentTransformaTag(Transform baseTransform, string tagName) {
        while (baseTransform.gameObject.CompareTag(tagName)) {
            if (!baseTransform.parent) {
                break;
            }
            baseTransform = baseTransform.parent;
        }
        if (baseTransform.gameObject.CompareTag(tagName)) return null;
        return baseTransform;
    }
    /*
    public static GameObject EffectPlayer(GameObject playedEffect,Vector3 startPos, float destroyDelay,Quaternion rotation, float animSpeed = 1) {
        GameObject effect = Instantiate(playedEffect, startPos, rotation);
        Destroy(effect, destroyDelay);
        if(animSpeed!=1)    effect.GetComponent<Animator>().speed = animSpeed;

        return playedEffect;
    }*/

    public static float LerpHelper(float startValue, float endValue, float totalTime, float counter) {
        counter += Time.deltaTime;
        return Mathf.Lerp(startValue, endValue, 1-(counter/totalTime));

    }


}
