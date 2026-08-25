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

    void Setup()
    {
        FinishButton.onClick.AddListener(OnFinish);
        
        for (int i = 0; i < Results.Length; i++)
        {
            Results[i].gameObject.SetActive(false);
            Results[i].Result.sprite = EmptyStar;
            switch (i)
            {
                case 0:
                Results[0].Text.text = "Roundy passed out...";
                break;
                case 1:
                Results[1].Text.text = "CO still present...";
                break;
                case 2:
                Results[2].Text.text = "NOx still present";
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
        
        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;

        // check if roundy is saved
        // for now, if the furnace is replaced, or if a fan/purifier is in his room
        if (finalLoop.ReplacedFurnace || finalLoop.FilterPlacement == scenario.WinConditions.PlaceFilterInRoom 
            || finalLoop.PlacedFans.Contains(scenario.WinConditions.PlacedFansInRooms[0]))
        {
            Results[0].Result.sprite = FilledStar;
            Results[0].Text.text = "Saved Roundy!";
        }

        // check if co is cleared
        if (finalLoop.ReplacedFurnace)
        {
            Results[1].Result.sprite = FilledStar;
            Results[1].Text.text = "Cleared CO!";
        }

        // check if no is cleared
        if (finalLoop.ReplacedStove)
        {
            Results[2].Result.sprite = FilledStar;
            Results[2].Text.text = "Cleared CO!";
        }

        // check psa relevancy
        int numRelevant = 0;
        if (finalLoop.PosterFeature == scenario.WinConditions.PosterFeature) numRelevant++;
        if (finalLoop.PosterSubjectPollutant == scenario.WinConditions.PosterSubjectPollutant) numRelevant++;
        if (finalLoop.PosterMessagePollutant == scenario.WinConditions.PosterMessagePollutant) numRelevant++;

        if (numRelevant >= 2)
        {
            Results[3].Result.sprite = FilledStar;
            Results[3].Text.text = "Relevant PSA!";
        }

        // check loop count
        if (NewGameManager.Instance.Statistics.NumLoops <= scenario.WinConditions.TargetLoopCount)
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
