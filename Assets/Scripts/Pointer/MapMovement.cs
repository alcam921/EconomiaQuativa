using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapMovement : MonoBehaviour
{
    public Vector2 point;

    private Rigidbody2D rb;
    public GameObject uiElement;

    public float speed = 8f;
    private bool isMoving = false;
    private bool startPosition = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (point != Vector2.zero)
        {
            isMoving = true;
            uiElement.SetActive(false);

        }
        if (isMoving || startPosition)
        {
            startPosition = false;
            Vector2 direction = point - rb.position;
            direction.Normalize();
            rb.velocity = direction * speed;
        }
        //Mecanica de Aproximção-
        if (Vector2.Distance(rb.position, point) < 0.3f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            uiElement.SetActive(true);
        }
    }

}
