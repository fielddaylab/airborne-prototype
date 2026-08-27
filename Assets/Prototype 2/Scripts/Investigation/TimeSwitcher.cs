using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSwitcher : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    public TimeAdjuster[] adjusters;
    [SerializeField] private int startTab = 0;


    void Start()
    {
        SwitchTo(startTab);
    }

    public void SwitchTo(int t)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (t == i) 
            { 
                buttons[i].interactable = false;
                adjusters[i].SetTime();
                Debug.Log("Setting time to " + adjusters[i].gameObject.name);
            } else
            {
                buttons[i].interactable = true;
            }
        }
    }
}
