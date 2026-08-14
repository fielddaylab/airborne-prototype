using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour
{
    public Image ToolImage;
    public Button MyButton;
    public Image[] ToolPips;
    public EquipmentMapObject Map;

    public EquipmentType ToolType;
    
    public void Setup(EquipmentType toolType)
    {
        ToolImage.sprite = EquipmentMapUtility.GetSprite(Map, toolType);

        foreach (var pip in ToolPips)
        {
            pip.gameObject.SetActive(false);
        }

        if (EquipmentMapUtility.UsesPips(Map, toolType))
        {
            int numPips = EquipmentMapUtility.GetNumPips(Map, toolType);

            for (int i = 0; i < numPips; i++ )
            {
                ToolPips[i].gameObject.SetActive(true);
            }
        }

        ToolType = toolType;
    }
}
