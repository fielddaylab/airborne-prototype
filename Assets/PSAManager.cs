using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PSAManager : MonoBehaviour
{
    public Button FinishButton;
    public ResultsMap PSAData;

    [Header("Background")]
    public Button BLeft, BRight;
    public Image BackgroundImage;
    public Image PosterBackground;
    public TMP_Text BackgroundLabel;
    private int BCycleIndex = 0;

    [Header("Subject")]
    public Button SLeft, SRight;
    public Image SubjectImage;
    public Image PosterSubject;
    public TMP_Text SubjectLabel;
    private int SCycleIndex = 0;

    [Header("Message")]
    public Button MLeft, MRight;
    public TMP_Text MessageText;
    public TMP_Text PosterText;
    public TMP_Text MessageLabel;
    private int MCycleIndex = 0;

    public void Start()
    {
        FinishButton.onClick.AddListener(OnFinish);

        BLeft.onClick.AddListener(() => CycleBackground(-1));
        BRight.onClick.AddListener(()=> CycleBackground(1));

        SLeft.onClick.AddListener(() => CycleSubject(-1));
        SRight.onClick.AddListener(()=> CycleSubject(1));

        MLeft.onClick.AddListener(() => CycleMessage(-1));
        MRight.onClick.AddListener(()=> CycleMessage(1));

        CycleBackground(0);
        CycleSubject(0);
        CycleMessage(0);
    }

    private void OnFinish()
    {
        NewGameManager.Instance.FinalLoopData.PosterFeature = PSAData.BackgroundSets[BCycleIndex].RelevantPolluter;
        NewGameManager.Instance.FinalLoopData.PosterSubjectPollutant = PSAData.SubjectSets[SCycleIndex].RelevantPollutant;
        NewGameManager.Instance.FinalLoopData.PosterMessagePollutant = PSAData.MessageSets[SCycleIndex].RelevantPollutant;
        
        NewGameManager.Instance.SwitchToPhase(NewGamePhase.Results);
    }

    private void CycleBackground(int dir)
    {
        BCycleIndex += dir;
        if (BCycleIndex > PSAData.BackgroundSets.Length - 1) BCycleIndex = 0;
        if (BCycleIndex < 0) BCycleIndex = PSAData.BackgroundSets.Length - 1;

        BackgroundImage.sprite = PSAData.BackgroundSets[BCycleIndex].BackgroundSprite;
        PosterBackground.sprite = PSAData.BackgroundSets[BCycleIndex].BackgroundSprite;
        BackgroundLabel.text = PSAData.BackgroundSets[BCycleIndex].Label;
    }

    private void CycleSubject(int dir)
    {
        SCycleIndex += dir;
        if (SCycleIndex > PSAData.SubjectSets.Length - 1) SCycleIndex = 0;
        if (SCycleIndex < 0) SCycleIndex = PSAData.SubjectSets.Length - 1;

        SubjectImage.sprite = PSAData.SubjectSets[SCycleIndex].SubjectSprite;
        PosterSubject.sprite = PSAData.SubjectSets[SCycleIndex].SubjectSprite;
        SubjectLabel.text = PSAData.SubjectSets[SCycleIndex].Label;
    }

    private void CycleMessage(int dir)
    {
        MCycleIndex += dir;
        if (MCycleIndex > PSAData.MessageSets.Length - 1) MCycleIndex = 0;
        if (MCycleIndex < 0) MCycleIndex = PSAData.MessageSets.Length - 1;

        MessageText.text = PSAData.MessageSets[MCycleIndex].Label;
        PosterText.text = PSAData.MessageSets[MCycleIndex].Label;
        MessageLabel.text = PSAData.MessageSets[MCycleIndex].Label;
    }
}
