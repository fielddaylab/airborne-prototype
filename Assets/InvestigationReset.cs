using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InvestigationReset : MonoBehaviour
{
    [SerializeField] private string m_SceneToLoad;
    private Button m_Button;

    private void OnEnable()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(LoadScene);
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    public void LoadScene()
    {
        PlayerKnowledgeState.Reset();
        SceneManager.LoadScene(m_SceneToLoad);
    }
}
