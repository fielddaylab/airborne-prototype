using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed, m_gravity;
    [SerializeField] private CharacterController m_characterController;

    private Vector2 _input;

    private Vector3 _velocity;
    private bool _grounded;

    
    void Start()
    {
        
    }
    
    void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _grounded = m_characterController.isGrounded;

        if (_grounded && _velocity.y < -2f) _velocity.y = -2f;

        Vector3 move = new Vector3(_input.x, 0, _input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        _velocity.y -= m_gravity * Time.fixedDeltaTime;

        Vector3 finalMovement = m_moveSpeed * move + Vector3.up * _velocity.y;
        m_characterController.Move(finalMovement * Time.fixedDeltaTime);
    }
}
