using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BuildingTool : MonoBehaviour
{
    #region Properties

    [SerializeField]
    LayerMask buildMask;

    [SerializeField]
    GameObject marker;

    [SerializeField]
    float SnapUnit;

    [SerializeField]
    Material CanBuildMaterial;

    [SerializeField]
    Material CannotBuildMaterial;

    [SerializeField]
    GameObject UIPanel;

    bool isToolActive;

    #endregion

    #region Internal Parameters

    public static BuildingTool Instance;

    RaycastHit RayHit;
    Ray MouseRay;

    string BlueprintKey;
    GameObject BlueprintItem;

    bool isSelectingGrid;

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

    //This map is used to store already built structures in order to see if the current state is buildable.
    Dictionary<Vector2, List<StructureProp>> SMAP = new Dictionary<Vector2, List<StructureProp>>();

    Vector3 PreviousSelectedTile;

    #endregion

    #region Unity LifeCycle

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(!isToolActive)
        {
            return;
        }

        RefreshSelectionPoint();

        if (State == ToolState.Building)
        {
            RefreshBuildingMode();
        }
    }

    #endregion

    #region Private Methods

    void RefreshSelectionPoint()
    {

        MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(MouseRay.origin, MouseRay.direction, out RayHit, Mathf.Infinity, buildMask, QueryTriggerInteraction.UseGlobal))
        {
            isSelectingGrid = true;

            if (!marker.activeInHierarchy)
            {
                marker.gameObject.SetActive(true);
            }

            marker.transform.position =
                new Vector3(
                    RoundToNearestUnit(RayHit.point.x),
                    RoundToNearestUnit(RayHit.point.y + ((CurrentFloor*2) * SnapUnit)),
                    RoundToNearestUnit(RayHit.point.z));
        }
        else
        {
            isSelectingGrid = false;

            if (marker.activeInHierarchy)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }

    void RefreshBuildingMode()
    {
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

    float RoundToNearestUnit(float Value)
    {
        int integer = Mathf.FloorToInt(Value);

        return (float) Math.Round(integer / SnapUnit) * SnapUnit;
    }

    void ClearBlueprint()
    {
        Destroy(BlueprintItem);
        BlueprintItem = null;
        BlueprintKey = "";

        State = ToolState.Normal;
    }

    void PlaceBlueprintItem()
    {
        if(CORE.Instance.IsMouseOnUI)
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
    }

    void RefreshBlueprintState()
    {
        if(BlueprintItem == null)
        {
            return;
        }

        List<Material> iMaterials = new List<Material>();
        iMaterials.InsertRange(0, BlueprintItem.GetComponent<BlueprintObject>().MeshObject.GetComponent<MeshRenderer>().materials);
        
        while(iMaterials.Count > 1)
        {
            iMaterials.RemoveAt(1);
        }

        if (CanBuild(new Vector2(BlueprintItem.transform.position.x, BlueprintItem.transform.position.z), BlueprintItem.GetComponent<StructureProp>()))
        {
            iMaterials.Add(CanBuildMaterial);
            canBuild = true;
        }
        else
        {
            iMaterials.Add(CannotBuildMaterial);
            canBuild = false;
        }

        BlueprintItem.GetComponent<BlueprintObject>().MeshObject.GetComponent<MeshRenderer>().materials = iMaterials.ToArray();
    }

    bool canOccupySpot(Vector2 point, StructureProp structure)
    {
        if(!SMAP.ContainsKey(point) || SMAP[point].Count == 0)
        {
            return true;
        }

        for(int i=0;i<SMAP[point].Count;i++)
        {
            if(SMAP[point][i].Data.Type == structure.Data.Type 
                && SMAP[point][i].Floor == CurrentFloor)
            {
                return false;
            }
        }

        return true;
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
        UIPanel.SetActive(true);
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
        UIPanel.SetActive(false);
    }

    public bool AddOccupationToSMAP(Vector2 point, StructureProp structure)
    {
        if(!SMAP.ContainsKey(point))
        {
            SMAP.Add(point, new List<StructureProp>());
        }

        if(SMAP[point].Contains(structure))
        {
            return false;
        }

        SMAP[point].Add(structure);
        return true;
    }

    public void RemoveOccupationFromSMAP(Vector2 point, StructureProp structure)
    {
        SMAP[point].Remove(structure);
    }

    public List<StructureProp> GetStructuresInFloor(int floor)
    {
        List<StructureProp> props = new List<StructureProp>();

        for(int a=0;a<SMAP.Keys.Count;a++)
        {
            for(int b=0; b < SMAP[SMAP.Keys.ElementAt(a)].Count;b++)
            {
                StructureProp prop = SMAP[SMAP.Keys.ElementAt(a)][b];

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
        if(!canOccupySpot(new Vector2(smapPos.x, smapPos.y), prop))
        {
            return false;
        }

        for (int x = 1; x < prop.SizeX + 1; x++)
        {
            if (!canOccupySpot(new Vector2(smapPos.x + x, smapPos.y), prop))
            {
                return false;
            }
            if (!canOccupySpot(new Vector2(smapPos.x - x, smapPos.y), prop))
            {
                return false;
            }

            for (int z = 1; z < prop.SizeZ + 1; z++)
            {
                if (!canOccupySpot(new Vector2(smapPos.x + x, smapPos.y + z), prop))
                {
                    return false;
                }
                if (!canOccupySpot(new Vector2(smapPos.x - x, smapPos.y - z), prop))
                {
                    return false;
                }
            }
        }

        for (int z = 1; z < prop.SizeZ + 1; z++)
        {
            if (!canOccupySpot(new Vector2(smapPos.x, smapPos.y + z), prop))
            {
                return false;
            }
            if (!canOccupySpot(new Vector2(smapPos.x, smapPos.y - z), prop))
            {
                return false;
            }

            for (int x = 1; x < prop.SizeX+1; x++)
            {
                if (!canOccupySpot(new Vector2(smapPos.x + x, smapPos.y + z), prop))
                {
                    return false;
                }
                if (!canOccupySpot(new Vector2(smapPos.x - x, smapPos.y - z), prop))
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

    #endregion

    public enum ToolState
    {
        Normal,
        Building,
        Selecting,
        Removing
    }
}