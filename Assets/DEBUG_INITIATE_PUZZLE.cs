using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_INITIATE_PUZZLE : MonoBehaviour
{
    public GameObject interactionPanel;
    PuzzleWordFill puzzleWordFill;
    private void Start()
    {
        puzzleWordFill = interactionPanel.GetComponentInChildren<PuzzleWordFill>();
    }

    public void DEBUG()
    {
        interactionPanel.GetComponent<Animator>().SetTrigger("ToggleInteractionPanel");
        InventoryManager.getInstance().AddWord("Dog");
        InventoryManager.getInstance().AddWord("cat");
        InventoryManager.getInstance().AddWord("Bucket");
        InventoryManager.getInstance().AddWord("computer");
        InventoryManager.getInstance().AddWord("table");
        InventoryManager.getInstance().AddWord("plants");
        puzzleWordFill.Init();
        Destroy(this.gameObject);
    }
}
