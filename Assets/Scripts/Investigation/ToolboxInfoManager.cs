using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolboxInfoManager : MonoBehaviour
{
    public static ToolboxInfoManager Instance;

    public Image EquipmentImage;
    public TMP_Text Label;
    public TMP_Text Description;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Populate(EquipmentMapObject map, EquipmentType type)
    {
        EquipmentImage.sprite = EquipmentMapUtility.GetSprite(map, type);
        Label.text = EquipmentMapUtility.GetLabel(map, type);
        Description.text = EquipmentMapUtility.GetDescription(map, type);
    }
}
