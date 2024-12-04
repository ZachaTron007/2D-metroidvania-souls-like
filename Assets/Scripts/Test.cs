using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(PlayerState))]
[CustomEditor(typeof(Health))]
public class Test : Editor
{
    public override void OnInspectorGUI() {
        Health button = (Health)target;
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("Full Heal")) {
            button.Heal(1000);

        }
        
    }
    
}
