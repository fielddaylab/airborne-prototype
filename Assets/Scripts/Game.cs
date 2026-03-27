using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public SuspectObject currentSuspect;
    public GamePhase gamePhase;
    public Prompter prompt;

    [SerializeField] private DiffusionManager diffusionManager = null;

    public void CheckPollutant(Pollutant p)
    {
        if (gamePhase != GamePhase.FindingPollutant) return;
        
        if (currentSuspect.pollutant == p)
        {
            prompt.SetText($"What was the source of {p}?");
            gamePhase = GamePhase.SelectingSource;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        gamePhase = GamePhase.FindingPollutant;
        diffusionManager.modeling = false;
    }

    void Update()
    {
        
    }
}

public enum GamePhase
{
    FindingPollutant,
    SelectingSource,
    DeterminingSpread
}