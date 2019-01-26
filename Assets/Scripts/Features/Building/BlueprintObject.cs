using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintObject : MonoBehaviour
{
    [SerializeField]
    public GameObject MeshObject;

    [SerializeField][TooltipAttribute("Leave empty if the target object is self.")]
    string TargetObject;
}
