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

    private void OnEnable()
    {
        AddListeners();
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
        CurrentSeconds--;

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
        Vector3 raySource = Surface.transform.position + new Vector3(0f, 10f, 0f);
        RaycastHit rhit;
        Collider foundCollider = null;
        Vector3 bottomLeft = Surface.transform.position - new Vector3(Surface.size.x / 2f, 0f, Surface.size.y / 2f);
        Vector3 topRight = Surface.transform.position   + new Vector3(Surface.size.x / 2f, 0f, Surface.size.y / 2f);
        Vector3 randomPoint;
        Vector3 rayPoint = Vector3.zero;
        StructureProp randomProp = PropsCollection[Random.Range(0, PropsCollection.Count)].Prefab.GetComponent<StructureProp>();


        while (foundCollider == null || foundCollider.tag != "BuildableSurface" || rayPoint == Vector3.zero || !LocationMap.Instance.isSpotOccupiable(rayPoint, randomProp, 0))
        {
            randomPoint = new Vector3(Random.Range(bottomLeft.x, topRight.x), 0f, Random.Range(bottomLeft.z, topRight.z));

            Physics.Raycast(raySource, (randomPoint - raySource), out rhit, Mathf.Infinity);

            foundCollider = rhit.collider;
            rayPoint = rhit.point;

            yield return 0;
        }

        GameObject tempProp = Instantiate(randomProp.gameObject);
        tempProp.transform.position = rayPoint;
        tempProp.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        tempProp.GetComponent<StructureProp>().PlaceStructureProp(0, null);

        CurrentSeconds = Random.Range(MinSeconds, MaxSeconds);
        AttemptSpawnRoutine = null;
    }
}