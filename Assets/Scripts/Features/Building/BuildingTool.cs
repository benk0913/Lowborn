using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;

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



    bool isToolActive;

    #endregion

    #region Internal Parameters

    public static BuildingTool Instance;

    string BlueprintKey;
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
        BlueprintKey = "";

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

        GameObject obj = Instantiate(ResourcesLoader.Instance.GetObject(BlueprintKey));

        obj.transform.position = BlueprintItem.transform.position;
        obj.transform.rotation = BlueprintItem.transform.rotation;

        GameObject blueprintMesh = BlueprintItem.GetComponent<BlueprintObject>().MeshObject;

        ResourcesLoader.Instance.GetRecycledObject("MeshParticles").GetComponent<MeshParticles>()
            .SetTarget(blueprintMesh.GetComponent<MeshFilter>());

        StructureProp prop = obj.GetComponent<StructureProp>();
        prop.Floor = CurrentFloor;
        prop.OccupySMAP();

        RefreshBlueprintState();

        //LocationMap.Instance.GetComponent<NavMeshSurface>().RemoveData();
        LocationMap.Instance.GetComponent<NavMeshSurface>().BuildNavMesh();
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

        for (int x = 1; x < prop.SizeX + 1; x++)
        {
            if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x + x, smapPos.y), prop, CurrentFloor))
            {
                return false;
            }
            if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x - x, smapPos.y), prop, CurrentFloor))
            {
                return false;
            }

            for (int z = 1; z < prop.SizeZ + 1; z++)
            {
                if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x + x, smapPos.y + z), prop, CurrentFloor))
                {
                    return false;
                }
                if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x - x, smapPos.y - z), prop, CurrentFloor))
                {
                    return false;
                }
            }
        }

        for (int z = 1; z < prop.SizeZ + 1; z++)
        {
            if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x, smapPos.y + z), prop, CurrentFloor))
            {
                return false;
            }
            if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x, smapPos.y - z), prop, CurrentFloor))
            {
                return false;
            }

            for (int x = 1; x < prop.SizeX+1; x++)
            {
                if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x + x, smapPos.y + z), prop, CurrentFloor))
                {
                    return false;
                }
                if (!LocationMap.Instance.isSpotOccupiable(new Vector2(smapPos.x - x, smapPos.y - z), prop, CurrentFloor))
                {
                    return false;
                }
            }
        }

        return true;
    }


    public void SetBlueprintItem(string itemKey)
    {
        if (State == ToolState.Building)
        {
            ClearBlueprint();
        }

        State = ToolState.Building;

        BlueprintKey = itemKey;
        BlueprintItem = Instantiate(ResourcesLoader.Instance.GetObject(itemKey));

        RefreshBlueprintState();
    }

    public void RemoveSelectedStructure(StructureProp structure)
    {
        if (structure.Data.IsBuildable)
        {
            structure.RemoveOccupation();
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