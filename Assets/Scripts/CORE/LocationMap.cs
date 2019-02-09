using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMap : MonoBehaviour
{
    public bool IsMouseOnUI { private set; get; }

    public RaycastHit GroundMouseHit;
    public Ray MouseRay { private set; get; }
    public bool IsMouseOnGround { private set; get; }

    public RaycastHit StructureMouseHit;
    public Ray StructureMouseRay { private set; get; }
    public bool IsMouseOnStructure { private set; get; }

    public static LocationMap Instance;

    [SerializeField]
    public LocationData Data;


    public GameTool CurrentGameTool
    {
        set
        {
            gameTool = value;
            switch(gameTool)
            {
                case GameTool.Build:
                    {
                        BuildingTool.Instance.ActivateTool();

                        break;
                    }
                case GameTool.Play:
                    {
                        BuildingTool.Instance.DeactivateTool();

                        break;
                    }
            }
        }
        get
        {
            return gameTool;
        }
    }
    GameTool gameTool;

    public ViewMode CurrentViewMode
    {
        get
        {
            return currentViewMode;
        }
        set
        {
            currentViewMode = value;
            ViewFloor = viewFloor;
        }
    }
    ViewMode currentViewMode;

    public int ViewFloor
    {
        get
        {
            return viewFloor;
        }
        set
        {
            if(value < -1)
            {
                value = -1;
            }
            else if(value > MaxFloor)
            {
                value = MaxFloor;
            }

            viewFloor = value;

            if (value <= -1)
            {
                BuildingTool.Instance.CurrentFloor = 0;
            }
            else
            {
                BuildingTool.Instance.CurrentFloor = viewFloor;
            }

            GamePanelUI.Instance.SetFloorText(viewFloor.ToString());

            RefreshVisibleFloors(viewFloor);
        }
    }
    int viewFloor;


    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    LayerMask structureMask;

    [SerializeField]
    int MaxFloor;

    [SerializeField]
    public float SnapUnit;



    private void Awake()
    {
        Instance = this;
    }


    public void Initialize(LocationData data = null)
    {
        CurrentGameTool = GameTool.Play;

        Data.PlayerActor = SpawnActor(CORE.Instance.PlayerCharacter);
        Data.PlayerActor.transform.position = Data.SpawnPoints[0].position;

        foreach (StructureProp prop in Data.PresetStructures)
        {
            prop.OccupySMAP();
        }

        StartCoroutine(RefreshGameRoutine());
    }

    IEnumerator RefreshGameRoutine()
    {
        while(true)
        {
            RefreshMousePosition();

            yield return new WaitForEndOfFrame();

            RefreshVisibleWalls();
        }
    }


    private void RefreshMousePosition()
    {
        MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(MouseRay.origin, MouseRay.direction, out GroundMouseHit, Mathf.Infinity, groundMask, QueryTriggerInteraction.UseGlobal))
        {
            IsMouseOnGround = true;
        }
        else
        {
            IsMouseOnGround = false;
        }


        if (Physics.Raycast(MouseRay.origin, MouseRay.direction, out StructureMouseHit, Mathf.Infinity, structureMask, QueryTriggerInteraction.UseGlobal))
        {
            IsMouseOnStructure = true;
        }
        else
        {
            IsMouseOnStructure = false;
        }
    }

    private void RefreshVisibleFloors(int floor)
    {
        switch(CurrentViewMode)
        {
            case ViewMode.Outside:
                {
                    List<StructureProp> structures;
                    for (int i = 0; i < MaxFloor; i++)
                    {
                        structures = BuildingTool.Instance.GetStructuresInFloor(i);

                        for (int b = 0; b < structures.Count; b++)
                        {
                            structures[b].ShowMesh();
                        }
                    }

                    return;
                }
            default :
                {

                    List<StructureProp> structures;
                    for (int i = 0; i < MaxFloor; i++)
                    {
                        if (i > floor)
                        {
                            structures = BuildingTool.Instance.GetStructuresInFloor(i);

                            for (int b = 0; b < structures.Count; b++)
                            {
                                structures[b].HideMesh();
                            }
                        }
                        else
                        {
                            structures = BuildingTool.Instance.GetStructuresInFloor(i);

                            for (int b = 0; b < structures.Count; b++)
                            {
                                structures[b].ShowMesh();
                            }
                        }
                    }
                    return;
                }

        }
    }

    private void RefreshVisibleWalls()
    {
        if (CurrentViewMode != ViewMode.HideWalls)
        {
            return;
        }

        List<StructureProp> structures = BuildingTool.Instance.GetStructuresInFloor(ViewFloor);

        for (int b = 0; b < structures.Count; b++)
        {
            if (structures[b].Data.Type == PropType.Wall
                &&
                (structures[b].transform.position.z   + (SnapUnit * viewFloor) + SnapUnit < GroundMouseHit.point.z
                || structures[b].transform.position.x + (SnapUnit * viewFloor) + SnapUnit < GroundMouseHit.point.x)) // Closer than mouse position and in hide walls mode.
            {
                structures[b].HideMesh();
                continue;
            }

            structures[b].ShowMesh();
        }
    }

    public float GetNearestSnapUnit(float Value)
    {
        int integer = Mathf.RoundToInt(Value);

        return (float)Math.Round(integer / SnapUnit) * SnapUnit;
    }

    public Vector3 GetNearestSnapPosition(Vector3 pos, bool includeY = false)
    {
        return new Vector3(
                    GetNearestSnapUnit(pos.x),
                    includeY? GetNearestSnapUnit(pos.y) : pos.y,
                    GetNearestSnapUnit(pos.z));
    }

    public void SetIsMouseOnUI(bool state)
    {
        IsMouseOnUI = state;
    }

    public Actor SpawnActor(Character character)
    {
        Actor temp = Instantiate(ResourcesLoader.Instance.GetObject("Actor")).GetComponent<Actor>();
        temp.SetCharacter(character);

        return temp;
    }

    #region SMAP


    //This map is used to store already built structures in order to see if the current state is buildable.
    public Dictionary<Vector2, List<StructureProp>> SMAP = new Dictionary<Vector2, List<StructureProp>>();

    public bool AddOccupationToSMAP(Vector2 point, StructureProp structure)
    {
        if (!SMAP.ContainsKey(point))
        {
            SMAP.Add(point, new List<StructureProp>());
        }

        if (SMAP[point].Contains(structure))
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


    public bool isSpotOccupiable(Vector2 point, StructureProp structure, int inFloor)
    {
        if (!SMAP.ContainsKey(point) || SMAP[point].Count == 0)
        {
            return true;
        }

        for (int i = 0; i < SMAP[point].Count; i++)
        {
            if (SMAP[point][i].Data.Type == structure.Data.Type
                && SMAP[point][i].Floor == inFloor)
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}

public enum ViewMode
{
    Outside,
    Walls,
    HideWalls
}

public enum GameTool
{
    Build,
    Play
}

