using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
    [SerializeField] private int numRooms;
    [SerializeField] private bool directed;

    int[,] matrix;

    void Start()
    {
        GenerateEmptyMatrix();
    }

    private void GenerateEmptyMatrix()
    {
        matrix = new int[numRooms, numRooms];
        for (int i = 0; i < numRooms; i++)
        {
            for (int j = 0; j < numRooms; j++)
            {
                matrix[i, j] = 0;
            }
        }
    }

    void Update()
    {
        
    }
}
