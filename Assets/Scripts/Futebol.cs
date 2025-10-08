using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Futebol : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject _bolaPrefab;
    [SerializeField] private Transform _bolaSpawn;

    [Header("Particulas")]
    [SerializeField] private ParticleSystem _particleE;
    [SerializeField] private ParticleSystem _particleD;

    void Start()
    {
        BallSpawn();
    }

    void OnEnable()
    {
        BolaFutebol.OnScore += Confetti;
    }

    void OnDisable()
    {
        BolaFutebol.OnScore -= Confetti;
    }

    private void BallSpawn()
    {
        if (_bolaSpawn != null) {
            Instantiate(_bolaPrefab, _bolaSpawn);
        }
    }

    void Confetti(int a)
    {
        if (a == 1) { 
            _particleE.Play();
        } else {
            _particleD.Play();
        }

        Debug.Log("gol!");
    }
}
