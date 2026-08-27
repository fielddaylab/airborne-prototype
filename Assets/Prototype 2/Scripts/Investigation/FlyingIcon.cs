using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingIcon : MonoBehaviour
{
    public Image Icon;
    
    public void Setup(Sprite icon, Transform destination, float delay = 0)
    {
        Icon.sprite = icon;
        StartCoroutine(Fly(destination, delay));
    }

    IEnumerator Fly(Transform destination, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Vector3 start = transform.position;
        Vector3 end = destination.position;

        Vector3 control = (start + end) * 0.5f + Vector3.up * 10;

        float duration = 0.75f;

        for (float t = 0; t < 1f; t += Time.deltaTime / duration)
        {
            transform.position = Bezier(start, control, end, t);
            yield return null;
        }

        transform.position = end;
        Destroy(gameObject);
    }

    public Vector3 Bezier(Vector3 start, Vector3 control, Vector3 target, float t)
    {
        float u = (1f - t);
        return u * u * start + 2 * u * t * control + t * t * target;
    }
}
