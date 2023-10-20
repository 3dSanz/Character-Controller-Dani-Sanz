using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShoulderController : MonoBehaviour
{
    private CharacterController _controller;
    private float _horizontal;
    private float _vertical;
    private Transform _camera;
    private Transform _lookAtTransform;

    //variables para velocidad, salto y gravedad
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    //variables para rotacion
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    //varibles para sensor
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    //variables para Cinemachine
    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;
    public GameObject _mainCamera;
    public GameObject _cameraAim;
    
    // Awake inicia antes que Start, sirve sobretodo para inicializar variables antes de que se empiece a ejecutar cualquier codigo
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _lookAtTransform = GameObject.Find("LookAt").transform;

        Movement();
        Jump();

        if(Input.GetButton("Fire2"))
        {
            _mainCamera.SetActive(false);
            _cameraAim.SetActive(true);
        }else 
        {
            _mainCamera.SetActive(true);
            _cameraAim.SetActive(false);
        } 
        Jump();
    }

    void Movement()
    {
        Vector3 move = new Vector3(_horizontal, 0, _vertical).normalized;

        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        //Rotacion de la camara en el eje x, tocando el eje y
        transform.rotation = Quaternion.Euler(0,xAxis.Value,0);

        //Rotacion de la camara en el eje y
        _lookAtTransform.eulerAngles = new Vector3(yAxis.Value,xAxis.Value,0);

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x,move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle,0) * Vector3.forward;
            _controller.Move(moveDirection * _playerSpeed * Time.deltaTime);
        }

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
