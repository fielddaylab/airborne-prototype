using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDragAnchor : MonoBehaviour
{
    public static InventoryDragAnchor Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
