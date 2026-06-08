using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed, m_gravity;
    [SerializeField] public CharacterController PlayerCharacterController;
    [SerializeField] private float m_TeleportCoolDown;

    [HideInInspector] public bool JustTeleported = false;
    private float _accumulatedTime = 0;
    public InferenceRoom currentRoom;
    public InferenceCamera playerCamera;

    private Vector2 _input;

    private Vector3 _velocity;
    private bool _grounded;
    
    void Start()
    {
        
    }
    
    //void Update()
    //{
    //    _input.x = Input.GetAxis("Horizontal");
    //    _input.y = Input.GetAxis("Vertical");
    //}

    private void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _grounded = PlayerCharacterController.isGrounded;

        if (_grounded && _velocity.y < -2f) _velocity.y = -2f;

        Vector3 move = new Vector3(_input.x, 0, _input.y);
        move = Vector3.ClampMagnitude(move, 1f);

        _velocity.y -= m_gravity * Time.deltaTime;

        Vector3 finalMovement = m_moveSpeed * move + Vector3.up * _velocity.y;
        PlayerCharacterController.Move(finalMovement * Time.deltaTime);

        if (JustTeleported)
        {
            _accumulatedTime += Time.deltaTime;
            if (_accumulatedTime >= m_TeleportCoolDown)
            {
                JustTeleported = false;
                _accumulatedTime = 0;
            }
        }
    }
}
