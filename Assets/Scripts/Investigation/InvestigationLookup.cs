using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationLookup : MonoBehaviour
{
    public static InvestigationLookup Instance {get; private set;}

    public void Start()
    {
        if (Instance != null) {Destroy(gameObject); return; }
        Instance = this;
    }

    public SourceImageObject SourceImages;
    public PollutantKnowledgeMapObject PollutantMap;
    public CharacterSpriteMapObject CharacterMap;
    public SymptomSpriteMapObject SymptomMap;
    public RoomSpriteMapObject RoomMap;
}
