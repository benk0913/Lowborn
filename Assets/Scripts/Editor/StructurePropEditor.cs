using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructureProp))]
public class StructurePropEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StructureProp myScript = (StructureProp)target;
        if (GUILayout.Button("Reload Meshes"))
        {
            myScript.LoadMeshes();
            EditorUtility.SetDirty(myScript);
        }
    }
}
