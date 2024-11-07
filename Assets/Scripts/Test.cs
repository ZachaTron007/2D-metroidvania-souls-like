using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerState))]
//[CustomEditor(typeof(Health))]
public class Test : Editor
{
    public override void OnInspectorGUI() {
        PlayerState button = (PlayerState)target;
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("SLide")) {
            button.StateChanges();

        }
        
    }
    
}
