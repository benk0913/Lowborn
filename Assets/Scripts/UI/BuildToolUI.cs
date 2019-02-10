using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildToolUI : MonoBehaviour
{
    [SerializeField]
    Transform IconsContainer;

    private void Start()
    {
        Initialize();    
    }

    private void Initialize()
    {
        GameObject tempItem;
        for(int i=0;i< CORE.Instance.Database.Buildable.Count;i++)
        {
            tempItem = ResourcesLoader.Instance.GetRecycledObject("buildingItem");

            tempItem.transform.SetParent(IconsContainer, false);

            tempItem.GetComponent<SelectableBuildItemUI>().SetInfo(CORE.Instance.Database.Buildable[i]);
        }
    }


}
