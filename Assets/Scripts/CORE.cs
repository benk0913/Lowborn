using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CORE : MonoBehaviour
{
    public bool IsMouseOnUI { private set; get; }

    public static CORE Instance;

    [SerializeField]
    int MaxFloor;

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

    private void Awake()
    {
        Instance = this;
    }

    private void RefreshVisibleFloors(int floor)
    {
        for (int i = 0; i < MaxFloor; i++)
        {

            if (i > floor)
            {
                List<StructureProp> structures = BuildingTool.Instance.GetStructuresInFloor(i);

                for (int b = 0; b < structures.Count; b++)
                {
                    if (structures[b].transform.GetChild(0).gameObject.activeInHierarchy)
                        structures[b].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                List<StructureProp> structures = BuildingTool.Instance.GetStructuresInFloor(i);

                for (int b = 0; b < structures.Count; b++)
                {
                    if(!structures[b].transform.GetChild(0).gameObject.activeInHierarchy)
                        structures[b].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetIsMouseOnUI(bool state)
    {
        IsMouseOnUI = state;
    }
}
