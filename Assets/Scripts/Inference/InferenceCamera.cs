using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InferenceCamera : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset;
    [SerializeField] private float m_Smoothing;

    private Vector3 _targetPosition;

    public void UpdateRoom(InferenceRoom room)
    {
        _targetPosition = new Vector3(room.transform.position.x, room.transform.position.y, 0);
    }

    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition + m_Offset, m_Smoothing * Time.deltaTime);
    }
}
