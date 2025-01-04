using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class SwitchDirectionButton : Editor {
    public override void OnInspectorGUI() {
        Health button = (Health)target;


        DrawDefaultInspector();

        if (GUILayout.Button("Full Heal")) {
            button.Heal(1000);

        }


    }

}
