using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ParametricEffect),true)]
public class ParametricEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Activate"))
            (target as ParametricEffect).Activate();
    }
}
