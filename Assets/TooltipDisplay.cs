using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipDisplay : MonoBehaviour
{
    public TMP_Text Label;
    public float OffsetDistance;
    
    public void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        Label.text = text;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);

    }

    public void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

        float width = Screen.width;
        float height = Screen.height;

        bool isLeft = mousePos.x < width / 2;
        bool isDown = mousePos.y < height / 2;

        RectTransform rect = (RectTransform) transform;
        Vector2 pivot;
        pivot.x = isLeft ? 0 : 1;
        pivot.y = isDown ? 0 : 1;

        rect.pivot = pivot;

        if (isLeft)
        {
            mousePos.x += OffsetDistance;
        } else
        {
            mousePos.x -= OffsetDistance;
        }
        
        if (isDown)
        {
            mousePos.y += OffsetDistance;
        } else
        {
            mousePos.y -= OffsetDistance;
        }

        mousePos.z = rect.position.z;
        rect.position = mousePos;
    }
}
