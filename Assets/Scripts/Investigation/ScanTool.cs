using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTool : MonoBehaviour
{
    public SpriteRenderer ObserveRenderer;
    public SpriteRenderer ScanRenderer;
    public SpriteRenderer MeterRenderer;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        ObserveRenderer.enabled = false;
        ScanRenderer.enabled = false;
        MeterRenderer.enabled = false;
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    void Update()
    {
        Vector3 planePosition = Input.mousePosition;
        float distanceFromCamera = transform.position.z - Camera.main.transform.position.z;
        planePosition.z = distanceFromCamera;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(planePosition);
        worldPosition.z = transform.position.z;

        Vector3 direction = worldPosition - transform.position;
        transform.right = direction;

        ObserveRenderer.flipY = direction.x < 0;
        ScanRenderer.flipY = direction.x < 0;
        MeterRenderer.flipY = direction.x < 0;
    }

    private void HandleToolUpdated(ToolType type)
    {
        ObserveRenderer.enabled = false;
        ScanRenderer.enabled = false;
        MeterRenderer.enabled = false;

        switch (type)
        {
            case ToolType.Observe:
                ObserveRenderer.enabled = true;
                break;
            case ToolType.Scan:
                ScanRenderer.enabled = true;
                break;
            case ToolType.Meter:
                MeterRenderer.enabled = true;
                break;
        }
    }   
}
