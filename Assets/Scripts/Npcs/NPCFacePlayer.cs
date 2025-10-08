using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    public Transform player; // arraste o Player aqui no Inspector

    private Vector3 initialScale;
    [SerializeField] private GameObject _conversationObject;
    private bool _facePlayer = false;

    public bool barracaTutorial = false;
    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;
        if (!_facePlayer) return;

        // Se o player está à esquerda
        if (player.position.x < transform.position.x)
        {
            // Olha para a esquerda
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else
        {
            // Olha para a direita
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger area");
            _facePlayer = true;
            if(_conversationObject)
                _conversationObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_conversationObject)
                _conversationObject.SetActive(false);
            _facePlayer = false;
        }
    }
}
