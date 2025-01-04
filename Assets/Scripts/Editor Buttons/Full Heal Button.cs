using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwordEnemyScript))]
public class FullHealButton : Editor
{
    public override void OnInspectorGUI() {
        SwordEnemyScript enemyScript = (SwordEnemyScript)target;
        
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("Flip Directions")) {
            enemyScript.FlipDirection();

        }
        

    }
    
}
