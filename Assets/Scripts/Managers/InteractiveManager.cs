using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PuzzleWordFill puzzleWordFill;

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
        animator = UIManager.getInstance().GetInteractionPanel().GetComponent<Animator>();
        puzzleWordFill = UIManager.getInstance().GetInteractionPanel().GetComponentInChildren<PuzzleWordFill>();
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

    public void LoadWordFillPuzzle(int index)
    {
        puzzleWordFill.InitPuzzle(index);
        ToggleInteraction();
    }
}
