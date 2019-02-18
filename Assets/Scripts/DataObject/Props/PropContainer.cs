using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropContainer", menuName = "DataObjects/Building/PropContainer", order = 2)]
public class PropContainer : Prop
{
    [Tooltip("Size in weight")]
    public int ContainerStorageSize;
}

