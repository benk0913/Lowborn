using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "DataObjects/Need", order = 2)]
public class Need : ScriptableObject
{
    [SerializeField]
    public Sprite Icon;

    [SerializeField][Tooltip("How much to deteriorate per second (precent between 0 to 1  [Default is 0.5%])")]
    public float BaseDecrease = 0.0005f;
}
