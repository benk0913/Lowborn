using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleOnlyInBuildMode : MonoBehaviour
{
    [SerializeField]
    GameObject Object;

    private void Awake()
    {
        AddListeners();
        UpdateState();
    }

    void AddListeners()
    {
        BuildingTool.Instance.OnActivate.AddListener(UpdateState);
        BuildingTool.Instance.OnDeactivate.AddListener(UpdateState);
    }

    void UpdateState()
    {
        Object.SetActive(BuildingTool.Instance.isToolActive);
    }
}
