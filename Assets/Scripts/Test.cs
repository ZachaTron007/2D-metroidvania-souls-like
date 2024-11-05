using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthBar))]
//[CustomEditor(typeof(Health))]
public class Test : Editor
{
    public Health health;
    delegate void Button(int health);
    Button button;
    private void Start() {
        button = health.Damage;
    }
    public override void OnInspectorGUI() {
        HealthBar healthbar = (HealthBar)target;
        
        DrawDefaultInspector();
        /*
        if (GUILayout.Button("Go to 90 Health")) {
            healthbar.changeHealthToNumber(90);

        }
        if (GUILayout.Button("Heal 10 Health")) {
            healthbar.changeHealthByNumber(10);

        }
        if (GUILayout.Button("Go to 10%")) {
            healthbar.changeHealthToPercent(10);

        }
        if (GUILayout.Button("Go to 50%")) {
            healthbar.changeHealthToPercent(50);

        }*/
    }
    
}
