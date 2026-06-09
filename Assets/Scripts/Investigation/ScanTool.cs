using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTool : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    void Update()
    {
        Vector3 planePosition = Input.mousePosition;
        float distanceFromCamera = transform.position.z - Camera.main.transform.position.z;
        planePosition.z = distanceFromCamera;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(planePosition);
        worldPosition.z = transform.position.z;

        Vector3 direction = worldPosition - transform.position;
        transform.right = direction;

        m_spriteRenderer.flipY = direction.x < 0;
    }   
}
