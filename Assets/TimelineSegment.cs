using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public void AssembleChunk(TimelineStep timeStep)
    {
        for (int i = 0; i < 1; i++) // only do the first one for now
        {
            if (timeStep.roomSteps.Length == 0) return;

            RoomStep roomStep = timeStep.roomSteps[i];

            foreach (var pollutantStep in roomStep.pollutantSteps)
            {
                GameObject sensorStep = Instantiate(SensorPrefab);
                RawImage image = sensorStep.GetComponent<RawImage>();
                switch (pollutantStep.pollutantType)
                {
                    case Pollutant.CO2:
                        image.texture = COSensors[pollutantStep.concentration];
                        break;
                    case Pollutant.NO:
                        image.texture = NOSensors[pollutantStep.concentration];
                        break;
                    case Pollutant.VOC:
                        image.texture = VOCSensors[pollutantStep.concentration];
                        break;
                }
                sensorStep.transform.SetParent(SensorBlockParent.transform);
                sensorStep.transform.localScale = Vector3.one;
            }
        }
    }
}
