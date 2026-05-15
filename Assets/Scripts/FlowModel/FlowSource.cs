using UnityEngine;

/// <summary>
/// Source of pollutants in the Flow model simulation.
/// </summary>
public class FlowSource : MonoBehaviour {
    public bool SourceVisible = false;
    public bool SourceActive = false;
    public Pollutant Pollutant;
    public PollutionSource ObjectType;
    public FlowRoom Room;

    // when pollutant set to non fresh air, add to active sources
    public void AddToSourceList() {
        FlowController.Instance.ActiveSources.Add(this);
    }
}