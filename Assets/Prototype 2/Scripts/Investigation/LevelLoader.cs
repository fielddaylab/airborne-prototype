using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Button MyButton;
    public ScenarioDataObject LevelScenario;
    public string LevelScene = "Level";

    void OnEnable()
    {
        MyButton.onClick.AddListener(LoadLevel);
    }

    void OnDisable()
    {
        MyButton.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(LevelScene);
    }
}
