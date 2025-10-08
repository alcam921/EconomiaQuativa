using System;
using System.Collections;
using UnityEngine;

public class BolaFutebol : MonoBehaviour
{
    public static event Action<int> OnScore;

    [SerializeField] private float _ballSpeed;
    [SerializeField] private float _decelerationForce;

    private Rigidbody2D _rb2d;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_rb2d.velocity.magnitude > 0.1f) {
            _rb2d.AddForce(-_rb2d.velocity.normalized * _decelerationForce);
        } else {
            _rb2d.velocity = Vector3.zero;
        }
    }

    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(2f);
        _rb2d.velocity = Vector3.zero;
        _rb2d.angularVelocity = 0f;
        
        transform.position = _initialPos;
        Debug.Log("posi��o reiniciada!");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player") {
            Vector2 direction = other.transform.position - transform.position;
            _rb2d.AddForce(direction * _ballSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Gol") {
            var pos = 0;
            if ((transform.position.x - _initialPos.x) < 0f) {
                pos = 1;
            } else {
                pos = 2;
            }

            OnScore?.Invoke(pos);
            StartCoroutine(ResetPos());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CampoDeFutebol") {
            StartCoroutine(ResetPos());
        }
    }
}
