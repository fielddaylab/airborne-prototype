using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSAManager : MonoBehaviour
{
    public Button FinishButton;

    public void Start()
    {
        FinishButton.onClick.AddListener(OnFinish);
    }

    private void OnFinish()
    {
        NewGameManager.Instance.SwitchToPhase(NewGamePhase.Results);
    }
}
