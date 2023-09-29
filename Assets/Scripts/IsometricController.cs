using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricController : MonoBehaviour
{
    private CharacterController _controller;

    //Variables para movimiento
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _playerSpeed = 5;

    //Variables para rotacion
    private float _turnSmoothVelocity;
    [SerializeField] private float _turnSmoothTime = 0.1f;

    //Variables para salto
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;
    
    //Variables para sensor de suelo
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius;
    [SerializeField] private LayerMask _groudLayer;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        Movement();
        Jump();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);
        
        /*Atan2 devuelve un angulo*/
        if(direction != Vector3.zero)
        {
            _controller.Move(direction.normalized*_playerSpeed * Time.deltaTime);
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0,smoothAngle,0);

        }

    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groudLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }
        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);
    }
}
