using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        RoomController.RoomSelected += HandleRoomSelected;
    }
    private void HandleRoomSelected(object sender, EventArgs e)
    {
        RoomController room = sender as RoomController;

        if (gamePhase == GamePhase.SelectingSource)
        {
            if (room.roomType == currentSuspect.sourceRoom)
            {
                Debug.Log("YOU DID HAVE A COOKIE!");
                gamePhase = GamePhase.DeterminingSpread;
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
    FindingPollutant,
    SelectingSource,
    DeterminingSpread
}