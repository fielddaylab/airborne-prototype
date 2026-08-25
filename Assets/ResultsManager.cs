using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public Button FinishButton;
    public GameObject End;
    public ResultsMap ResultsData;

    void Start()
    {
        FinishButton.onClick.AddListener(OnFinish);
    }

    public void EvaluateResults()
    {
        
    }

    private void OnFinish()
    {
        End.SetActive(true);
    }
}
