using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SuspectObject currentSuspect;
    public GamePhase GamePhase;
    public Prompter prompt;

    [SerializeField] private DiffusionManager diffusionManager = null;

    public void CheckPollutant(Pollutant p)
    {
        if (GamePhase != GamePhase.FindingPollutant) return;
        
        if (currentSuspect.pollutant == p)
        {
            prompt.SetText($"What was the source of {p}?");
            GamePhase = GamePhase.SelectingSource;
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
        GamePhase = GamePhase.SelectingPollutant;
        diffusionManager.modeling = false;
        RoomController.RoomSelected += HandleRoomSelected;
    }
    private void HandleRoomSelected(object sender, EventArgs e)
    {
        RoomController room = sender as RoomController;

        if (GamePhase == GamePhase.SelectingSource)
        {
            if (room.roomType == currentSuspect.sourceRoom)
            {
                GamePhase = GamePhase.DeterminingSpread;
                prompt.SetText($"How did the <b>Carbon Monoxide</b> move through the building at <b>{diffusionManager.time + 1}PM</b>?");
                diffusionManager.modeling = true;
            }
        }
    }

    void Update()
    {
        
    }
}

public enum GamePhase
{
    SelectingPollutant,
    ArguingPollutant,
    FindingPollutant,
    SelectingSource,
    DeterminingSpread
}