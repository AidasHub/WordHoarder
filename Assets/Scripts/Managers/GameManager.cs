using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<TextAsset> wordFillPuzzles;

    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {

    }

    public static GameManager getInstance()
    {
        return instance;
    }

    public TextAsset GetWordFillPuzzle(int i)
    {
        return wordFillPuzzles[i];
    }


}
