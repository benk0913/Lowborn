using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool IsMouseOnUI { private set; get; }

    public RaycastHit GroundMouseHit;
    public Ray MouseRay { private set; get; }
    public bool IsMouseOnGround { private set; get; }

    public RaycastHit StructureMouseHit;
    public Ray StructureMouseRay { private set; get; }
    public bool IsMouseOnStructure { private set; get; }

    public static Game Instance;

    public Session Session;

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


    public void Initialize(Session currentSession)
    {
        this.Session = currentSession;
        CurrentGameTool = GameTool.Play;

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
            if (structures[b].Data.Type == StructureData.StructureType.Wall
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

    public void SetIsMouseOnUI(bool state)
    {
        IsMouseOnUI = state;
    }


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

