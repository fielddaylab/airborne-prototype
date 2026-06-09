using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationCamera : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset;
    // higher values slower
    [SerializeField] private float m_Smoothing;

    private Vector3 _targetPosition;
    private Vector3 _smoothVelocity = Vector3.zero;

    public void UpdateRoom(InvestigationRoom room)
    {
        _targetPosition = new Vector3(room.transform.position.x, room.transform.position.y, 0);
    }

    public void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition + m_Offset, ref _smoothVelocity,  m_Smoothing);
    }
}
