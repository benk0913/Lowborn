using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Session", menuName = "DataObjects/Session", order = 2)]
public class Session : ScriptableObject
{
    public Dictionary<string, Dynasty> Dynasties = new Dictionary<string, Dynasty>();
    public Dictionary<string, Character> Characters = new Dictionary<string, Character>();

    [System.NonSerialized]
    public Dynasty PlayerDynasty;

    [SerializeField]
    List<GameRule> Rules = new List<GameRule>();

    [SerializeField]
    Sprite Map;


}
