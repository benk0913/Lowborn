using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildToolUI : MonoBehaviour
{
    private void OnEnable()
    {
        BuildingTool.Instance.ActivateTool();
    }

    private void OnDisable()
    {
        BuildingTool.Instance.DeactivateTool();
    }
}
