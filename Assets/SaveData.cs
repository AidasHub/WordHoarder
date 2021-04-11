using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int currentEnvironment;
    public List<Tuple<string, bool>> environmentStatus;
    public List<Tuple<string, bool>> worldWords;
    public List<string> inventoryWords;

    public SaveData(int ce, List<Tuple<string, bool>> es, List<Tuple<string, bool>> ww, List<string> iw)
    {
        currentEnvironment = ce;
        environmentStatus = es;
        worldWords = ww;
        inventoryWords = iw;
    }
}
