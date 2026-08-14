using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public CyclerHighlighterButton SelectedSlot;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogWarning("Two UI Managers in scene.");
            Destroy(gameObject);
        }
    }

    public void HandleSlotClick(CyclerHighlighterButton slot)
    {

    }
}
