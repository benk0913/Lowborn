using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "DataObjects/Trait", order = 2)]
public class Trait : ScriptableObject
{
    [SerializeField]
    public string Description;

    [SerializeField]
    public DateTime? ExpirationDate = null;

    [SerializeField]
    public List<TraitBonus> Bonuses = new List<TraitBonus>();

    [SerializeField]
    public Trait NextStageTrait;

    [SerializeField]
    public Trait CounterTrait;
}