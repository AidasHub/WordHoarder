using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    public List<TextAsset> wordFillPuzzles;

    private static InteractiveManager instance;

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

    public static InteractiveManager getInstance()
    {
        return instance;
    }

    public TextAsset GetWordFillPuzzle(int i)
    {
        return wordFillPuzzles[i];
    }

    public void ToggleInteraction()
    {
        if(!animator.GetBool("InteractionPanelEnabled"))
        {
            animator.SetBool("InteractionPanelEnabled", true);
        }
        else
        {
            animator.SetBool("InteractionPanelEnabled", false);
        }
        
    }
}
