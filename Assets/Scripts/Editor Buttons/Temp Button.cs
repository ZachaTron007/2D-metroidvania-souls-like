using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerState))]
public class TempButton : Editor
{
    public override void OnInspectorGUI() {
        PlayerState button = (PlayerState)target;


        DrawDefaultInspector();

        if (GUILayout.Button("Hit Collided")) {
            //button.HitSuccess();

        }


    }
}
