using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pecas : MonoBehaviour
{
    private Rigidbody2D rb;
    public delegate void DragEndedDelegate(Transform transform);
    public DragEndedDelegate dragEndedDelegate;
    bool arrastando;
    bool overlap;
    private Vector2 peca;

   
    private void Awake()
    {     
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    { bool pecasNoLugar = false;
        while (!pecasNoLugar) {         
            peca = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-3.5f, 3.5f));
            if (peca.x > -3.5f && peca.x < 3.5f && peca.y > -2.5f && peca.y < 2.5f)
            {
                continue;
            } else
            {
                gameObject.transform.position = peca;
                pecasNoLugar = true;
            }
          }
    }
    private void Update()
    {
        
    }
    private void OnMouseDown()
    {
        arrastando = true;
    }
    void OnMouseDrag()
    {
        arrastando = true;
        rb.velocity = Vector3.zero;
        transform.position = PegarPosMouse();
    }
    Vector3 PegarPosMouse()
    {
        var posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posMouse.z = 0;
        return posMouse;
    }


    private void OnMouseUp()
    {
        arrastando = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PecaC")
        {
            overlap = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        overlap = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (arrastando == false && overlap == false)
        {
            dragEndedDelegate(this.transform);
        }
    }
}
