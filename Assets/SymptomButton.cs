using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SymptomButton : MonoBehaviour
{
    public Image SymptomImage;
    public Button Button;
    public Symptom Symptom;

    void OnEnable()
    {
        Button.onClick.AddListener(HandleClicked);
    }

    void OnDisable()
    {
        Button.onClick.RemoveListener(HandleClicked);
    }

    private void HandleClicked()
    {
        SourceAndSymptomManager.OnSelectSymptom?.Invoke(Symptom);
    }
}
