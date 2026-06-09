using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableBox : MonoBehaviour
{
    public string DataPointName;
    
    private void OnMouseDown()
    {
        Debug.Log(DataPointName + " was clicked.");
    }
}
