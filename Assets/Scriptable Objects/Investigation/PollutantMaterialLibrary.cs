using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Pollutant Material Lookup")]
public class PollutantMaterialLibrary : ScriptableObject
{
    public PollutantMaterialEntry[] Entries;

    private Dictionary<PollutantType, Material> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<PollutantType, Material>();
        foreach (var entry in Entries) {
            _lookup[entry.Pollutant] = entry.Material;
        }
    }

    public Material GetMaterial(PollutantType pollutant)
    {
        _lookup.TryGetValue(pollutant, out Material mat);
        return mat;
    }
}

[System.Serializable]
public class PollutantMaterialEntry
{
    public PollutantType Pollutant;
    public Material Material;
}
