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

    public int NumPips = 0;
    public int UsedPips = 0;
    
    public void Setup(EquipmentType toolType)
    {
        ToolImage.sprite = EquipmentMapUtility.GetSprite(Map, toolType);

        foreach (var pip in ToolPips)
        {
            pip.gameObject.SetActive(false);
        }

        if (EquipmentMapUtility.UsesPips(Map, toolType))
        {
            NumPips = EquipmentMapUtility.GetNumPips(Map, toolType);
            UsedPips = NumPips;
            
            for (int i = 0; i < NumPips; i++ )
            {
                ToolPips[i].gameObject.SetActive(true);
            }
        }

        ToolType = toolType;
    }
}
