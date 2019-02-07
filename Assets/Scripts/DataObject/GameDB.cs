using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Database", menuName = "DataObjects/Game Database", order = 2)]
public class GameDB : ScriptableObject
{
    public List<RaceSet> Races = new List<RaceSet>();

    public List<Session> Scenarios = new List<Session>();

    public List<Prop> Buildable = new List<Prop>();

    public RaceSet GetRace(string raceName, bool fallback = true)
    {
        for(int i=0;i<Races.Count;i++)
        {
            if(raceName == Races[i].name)
            {
                return Races[i];
            }
        }

        if(fallback)
        {
            return Races[0];
        }

        return null;
    }
}
