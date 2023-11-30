using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaExamen : MonoBehaviour
{
    private CharacterController _controller;
    private float _horizontal;
    private float _vertical;
    private Transform _camera;

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
    public bool _isGrounded;

    //animaciones
    private Animator _anim;

    int _damage = 5;
    
    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Fire2"))
        {
            AimMovement();
        }else 
        {
            Movement();
        } 
        Jump();

        if(Input.GetKey(KeyCode.K))
        {
            RayCasting();
        }

        if(Input.GetKey(KeyCode.J))
        {
            DeathRay();
        }
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _anim.SetFloat("VelX", 0); //para desactivar las animaciones de izquierda/derecha
        _anim.SetFloat("VelZ", direction.magnitude); //magnitude devuelve el tamano del vector pero sin ninguna direccion   

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y; //_camera.eulerAngles.y devuele el eje de rotacion en angulos
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle,0) * Vector3.forward; //hace que la direccion apunte donde mira la camara

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }

        void AimMovement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _anim.SetFloat("VelX", _horizontal); //animaciones izquierda derecha
        _anim.SetFloat("VelZ", _vertical); //animaciones delante y atras

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y; //_camera.eulerAngles.y devuele el eje de rotacion en angulos
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if(direction != Vector3.zero)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle,0) * Vector3.forward; //hace que la direccion apunte donde mira la camara

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
        /*_isGrounded = Physics.Raycast(_sensorPosition.position, Vector3.down, _sensorRadius, _groundLayer); //Raycast devuelve una bool, por lo que se le puede asignar al _isGrounded
        Debug.DrawRay(transform.position, Vector3.down * _sensorRadius, Color.red); //Para dibujar el rayo*/

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void RayCasting() //Rayo hacia adelante
    {
        /*if(Physics.Raycast(transform.position, transform.forward, 10)) // hay que indicarle posicion, direccion y tamano
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, transform.forward * 10, Color.green); //Para dibujar el rayo si impacta
        }else

            {
                Debug.Log("No Hit");
                Debug.DrawRay(transform.position, transform.forward * 10, Color.red); //Para dibujar el rayo si no impacta
            }*/

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10)) //hit almacena la informacion del objeto con el que haya impactado
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.position);
            //Destroy(hit.transform.gameObject);
            hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

            /*Box caja = hit.transform.GetComponent<Box>();
            if(caja != null)
            {
                caja.TakeDamage(_damage);
            }*/
            
        }
    }

    void DeathRay() //Rayo hacia adelante
    {
        if(Physics.Raycast(transform.position, transform.forward, 10)) // hay que indicarle posicion, direccion y tamano
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, transform.forward * 10, Color.green); //Para dibujar el rayo si impacta
        }else

            {
                Debug.Log("No Hit");
                Debug.DrawRay(transform.position, transform.forward * 10, Color.red); //Para dibujar el rayo si no impacta
            }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10)) //hit almacena la informacion del objeto con el que haya impactado
        {
            Box caja = hit.transform.GetComponent<Box>();
            if(caja != null)
            {
                caja.TakeDamage(_damage);
            }
            
        }
    }
    
}
