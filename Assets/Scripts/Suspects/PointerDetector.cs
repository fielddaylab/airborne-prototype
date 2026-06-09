using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private EvidenceBucket evidenceBucket;

    [SerializeField]
    private string evidenceHoverText;
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        evidenceBucket.SetText(evidenceHoverText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        evidenceBucket.ClearText();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
