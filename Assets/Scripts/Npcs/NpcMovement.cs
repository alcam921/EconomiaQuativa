using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    public List<Transform> path = new List<Transform>();
    private int _currentIndex = 0;

    private NavMeshAgent _agent;
    private SpriteRenderer _sprite;
    private Animator _anim;
    [SerializeField] private float _minRadios;
    [SerializeField] private float _maxRadios;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        if (path.Count > 0)
            _agent.SetDestination(path[_currentIndex].position);
    }

    public void SetPath(List<Transform> newPath)
    {
        path = newPath;
        _currentIndex = 0;
    }

    void Update()
    {
        if (path.Count == 0) return;
        if(_agent.velocity.magnitude >= 0.5f){
            _anim.SetBool("Walk", true);
        }else{
            _anim.SetBool("Walk", false);
        }
        Vector3 dir = _agent.desiredVelocity;
        if (dir.x != 0)
            _sprite.flipX = dir.x < 0;
        if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
        {
            _currentIndex++;

            if (_currentIndex < path.Count)
            {
                _agent.SetDestination(path[_currentIndex].position);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
    }
}
