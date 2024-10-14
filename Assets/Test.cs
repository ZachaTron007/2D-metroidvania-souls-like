using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class Test : Editor
{/*
    public Health health;
    delegate void Button(int health);
    Button button;
    private void Start() {
        button = health.Damage;
    }*//*
    public override void OnInspectorGUI() {
        health = ()target;
        customButton<Health1>("Heal",button,50);
        /*DrawDefaultInspector();
        if (GUILayout.Button("Heal")) {
            health.Heal(50);

        }
    }

    private void customButton<T>(string buttonName,Button button,int param) where T : UnityEngine.Object {
            T script = (T)target;
            DrawDefaultInspector();
            if (GUILayout.Button(buttonName)) {
            button(param);

            }

    }*/
    
}
