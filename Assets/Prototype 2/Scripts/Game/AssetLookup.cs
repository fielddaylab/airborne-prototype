using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLookup : MonoBehaviour
{
    public static AssetLookup Instance {get; private set;}

    public void Awake()
    {
        if (Instance != null) {Destroy(gameObject); return; }
        Instance = this;
    }

    public FeatureSpriteMapObject FeatureSpriteMap;
    public PollutantKnowledgeMapObject PollutantMap;
    public CharacterSpriteMapObject CharacterMap;
    public SymptomSpriteMapObject SymptomMap;
    public RoomSpriteMapObject RoomMap;
    public FeatureMapObject FeatureMap;
    public PollutantDataObject[] PollutantSymptomMaps;
}
