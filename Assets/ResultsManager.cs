using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public Button FinishButton;
    public GameObject End;

    void Start()
    {
        FinishButton.onClick.AddListener(OnFinish);
    }

    private void OnFinish()
    {
        End.SetActive(true);
    }
}
