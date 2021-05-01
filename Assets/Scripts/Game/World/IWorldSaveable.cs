using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldSaveable
{
    public Tuple<string, bool> PrepareSaveData();
    public void LoadSaveData(bool isComplete);
}
