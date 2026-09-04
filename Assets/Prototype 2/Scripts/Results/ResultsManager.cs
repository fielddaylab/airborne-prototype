using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public Button FinishButton;
    public GameObject End;
    public ResultsMap ResultsData;

    public float WaitTime = 0.2f;

    public ResultPanel[] Results;
    public Sprite FilledStar, EmptyStar;

    private ScenarioDataObject Scenario;
    private WinConditionDataObject WinCons;

    void Setup()
    {
        FinishButton.onClick.AddListener(OnFinish);

        Scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        WinCons = Scenario.WinConditions;
        
        for (int i = 0; i < Results.Length; i++)
        {
            Results[i].gameObject.SetActive(false);
            Results[i].Result.sprite = EmptyStar;
            switch (i)
            {
                case 0:
                Results[0].Text.text = $"{Scenario.MainNpc} passed out...";
                break;
                case 1:
                Results[1].Text.text = $"{WinCons.MainPollutant} still present...";
                break;
                case 2:
                Results[2].Text.text = $"{WinCons.RedherringPollutant} still present";
                break;
                case 3:
                Results[3].Text.text = "Irrelevant PSA";
                break;
                case 4:
                Results[4].Text.text = "x Loops";
                break;
            }
        }

        FinishButton.enabled = false;
    }

    public void EvaluateResults(FinalLoopTracker finalLoop)
    {
        Setup();
        

        // check if npc is saved
        // for now, if the target obj is replaced, or if a fan/purifier is in his room
        if ((WinCons.ReplaceFurnace && finalLoop.ReplacedFurnace) || (WinCons.ReplaceStove && finalLoop.ReplacedStove))
        {
            Results[0].Result.sprite = FilledStar;
            Results[0].Text.text = $"Saved {Scenario.MainNpc}!";
        }

        // check if co is cleared
        if (finalLoop.ReplacedFurnace)
        {
            Results[1].Result.sprite = FilledStar;
            Results[1].Text.text = $"Cleared {WinCons.MainPollutant}!";
        }

        // check if no is cleared
        if (finalLoop.ReplacedStove)
        {
            Results[2].Result.sprite = FilledStar;
            Results[2].Text.text = $"Cleared {WinCons.RedherringPollutant}!";
        }

        // check psa relevancy
        int numRelevant = 0;
        if (finalLoop.PosterFeature == Scenario.WinConditions.PosterFeature) numRelevant++;
        if (finalLoop.PosterSubjectPollutant == Scenario.WinConditions.PosterSubjectPollutant) numRelevant++;
        if (finalLoop.PosterMessagePollutant == Scenario.WinConditions.PosterMessagePollutant) numRelevant++;

        if (numRelevant >= 2)
        {
            Results[3].Result.sprite = FilledStar;
            Results[3].Text.text = "Relevant PSA!";
        }

        // check loop count
        if (NewGameManager.Instance.Statistics.NumLoops <= Scenario.WinConditions.TargetLoopCount)
        {
            Results[4].Result.sprite = FilledStar;
        }

        Results[4].Text.text = $"{NewGameManager.Instance.Statistics.NumLoops} loops.";

        StartCoroutine(AnimateResults());
    }

    public IEnumerator AnimateResults()
    {
        yield return new WaitForSeconds(0.8f);
        
        for (int i = 0; i < Results.Length; i++)
        {
            Results[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(WaitTime);
        }

        FinishButton.enabled = true;
    }

    private void OnFinish()
    {
        End.SetActive(true);
    }
}
