using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        BossParent.SetActive(false);
        PersusasionSlider.value = 0;
        PersusasionSlider.maxValue = MaxValue;
    }

    public static void RegisterCheckmark(Image mark)
    {
        CheckMarks.Add(mark);
    }

    public void AnimateCheckMarkMovement()
    {
        int animationIndex = 0;
        for (int i = 0; i < CheckMarks.Count; i++)
        {
            if (CheckMarks[i].enabled && CheckMarks[i].sprite == checkSprite && CheckMarks[i].gameObject.activeInHierarchy) {
                StartCoroutine(MoveCheck(CheckMarks[i], animationIndex));
                animationIndex++;
            }
        }
    }

    IEnumerator MoveCheck(Image check, int index)
    {
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
    }

    public void StartBossSequence()
    {
        BossParent.SetActive(true);
        AnimateCheckMarkMovement();
    }

    public Vector3 Bezier(Vector3 start, Vector3 control, Vector3 target, float t)
    {
        float u = (1f - t);
        return u * u * start + 2 * u * t * control + t * t * target;
    }
}
