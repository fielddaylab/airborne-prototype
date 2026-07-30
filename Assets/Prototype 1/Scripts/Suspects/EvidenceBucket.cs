using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvidenceBucket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BucketText = null;
    [SerializeField] private GameObject EvidenceLogger;
    [SerializeField] private GameObject evidenceParent;

    private int evidencePieces = 0;

    public void AddEvidence(string text)
    {
        if (evidencePieces >= 3) return;
        evidencePieces++;

        Debug.Log(text);
        GameObject evidence = Instantiate(EvidenceLogger);
        evidence.transform.SetParent(evidenceParent.transform);
        evidence.transform.localScale = Vector3.one; // not sure why it has a different scale?

        EvidencePiece evidencePiece = evidence.GetComponent<EvidencePiece>();
        evidencePiece.SetText(text);
    }

    public void ClearEvidence()
    {
        for (int i = 0; i < evidenceParent.transform.childCount; i++)
        {
            Destroy(evidenceParent.transform.GetChild(i).gameObject);
        }
        evidencePieces = 0;
    }

    public void SetText(string text)
    {
        BucketText.text = text;
    }

    public void ClearText()
    {
        BucketText.text = "<i>evidence bucket</i>";
    }
}
