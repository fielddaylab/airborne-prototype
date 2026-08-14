using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button[] buttons;
    [SerializeField] private int startTab = 1;

    private void Start()
    {
        SwitchTo(startTab);
    }

    public void SwitchTo(int t)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (t == i) 
            { 
                tabs[i].SetActive(true);
                buttons[i].interactable = false;
            } else
            {
                tabs[i].SetActive(false);
                buttons[i].interactable = true;
            }
        }
    }
}
