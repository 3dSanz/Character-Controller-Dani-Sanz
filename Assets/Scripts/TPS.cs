using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _anim;
    private Transform _camera;

    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _velocidad = 5;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField] private float _alturaSalto = 1;
    private float _gravedad = -9.81f;
    private Vector3 _jugadorGravedad;
    [SerializeField] private Transform _posicionSensor;
    [SerializeField] private float _radioSensor = 0.2f;
    [SerializeField] private LayerMask _layerSuelo;
    public bool _isGrounded;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
    }

    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetButton("Fire2"))
        {
            ApuntadoMovimiento();
        }else 
        {
            Movimiento();
        }
    
    }

    void Movimiento()
    {
        Vector3 _direccion = new Vector3 (_horizontal, 0, _vertical);

        _anim.SetFloat("VelX",0);
        _anim.SetFloat("VelZ", _direccion.magnitude);

        if(_direccion != Vector3.zero)
        {
            float _targetAngle = Mathf.Atan2(_direccion.x, _direccion.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float _smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0,_smoothAngle,0);
            Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
            _controller.Move(_moveDirection.normalized * _velocidad * Time.deltaTime);
        }
    }

    void ApuntadoMovimiento()
    {
        Vector3 _direccion = new Vector3 (_horizontal, 0, _vertical);

        _anim.SetFloat("VelX",_horizontal);
        _anim.SetFloat("VelZ", _vertical);
        float _targetAngle = Mathf.Atan2(_direccion.x, _direccion.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float _smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,  _camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0,_smoothAngle,0);

        if(_direccion != Vector3.zero)
        {
            Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
            _controller.Move(_moveDirection.normalized * _velocidad * Time.deltaTime);
        }
    }

    void Salto()
    {
        _isGrounded = Physics.CheckSphere(_posicionSensor.position, _radioSensor, _layerSuelo);

        if(_isGrounded && _jugadorGravedad.y < 0)
        {
            _jugadorGravedad.y = -2;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _jugadorGravedad.y = Mathf.Sqrt(_alturaSalto * -2 * _gravedad);
        }
        _jugadorGravedad.y += _gravedad * Time.deltaTime;
        _controller.Move(_jugadorGravedad * Time.deltaTime);
    }
    
}