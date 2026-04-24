using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneLoader : MonoBehaviour
{
    [SerializeField] string _sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
