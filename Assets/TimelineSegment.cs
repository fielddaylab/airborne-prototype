using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineSegment : MonoBehaviour
{
    public Texture2D[] COSensors, NOSensors, VOCSensors;

    [System.Serializable]
    public struct SourceTexturePair
    {
        public PollutionSource source;
        public Texture2D textureOn;
        public Texture2D textureOff;
    }

    public List<SourceTexturePair> sourceTextures;

    //public Texture2D dialogueBubble;

    [System.Serializable]
    public struct SymptomTexturePair
    {
        public Symptom symptom;
        public Texture2D symptomTexture;
    }


    public GameObject DialogueImagePrefab, PollutantImagePrefab, SensorPrefab;
    public GameObject SensorBlockParent, DialogueSymptomParent, SourceStatusParent;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
