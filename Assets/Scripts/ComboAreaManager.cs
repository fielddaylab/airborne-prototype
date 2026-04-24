using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboAreaManager : MonoBehaviour
{
    public GameObject _selectedPollutantArea;

    [SerializeField] private SuspectPicker _suspectPicker;
    [SerializeField] private ComboCreator _comboCreator;

    public void LoadPollutant(GameObject pollutantPrefab)
    {
        GameObject prefab = Instantiate(pollutantPrefab);
        prefab.transform.SetParent(_selectedPollutantArea.transform);
        prefab.transform.localScale = Vector3.one;
        prefab.transform.localPosition = Vector3.zero;

        Button panelButton = prefab.GetComponentInChildren<Button>();
        panelButton.enabled = false;
    }

    public void ReturnToSuspects()
    {
        _comboCreator.ClearCombos();
        _suspectPicker.gameObject.SetActive(true);
        _suspectPicker.Restore();
        gameObject.SetActive(false);
    }
}
