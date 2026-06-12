using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableCloud : MonoBehaviour
{
    public string CloudName;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    private void HandleToolUpdated(ToolType type)
    {
        if (type == ToolType.Scan) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
