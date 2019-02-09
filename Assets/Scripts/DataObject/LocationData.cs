using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationData 
{
    public Actor PlayerActor;

    public List<Actor> Actors = new List<Actor>();

    public List<Transform> SpawnPoints = new List<Transform>();

    public List<StructureProp> PresetStructures = new List<StructureProp>();
}
