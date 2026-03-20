using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private DiffusionManager diffusionManager;
    
    public void SelectThisRoom()
    {
        diffusionManager.SelectedRoom(this);
    }
}
