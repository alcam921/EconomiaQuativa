using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoveTrain : MonoBehaviour
{
    #region Inspector
    [Header("Configurações de Movimento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float delayBeforeReturn = 2f;

    [Header("Smoke")]
    [SerializeField] private GameObject smoke;
    [SerializeField] private Vector2 smokeOffsetLeft = new Vector2(-1f, 0.1f);
    [SerializeField] private Vector2 smokeOffsetRight = new Vector2(1f, 0.1f);

    [Header("Wagon")]
    [SerializeField] private GameObject wagon;
    [SerializeField] private Vector2 wagonOffsetLeft = new Vector2(-2f, 0f);
    [SerializeField] private Vector2 wagonOffsetRight = new Vector2(2f, 0f);

    [Header("Escalas Diferentes")]
    [SerializeField] private Vector3 frontScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 backScale = new Vector3(0.8f, 0.8f, 1f);

    [Header("Ordem das Layers")]
    [SerializeField] private int frontLayer = 0;
    [SerializeField] private int backLayer = -4;
    [Header("Altura do Trem")]
    [SerializeField] private float trainBaseY = 0f;        // altura inicial (definida no Start)
    [SerializeField] private float returnYOffset = 1f;     // quanto sobe na volta
    [Header("Npc's Images")]
    [SerializeField] private SpriteRenderer[] npcSprites;
    #endregion

    #region Private
    private Rigidbody2D _rb2d;
    private bool _movingRight = true;
    private bool _isWaiting = false;

    private float _screenLeft;
    private float _screenRight;
    private float _trainWidth;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        // Limites da tela
        _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        _screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        _trainWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        // float trainY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 12, 0)).y;
        // aqui usei 1/3 da tela, pode trocar para /2 (meio), ou /4 (mais baixo)

        // Posição inicial fora da tela à esquerda
        transform.position = new Vector2(_screenLeft - _trainWidth, trainBaseY);

        // Começa indo para a direita
        SetDirection(true);
    }

    private void Update()
    {
        if (_isWaiting) return;

        // Verifica se chegou no limite da tela
        if (_movingRight && transform.position.x >= _screenRight + _trainWidth + 10f)
        {
            StartCoroutine(SwitchDirection(false));
        }
        else if (!_movingRight && transform.position.x <= _screenLeft - _trainWidth)
        {
            StartCoroutine(SwitchDirection(true));
        }
    }
    #endregion

    #region Movement Logic
    private void SetDirection(bool moveRight)
    {
        _movingRight = moveRight;

        // Direção e velocidade
        _rb2d.velocity = (moveRight ? Vector2.right : Vector2.left) * speed;

        // Flip do trem e vagão
        GetComponent<SpriteRenderer>().flipX = !moveRight;
        wagon.GetComponent<SpriteRenderer>().flipX = !moveRight;

        foreach (SpriteRenderer npcSprite in npcSprites){
            npcSprite.enabled = moveRight;
        }

        // Posição do vagão
        wagon.transform.localPosition = moveRight ? wagonOffsetLeft : wagonOffsetRight;

        // Posição do smoke
        smoke.transform.localPosition = moveRight ? smokeOffsetLeft : smokeOffsetRight;
    }

    private IEnumerator SwitchDirection(bool moveRight)
    {
        _isWaiting = true;
        _rb2d.velocity = Vector2.zero;

        yield return new WaitForSeconds(delayBeforeReturn);

        // Alterna entre frente e trás (camada + escala)
        int sorting = moveRight ? frontLayer : backLayer;
        Vector3 scale = moveRight ? frontScale : backScale;

        GetComponent<SpriteRenderer>().sortingOrder = sorting;
        wagon.GetComponent<SpriteRenderer>().sortingOrder = sorting;
        smoke.GetComponent<Renderer>().sortingOrder = sorting;

        transform.localScale = scale;

        float newY = trainBaseY;
        if (!moveRight) // quando volta
            newY += returnYOffset;

        transform.position = new Vector2(
            moveRight ? _screenLeft - _trainWidth : _screenRight + _trainWidth,
            newY
        );

        SetDirection(moveRight);

        _isWaiting = false;
    }
    #endregion
}
