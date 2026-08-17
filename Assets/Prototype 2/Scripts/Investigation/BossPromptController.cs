using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPromptController : MonoBehaviour
{
    public static List<Image> CheckMarks = new();

    public Transform TargetTransform;

    public float duration;

    public Sprite checkSprite;

    public GameObject BossParent;

    public Slider PersusasionSlider;
    public int MaxValue;

    public TMP_Text BossText;

    public Button AdvanceButton, GoBackButton;

    public RescuePlannerManager RescuePlanner;

    public CaseFileManager CaseFile;
    public ToolManager ToolManager;

    public void Start()
    {
        BossParent.SetActive(false);
        PersusasionSlider.value = 0;
        PersusasionSlider.maxValue = MaxValue;
        AdvanceButton.interactable = false;
        GoBackButton.interactable = false;
    }

    public void OnEnable()
    {
        AdvanceButton.onClick.AddListener(Advance);
        GoBackButton.onClick.AddListener(GoBack);
    }

    public void OnDisable()
    {
        
    }

    public static void RegisterCheckmark(Image mark)
    {
        CheckMarks.Add(mark);
    }

    private void Advance()
    {
        CaseFile.SetCaseFile(false);
        ToolManager.ClearTool();

        RescuePlanner.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void GoBack()
    {
        Debug.Log("TO DO");
    }

    public void AnimateCheckMarkMovement()
    {
        int animationIndex = 0;
        for (int i = 0; i < CheckMarks.Count; i++)
        {
            if (CheckMarks[i] != null && CheckMarks[i].enabled && CheckMarks[i].sprite == checkSprite && CheckMarks[i].gameObject.activeInHierarchy) {
                StartCoroutine(MoveCheck(CheckMarks[i], animationIndex, i == (CheckMarks.Count - 1)));
                animationIndex++;
            }
        }
    }

    IEnumerator MoveCheck(Image check, int index, bool finalCheck = false)
    {
        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(index * 0.2f);
        
        Vector3 start = check.transform.position;
        Vector3 end = TargetTransform.position;

        Vector3 control = (start + end) * 0.5f + Vector3.up * 10;

        float duration = 0.75f;

        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            check.transform.position = Bezier(start, control, end, t);
            yield return null;
        }

        check.transform.position = end;
        check.enabled = false;

        PersusasionSlider.value += 1;

        if (finalCheck) EndBossSequence();
    }

    private void EndBossSequence()
    {
        BossText.text = $"With this evidence, you theory has about an <b>{PersusasionSlider.value}0% chance</b> of being correct. Are you ready to plan your Rescue?";
        AdvanceButton.interactable = true;
        GoBackButton.interactable = true;
    }

    public void StartBossSequence(PollutantType selectedPollutant, FeatureType selectedSource)
    {
        BossParent.SetActive(true);

        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;
        RoomType sourceRoom = ScenarioUtility.GetRoom(selectedSource, scenarioData);

        string pollutantName = InvestigationLookup.Instance.PollutantMap.GetFullName(selectedPollutant);

        BossText.text = $"So it was <b>{pollutantName}</b> from the <b>{sourceRoom} {selectedSource}</b> that caused roundy to faint? Let's see your evidence...";

        AnimateCheckMarkMovement();
    }

    public Vector3 Bezier(Vector3 start, Vector3 control, Vector3 target, float t)
    {
        float u = (1f - t);
        return u * u * start + 2 * u * t * control + t * t * target;
    }
}
