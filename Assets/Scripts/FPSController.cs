using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    CharacterController _controller;

    Transform _fpsCamera;

    [SerializeField] float _speed = 10;

    [SerializeField] float _sensitivity = 350;

    float _XRotation = 0; 

        //variables para velocidad, salto y gravedad
    [SerializeField] private float _jumpHeight = 8;
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _playerGravity;

        //varibles para sensor
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _fpsCamera = Camera.main.transform;

    }


    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        //Rotacion camara
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensitivity * Time.deltaTime;

        _XRotation -= mouseY;
        _XRotation = Mathf.Clamp(_XRotation,-90,90);

        _fpsCamera.localRotation = Quaternion.Euler(_XRotation,0,0);

        transform.Rotate(Vector3.up * mouseX); //Vector3.up rotara sobre el eje Y

        //Movimiento personaje
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        _controller.Move(move.normalized * _speed * Time.deltaTime);

    }

    void Jump()
    {

        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_playerGravity * Time.deltaTime);
    }
}
