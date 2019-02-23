using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableBuildItemUI : SelectableUI
{
    [SerializeField]
    public Image Icon;

    [SerializeField]
    Prop currentProp;

    public void SetInfo(Prop prop)
    {
        
        Icon.sprite = prop.Icon != null? prop.Icon : ResourcesLoader.Instance.GetSprite(prop.Prefab.name);

        this.currentProp = prop;
    }

    public override void Select()
    {
        base.Select();

        BuildingTool.Instance.SetBlueprintItem(currentProp);
    }
}
