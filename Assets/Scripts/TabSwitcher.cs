using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;

    public void SwitchTo(int t)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (t == i) 
            { 
                tabs[i].SetActive(true); 
            } else
            {
                tabs[i].SetActive(false);
            }
        }
    }
}
