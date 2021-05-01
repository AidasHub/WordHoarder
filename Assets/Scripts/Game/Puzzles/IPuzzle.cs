using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPuzzle
{
    public void InitPuzzle(TextAsset puzzle, UnityAction rewardAction);
}
