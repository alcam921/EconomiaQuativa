using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FollowingTrainCamera : MonoBehaviour
{
    [SerializeField] private Transform trem;
    public Vector3 offset = new Vector3(3f, 2.7f, -10f);
    private float tempoAjuste = 0.1f;
    private Vector3 velocidade = Vector3.zero;
    public float limiteDireita, limiteEsquerda, limiteCima, limiteBaixo;
    public bool ignoreLimits = false; // usado em destinos especiais

    void Update()
    {
        Seguindo();
    }

    void Seguindo()
    {

        Vector3 posicaoTrem = trem.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, posicaoTrem, ref velocidade, tempoAjuste);

        if (!ignoreLimits)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, limiteEsquerda, limiteDireita),
                Mathf.Clamp(transform.position.y, limiteBaixo, limiteCima),
                transform.position.z);
        }
    }
}
