using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] DiffusionManager diffusionManager;

    void Update()
    {
        slider.value = diffusionManager.time;
    }
}
