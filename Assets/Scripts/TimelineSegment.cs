using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TimelineSegment : MonoBehaviour
{
    public Color EvenColor, OddColor;
    
    public Texture2D[] COSensors, NOSensors, VOCSensors;

    [System.Serializable]
    public struct SourceTexturePair
    {
        public PollutionSource source;
        public Texture2D textureOn;
        public Texture2D textureOff;
    }

    public List<SourceTexturePair> sourceTextures;

    [System.Serializable]
    public struct SymptomTexturePair
    {
        public Symptom symptom;
        public Texture2D symptomTexture;
    }

    public List<SymptomTexturePair> symptomTextures;

    public GameObject SensorBlockPrefab, DialogueImagePrefab, SymptomAndSourceImagePrefab;
    public GameObject SensorBlockParent, DialogueAndSymptomParent, SourceStatusParent;


    public void AssembleChunk(TimelineStep timeStep, FeedbackBlock feedbackBlock)
    {
        Image img = GetComponent<Image>();
        img.color = timeStep.hourTime % 2 == 0 ? EvenColor : OddColor;
        
        for (int i = 0; i < 1; i++) // only do the first one for now
        {
            if (timeStep.roomSteps.Length == 0) return;

            RoomStep roomStep = timeStep.roomSteps[i];

            // Sensor block backgrounds
            foreach (var pollutantStep in roomStep.pollutantSteps)
            {
                GameObject sensorStep = Instantiate(SensorBlockPrefab);
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

            foreach (var sourceStep in roomStep.sourceSteps)
            {
                GameObject sourceObj = Instantiate(SymptomAndSourceImagePrefab);
                RawImage image = sourceObj.transform.Find("Button Texture").GetComponent<RawImage>();
                SourceTexturePair match = sourceTextures.Find(x => x.source == sourceStep.pollutionSource);
                Texture2D matchingTexture = sourceStep.sourceAction == SourceAction.On ? match.textureOn : match.textureOff;
                image.texture = matchingTexture;

                sourceObj.transform.SetParent(SourceStatusParent.transform);
                sourceObj.transform.localScale = Vector3.one;

                TimelineButton sourceButton = sourceObj.GetComponent<TimelineButton>();
                string status = sourceStep.sourceAction == SourceAction.On ? " turns on." : " turns off.";
                sourceButton.Setup(feedbackBlock, sourceStep.pollutionSource.ToString(), matchingTexture, status);

                HighlighterSlot highlightSlot = sourceObj.GetComponent<HighlighterSlot>();
                highlightSlot.SlotType = SlotType.Source; // defaults to symptom so only need to set here
            }

            foreach (var characterStep in roomStep.characterSteps)
            {
                if (characterStep.dialogue != "")
                {
                    GameObject dialogueObj = Instantiate(DialogueImagePrefab);
                    dialogueObj.transform.SetParent(DialogueAndSymptomParent.transform);
                    dialogueObj.transform.localScale = Vector3.one;

                    TimelineButton dialogueButton = dialogueObj.GetComponent<TimelineButton>();
                    dialogueButton.Setup(feedbackBlock, characterStep.dialogue); // dialogue, no image
                }

                if (characterStep.observedSymptom != Symptom.None)
                {
                    GameObject symptomObj = Instantiate(SymptomAndSourceImagePrefab);
                    RawImage image = symptomObj.transform.Find("Button Texture").GetComponent<RawImage>();
                    SymptomTexturePair match = symptomTextures.Find(x => x.symptom == characterStep.observedSymptom);
                    image.texture = match.symptomTexture;

                    symptomObj.transform.SetParent(DialogueAndSymptomParent.transform);
                    symptomObj.transform.localScale = Vector3.one;

                    TimelineButton symptomButton = symptomObj.GetComponent<TimelineButton>();
                    symptomButton.Setup(feedbackBlock, "Roundy experiences", match.symptomTexture, characterStep.observedSymptom.ToString()); // character, image, symptom 
                }
            }
        }
    }
}
