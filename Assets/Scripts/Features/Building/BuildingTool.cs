using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Events;

public class BuildingTool : MonoBehaviour
{
    #region Properties

    [SerializeField]
    GameObject marker;

    [SerializeField]
    Material CanBuildMaterial;

    [SerializeField]
    string CanBuildSurfaceTag;

    [SerializeField]
    Material CannotBuildMaterial;

    [SerializeField]
    string CannotBuildSurfaceTag;


    [SerializeField]
    Material SelectedMaterial;

    [SerializeField]
    public UnityEvent OnActivate;

    [SerializeField]
    public UnityEvent OnDeactivate;



    public bool isToolActive;

    #endregion

    #region Internal Parameters

    public static BuildingTool Instance;

    Prop BlueprintProp;
    GameObject BlueprintItem;

    StructureProp SelectedItem;

    public ToolState State { private set; get; }

    public int CurrentFloor
    {
        get
        {
            return currentFloor;
        }
        set
        {
            currentFloor = value;
            RefreshBlueprintState();
        }
    }
    int currentFloor;

    bool canBuild;

    Vector3 PreviousSelectedTile;

    #endregion

    
    #region Unity LifeCycle

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (!isToolActive)
        {
            return;
        }

        switch(State)
        {
            case ToolState.Building:
                {
                    RefreshBuildingMode();
                    break;
                }
            case ToolState.Normal:
                {
                    RefreshNormalMode();
                    break;
                }
            case ToolState.Selecting:
                {
                    RefreshSelectedMode();
                    break;
                }
        }
    }

    #endregion

    #region Private Methods

    void RefreshBuildingMarker()
    {
        if (LocationMap.Instance.IsMouseOnGround)
        {
            if (!marker.activeInHierarchy)
            {
                marker.gameObject.SetActive(true);
            }

            marker.transform.position = LocationMap.Instance.GetNearestSnapPosition(
                new Vector3(
                    LocationMap.Instance.GroundMouseHit.point.x,
                    LocationMap.Instance.GroundMouseHit.point.y + ((CurrentFloor * 2) * LocationMap.Instance.SnapUnit),
                    LocationMap.Instance.GroundMouseHit.point.z));
               
        }
        else
        {
            if (marker.activeInHierarchy)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }

    void RefreshBuildingMode()
    {
        RefreshBuildingMarker();

        if (BlueprintItem == null)
        {
            return;
        }


        BlueprintItem.transform.position = marker.transform.position;

        if (marker.transform.position != PreviousSelectedTile)
        {
            RefreshBlueprintState();
        }

        PreviousSelectedTile = marker.transform.position;



        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild)
            {
                PlaceBlueprintItem();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearBlueprint();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            BlueprintItem.transform.rotation =
                Quaternion.Euler(
                    BlueprintItem.transform.rotation.eulerAngles.x,
                    BlueprintItem.transform.rotation.eulerAngles.y - 90f,
                    BlueprintItem.transform.rotation.eulerAngles.z);

            RefreshBlueprintState();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            BlueprintItem.transform.rotation =
                Quaternion.Euler(
                    BlueprintItem.transform.rotation.eulerAngles.x,
                    BlueprintItem.transform.rotation.eulerAngles.y + 90f,
                    BlueprintItem.transform.rotation.eulerAngles.z);

            RefreshBlueprintState();
        }
    }

    void RefreshNormalMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LocationMap.Instance.IsMouseOnStructure)
            {
                SelectProp(LocationMap.Instance.StructureMouseHit.collider.GetComponent<StructureProp>());
            }
        }
    }

    void RefreshSelectedMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (LocationMap.Instance.IsMouseOnStructure)
            {
                SelectProp(LocationMap.Instance.StructureMouseHit.collider.GetComponent<StructureProp>());
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
        }

        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveSelectedStructure(SelectedItem);
        }
    }

    void SelectProp(StructureProp prop)
    {
        StructureProp propToSelect = LocationMap.Instance.StructureMouseHit.collider.GetComponent<StructureProp>();

        if (propToSelect == null)
        {
            return;
        }

        ClearSelection();
        SelectedItem = propToSelect;

        State = ToolState.Selecting;

        SelectedItem.SetMaterial(SelectedMaterial);
    }

    void ClearBlueprint()
    {
        Destroy(BlueprintItem);
        BlueprintItem = null;
        BlueprintProp = null;

        State = ToolState.Normal;
    }

    void ClearSelection()
    {
        if(SelectedItem == null)
        {
            return;
        }

        SelectedItem.SetOriginalMaterials();
        SelectedItem = null;
        State = ToolState.Normal;
    }

    void PlaceBlueprintItem()
    {
        if(LocationMap.Instance.IsMouseOnUI)
        {
            return;
        }

        GameObject obj = Instantiate(BlueprintProp.Prefab);

        obj.transform.position = BlueprintItem.transform.position;
        obj.transform.rotation = BlueprintItem.transform.rotation;

        StructureProp blueprintMesh = BlueprintItem.GetComponent<StructureProp>();

        //A small cheat to initialize the collider in timescale 0
        obj.GetComponent<Collider>().enabled = false;
        obj.GetComponent<Collider>().enabled = true;

        foreach (MeshRenderer renderer in blueprintMesh.MeshRenderers)
        {
            ResourcesLoader.Instance.GetRecycledObject("MeshParticles").GetComponent<MeshParticles>().SetTarget(renderer.GetComponent<MeshFilter>());
        }

        StructureProp prop = obj.GetComponent<StructureProp>();
        prop.PlaceStructureProp(CurrentFloor, CORE.Instance.CurrentScenario.PlayerDynasty);

        RefreshBlueprintState();

    }

    void RefreshBlueprintState()
    {
        if(BlueprintItem == null)
        {
            return;
        }

        StructureProp structure = BlueprintItem.GetComponent<StructureProp>();

        if (   LocationMap.Instance.GroundMouseHit.collider.tag != CannotBuildSurfaceTag
            && CanBuild(new Vector2(BlueprintItem.transform.position.x, BlueprintItem.transform.position.z), structure))
        {
            structure.SetMaterial(CanBuildMaterial);
            canBuild = true;
        }
        else
        {
            structure.SetMaterial(CannotBuildMaterial);
            canBuild = false;
        }
    }
    
    #endregion

    #region Public Methods

    public void ActivateTool()
    {
        if(isToolActive)
        {
            return;
        }

        isToolActive = true;

        OnActivate.Invoke();
    }

    public void DeactivateTool()
    {
        if (!isToolActive)
        {
            return;
        }

        isToolActive = false;
        ClearBlueprint();
        marker.gameObject.SetActive(false);

        OnDeactivate.Invoke();
    }

    public List<StructureProp> GetStructuresInFloor(int floor)
    {
        List<StructureProp> props = new List<StructureProp>();

        for(int a=0;a< LocationMap.Instance.SMAP.Keys.Count;a++)
        {
            for(int b=0; b < LocationMap.Instance.SMAP[LocationMap.Instance.SMAP.Keys.ElementAt(a)].Count;b++)
            {
                StructureProp prop = LocationMap.Instance.SMAP[LocationMap.Instance.SMAP.Keys.ElementAt(a)][b];

                if (prop.Floor == floor)
                {
                    props.Add(prop);
                }
            }
        }

        return props;
    }

    public bool CanBuild(Vector2 smapPos, StructureProp prop)
    {
        if(!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x, smapPos.y), prop, CurrentFloor))
        {
            return false;
        }

        Vector2Int startingSpot = new Vector2Int(
        Mathf.FloorToInt(smapPos.x - prop.Data.SizeX),
        Mathf.FloorToInt(smapPos.y - prop.Data.SizeZ));

        Vector2Int endingSpot = new Vector2Int(
            startingSpot.x + 1 + prop.Data.SizeX * 2,
            startingSpot.y + 1 + prop.Data.SizeZ * 2);


        for (int x = startingSpot.x; x < endingSpot.x; x++)
        {
            for (int y = startingSpot.y; y < endingSpot.y; y++)
            {
                if (!LocationMap.Instance.isSpotOccupiable(new Vector2(x,y), prop, CurrentFloor))
                {
                    return false;
                }
            }
        }
        
        return true;
    }


    public void SetBlueprintItem(Prop propData)
    {
        if (State == ToolState.Building)
        {
            ClearBlueprint();
        }

        State = ToolState.Building;

        BlueprintProp = propData;
        BlueprintItem = Instantiate(propData.Prefab);

        RefreshBlueprintState();
    }

    public void RemoveSelectedStructure(StructureProp structure)
    {
        if (structure.Data.IsBuildable)
        {
            structure.RemoveStructureProp();
            Destroy(structure.gameObject);
        }
        else
        {
            //TODO CAN'T REMOVE THIS! (EFFECt)
        }

        ClearSelection();
    }

    #endregion

    public enum ToolState
    {
        Normal,
        Building,
        Selecting,
        Removing
    }
}