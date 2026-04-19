using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCard : MonoBehaviour
{
    public int PersuasionValue = 1;

    public void Remove()
    {
        Destroy(gameObject);
    }
}
