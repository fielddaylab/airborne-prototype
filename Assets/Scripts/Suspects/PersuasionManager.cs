using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersuasionManager : MonoBehaviour
{
    public static PersuasionManager Instance;
    
    [SerializeField] private Slider _slider;
    private int _persuasionValue;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAmount(int i)
    {
        _persuasionValue += i;
        _slider.value = _persuasionValue;
    }
}
