using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemController))]
public class ItemControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemController script = (ItemController)target;
        if (GUILayout.Button("Setup Item based on grid_dimensions_"))
        {
            script.SetupItem();
        }
    }
}