using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanTool : MonoBehaviour
{
    public EquipmentMapObject EquipmentMap;
    public SpriteRenderer ToolImage;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        ToolImage.enabled = false;
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

        ToolImage.flipY = direction.x < 0;
    }

    private void HandleToolUpdated(EquipmentType type)
    {
        if (type == EquipmentType.None)
        {
            ToolImage.enabled = false;
        } else
        {
            ToolImage.enabled = true;
            ToolImage.sprite = EquipmentMapUtility.GetSprite(EquipmentMap, type);
        }
    }   
}
