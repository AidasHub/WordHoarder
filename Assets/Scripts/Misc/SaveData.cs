using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int CurrentEnvironment;
    public List<Tuple<string, bool>> EnvironmentStatus;
    public List<Tuple<string, bool>> WorldWords;
    public List<string> InventoryWords;
    public int CollectedWords;
    public int TotalWords;

    public SaveData(int ce, List<Tuple<string, bool>> es, List<Tuple<string, bool>> ww, List<string> iw, int cw, int tw)
    {
        CurrentEnvironment = ce;
        EnvironmentStatus = es;
        WorldWords = ww;
        InventoryWords = iw;
        CollectedWords = cw;
        TotalWords = tw;
    }
}
