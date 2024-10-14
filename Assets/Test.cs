using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class Test : Editor
{
    /*
    public override void OnInspectorGUI() {
        Health script = (Health)target;
        //customButton<Health1>("Heal");
    }

    private void customButton<T>(string buttonName) where T : UnityEngine.Object {
            T script = (T)target;
            DrawDefaultInspector();
            if (GUILayout.Button(buttonName)) {
            //.Heal;

            }

    }
    */
}
