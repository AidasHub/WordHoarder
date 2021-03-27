using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class _PuzzleInfo
{
    public string textEN;
    public string[] missingWordsEN;
    public string textOther;
    public string[] missingWordsOther;

    public static _PuzzleInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<_PuzzleInfo>(jsonString);
    }

}
