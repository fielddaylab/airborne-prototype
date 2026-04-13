using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineLoader : MonoBehaviour
{
    [SerializeField] private TimelineObject _timelineToLoad;
    [SerializeField] private GameObject _timelineChunkPrefab;
    [SerializeField] private GameObject _timelineChunkParent;

    void Start()
    {
        DestroyTimeline();
        AssembleTimeline();
    }

    private void DestroyTimeline()
    {
        foreach (Transform child in _timelineChunkParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // there are probably better ways to go about this
    private void AssembleTimeline()
    {
        foreach (TimelineStep timeStep in _timelineToLoad.timeline)
        {
            GameObject timeChunk = Instantiate(_timelineChunkPrefab);
            timeChunk.transform.SetParent(_timelineChunkParent.transform);
            timeChunk.transform.localScale = Vector3.one; // still not sure why this is needed?
            TimelineSegment timeSegment = timeChunk.GetComponent<TimelineSegment>();
            timeChunk.name = "" + timeStep.hourTime;
            timeSegment.AssembleChunk(timeStep);
        }
    }
}
