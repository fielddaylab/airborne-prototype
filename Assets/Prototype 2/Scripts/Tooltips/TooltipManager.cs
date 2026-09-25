using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public TooltipDisplay Renderer;
    
    private TooltipHoverable _lastTooltip;
    private int _lastTooltipVersion;
    public float ToolTipTime;
    private float _toolTipCounter;

    void Start()
    {
        Renderer.HideTooltip();
    }

    void Update()
    {
        if (TooltipHoverable.CurrentHovered != _lastTooltip)
        {
            _lastTooltip = TooltipHoverable.CurrentHovered;
            _toolTipCounter = 0;
            Renderer.HideTooltip();
        }

        if (_lastTooltip != null) 
        {
            if (_toolTipCounter < ToolTipTime)
            {
                _toolTipCounter += Time.deltaTime;
                if (_toolTipCounter >= ToolTipTime)
                {
                    Renderer.ShowTooltip(_lastTooltip.TooltipText);
                    _lastTooltipVersion = _lastTooltip.Version;
                }
            } 
            else
            {
                if (_lastTooltipVersion != _lastTooltip.Version)
                {
                    Renderer.ShowTooltip(_lastTooltip.TooltipText);
                    _lastTooltipVersion = _lastTooltip.Version;
                }
            }
        }
    }
}
