using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableBuildItem : Selectable
{
    [SerializeField]
    string ItemPrefab;

    public override void Select()
    {
        base.Select();

        BuildingTool.Instance.SetBlueprintItem(ItemPrefab);
    }
}
