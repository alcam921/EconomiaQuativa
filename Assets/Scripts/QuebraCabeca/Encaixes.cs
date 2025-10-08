using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encaixes : MonoBehaviour
{
    public List<Transform> pontosEncaixe;
    public List<Pecas> scriptArrasta;
    public float distanciaEncaixe = 0.5f;
    private Vector3 mudancaTamanho;
    private void Awake()
    {

        mudancaTamanho = new Vector3(0, -0.7f, 0);
    }
    private void Start()
    {

        foreach (Pecas script in scriptArrasta)
        {
            script.dragEndedDelegate = EncaixarObjeto;
        }
    }
    public void EncaixarObjeto(Transform obj)
    {
        foreach (Transform pontos in pontosEncaixe)
        {
            if (Vector2.Distance(pontos.position, obj.position) <= distanciaEncaixe)
            {
                obj.GetComponent<Rigidbody2D>().gravityScale = 0;
                obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                obj.position = pontos.position;
                return;
            }
        }


    }

}
