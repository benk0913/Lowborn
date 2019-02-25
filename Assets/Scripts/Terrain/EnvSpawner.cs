using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvSpawner : MonoBehaviour
{
    [SerializeField]
    BoxCollider Surface;

    [SerializeField]
    int MinSeconds = 1;

    [SerializeField]
    int MaxSeconds = 3;

    [SerializeField]
    LayerMask RayMask;

    [SerializeField]
    List<Prop> PropsCollection = new List<Prop>();

    int CurrentSeconds = 3;

    
    float SizeX;
    float SizeY;
    float SizeZ;
    
    Vector3 bottomLeft;
    Vector3 topRight;

    private void OnEnable()
    {
        AddListeners();

        SizeX = Surface.size.x + Surface.transform.localScale.x;
        SizeY = Surface.size.y + Surface.transform.localScale.y;
        SizeZ = Surface.size.z + Surface.transform.localScale.z;
        bottomLeft = Surface.transform.position - new Vector3(SizeX / 2f, SizeY / 2f, SizeZ / 2f);
        topRight = Surface.transform.position + new Vector3(SizeX / 2f, SizeY / 2f, SizeZ / 2f);
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    void AddListeners()
    {
        PlayTool.Instance.OnSecondPassedEvent.AddListener(OnSecondPassed);
    }

    void RemoveListeners()
    {
        PlayTool.Instance.OnSecondPassedEvent.RemoveListener(OnSecondPassed);
    }
    
    private void OnSecondPassed()
    {
        if(AttemptSpawnRoutine != null)
        {
            return;
        }

        CurrentSeconds-=PlayTool.Instance.SecondsPerRealSecond;

        if(CurrentSeconds <= 0)
        {
            SpawnRandom();
        }
    }

    private void SpawnRandom()
    {
        if(AttemptSpawnRoutine != null)
        {
            return;
        }

        AttemptSpawnRoutine = StartCoroutine(AttemptSpawn());
    }

    Coroutine AttemptSpawnRoutine;
    IEnumerator AttemptSpawn()
    {
        Vector3 randomPos;
        StructureProp randomProp = PropsCollection[Random.Range(0, PropsCollection.Count)].Prefab.GetComponent<StructureProp>();

        while (true)
        {
            randomPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), topRight.y, Random.Range(bottomLeft.z, topRight.z));
            randomPos = LocationMap.Instance.GetNearestSnapPosition(randomPos, true);

            if(LocationMap.Instance.isSpotOccupiable(randomPos, randomProp, 0, false))
            {
                GameObject tempObj = Instantiate(randomProp.gameObject);
                tempObj.transform.position = randomPos;
                tempObj.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                tempObj.GetComponent<StructureProp>().PlaceStructureProp(0, null);
                break;
            }

            yield return 0;
        }

        CurrentSeconds = Random.Range(MinSeconds, MaxSeconds);
        AttemptSpawnRoutine = null;
    }
}