using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocationMap))]
public class LocationMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LocationMap myScript = (LocationMap)target;
        if (GUILayout.Button("Save Location Data"))
        {
            myScript.Data.Actors.Clear();

            GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Actor");

            foreach(GameObject actor in tempObjects)
            {
                myScript.Data.Actors.Add(actor.GetComponent<Actor>());
            }


            myScript.Data.SpawnPoints.Clear();

            tempObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

            foreach (GameObject sPoint in tempObjects)
            {
                myScript.Data.SpawnPoints.Add(sPoint.transform);
            }

            EditorUtility.SetDirty(myScript);



            myScript.SMAP.Clear();
            myScript.Data.PresetStructures.Clear();

            tempObjects = GameObject.FindGameObjectsWithTag("Structure");

            foreach (GameObject mapStructure in tempObjects)
            {
                mapStructure.transform.position = myScript.GetNearestSnapPosition(mapStructure.transform.position);

                myScript.Data.PresetStructures.Add(mapStructure.GetComponent<StructureProp>());
            }

            

            EditorUtility.SetDirty(myScript);

        }
    }
}
