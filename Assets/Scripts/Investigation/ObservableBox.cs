using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableBox : MonoBehaviour
{
    public string DataPointName;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        gameObject.SetActive(false);
    }

    public void Oestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    private void OnMouseDown()
    {
        Debug.Log(DataPointName + " was clicked.");
    }

    private void HandleToolUpdated(ToolType type)
    {
        if (type == ToolType.Observe) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
