using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerInfos")]
    [SerializeField] private float speed = 4;
    public delegate void IsOn(bool value);
    public static IsOn On;
    public GameObject button, joystick;
    private Rigidbody2D _rb2d;
    private Vector2 _myInput;
    private Animator anim;
    private bool _isMoving = false;
    private bool keyboardOn;
    private PlayerInput _playerInput;
    public bool canMove;
    public List<Transform> pontosDestino; // Lista de pontos para seguir

    [Header("Fase")]
    [SerializeField] Camera _camera;
    [SerializeField] Transform pontoDeFoco;
    public VisitFaseMovement visitFase;
    private bool _canOpenPopup = true;
    private float originalSize;
    private float zoom;
    public float zoomAmount = 5;
    private float camVelocity = 0;
    private Vector3 vetorZero = Vector3.zero;
    private bool temAlvo = false;
    private bool isWaitingForTrigger = false;

    [SerializeField] private Transform pontoDestino;
    private bool moverParaDestino = false;

    [Header("Objetos de Ativa��o")]
    [SerializeField] private GameObject collidersMapa; // Novo campo para ativa��o

    private void OnEnable()
    {
        On = MoveOnOFf;
    }
    private void OnDisable()
    {
        On = null;
    }
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        originalSize = _camera.orthographicSize;
        zoom = _camera.orthographicSize;
        pontoDeFoco.position = gameObject.transform.position;
    }
    private void Update()
    {
        if (!Application.isMobilePlatform && keyboardOn == false && canMove)
        {
            _playerInput.SwitchCurrentActionMap("Keyboard");
            keyboardOn = true;
            JoystickOverseer.disabled?.Invoke();
        }
        if (Application.isMobilePlatform && keyboardOn == true && canMove)
        {

            _playerInput.SwitchCurrentActionMap("Player");
            keyboardOn = false;
            JoystickOverseer.enabledd?.Invoke();
        }
    }
    private void Start()
    {
        if (joystick == null)
        {
            joystick = GameObject.FindGameObjectWithTag("Joystick");
        }
    }
    void FixedUpdate()
    {
       
        // S� controla anima��o pelo input se n�o estiver indo para o destino
        if (moverParaDestino && pontosDestino.Count > 0)
        {
            Transform destinoAtual = pontosDestino[0];
            Vector2 posAtual = transform.position;
            Vector2 posDestino = destinoAtual.position;
            Vector2 direcao = (posDestino - posAtual).normalized;
            float distancia = Vector2.Distance(posAtual, posDestino);
         
            // Flip do sprite
            if (direcao.x > 0)
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if (direcao.x < 0)
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            if (distancia > 0.1f)
            {
                _rb2d.velocity = direcao * speed;
                anim.SetBool("WalkHorizontal", true);
            }
            else
            {
                _rb2d.velocity = Vector2.zero;
                anim.SetBool("WalkHorizontal", false);

                // Remove ponto atual e vai pro próximo
                pontosDestino.RemoveAt(0);

                // Se acabou a lista, para
                if (pontosDestino.Count == 0)
                {
                    moverParaDestino = false;

                    if (collidersMapa != null)
                        collidersMapa.SetActive(true);
                }
            }
        }
        else
        {
            AnimController();

            if (_myInput != Vector2.zero)
            {
                if (_myInput.x > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (_myInput.x < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                _rb2d.velocity = new Vector2(_myInput.x * speed, _myInput.y * speed);
            }
            else
            {
                _rb2d.velocity = Vector2.zero;
            }
        }

        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, zoom, ref camVelocity, 0.25f);
        if (temAlvo == false)
        {
            pontoDeFoco.transform.position = Vector3.SmoothDamp(pontoDeFoco.transform.position, gameObject.transform.position, ref vetorZero, 0.25f);
        }
    }

    public void MoverAteDestino()
    {
        moverParaDestino = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("zoom"))
        {
            zoom = zoomAmount;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("zoom"))
        {
            temAlvo = true;
            pontoDeFoco.transform.position = Vector3.SmoothDamp(pontoDeFoco.transform.position, other.transform.position, ref vetorZero, 0.25f);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("zoom"))
        {
            zoom = originalSize;
            temAlvo = false;
        }
    }
    public void MoverPersonagem(InputAction.CallbackContext value)
    {
        if (canMove)
        {
            _myInput = value.ReadValue<Vector2>();
        }
    }

    private void AnimController()
    {
        if (_myInput != Vector2.zero)
        {
            
            
                anim.SetBool("WalkHorizontal", true);
            
        }
        else
        {
            anim.SetBool("WalkHorizontal", false);
            if (!isWaitingForTrigger)
            {
                StartCoroutine(RandomTriggerCoroutine());
            }
        }
    }

    private IEnumerator RandomTriggerCoroutine()
    {
        isWaitingForTrigger = true;

        while (_myInput == Vector2.zero)
        {
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

            string[] triggers = { "Idle2" };
            string randomTrigger = triggers[Random.Range(0, triggers.Length)];

            anim.SetTrigger(randomTrigger);
        }

        isWaitingForTrigger = false;
    }
    public void MoveOnOFf(bool b)
    {
        canMove = b;
        if (canMove == false)
        {
            _myInput = Vector2.zero;
            _rb2d.velocity = Vector2.zero;
        }
    }
}
