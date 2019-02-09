using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTool : MonoBehaviour
{
    public static PlayTool Instance;

    public bool isToolActive;

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

        if(Input.GetMouseButtonDown(1))
        {
            if (LocationMap.Instance.StructureMouseHit.collider != null)
            {
                LocationMap.Instance.Data.PlayerActor.NavigateTo(LocationMap.Instance.GroundMouseHit.point);
            }
            else if (LocationMap.Instance.GroundMouseHit.collider != null)
            {
                LocationMap.Instance.Data.PlayerActor.NavigateTo(LocationMap.Instance.GroundMouseHit.point);
            }
        }
    }


    public void ActivateTool()
    {
        if (isToolActive)
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
    }
}
