using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoverTrem : MonoBehaviour
{
    private Transform alvoAtual;
    public float velocidade = 5f; // Velocidade de movimento
    private bool moverParaAlvo = false;
    [SerializeField] Transform tunel;
    [SerializeField] Transform roda1;
    [SerializeField] float rotation;

    public GameObject cam;

    public void DefinirDestino(Transform novoAlvo)
    {
        if (moverParaAlvo == false)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<CityTabManager>().TabOpenClose(true);
            alvoAtual = novoAlvo;
            moverParaAlvo = true;
        }else{
            rotation++;
            roda1.eulerAngles = new Vector3(roda1.eulerAngles.x, roda1.eulerAngles.y,rotation);
        }
    }

    private void Update()
    {
        
            if (moverParaAlvo == true && alvoAtual != null)
            {
                if (gameObject.transform.position.x < alvoAtual.transform.position.x)
                {
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, alvoAtual.transform.position, velocidade * Time.deltaTime);
                }
                else if (gameObject.transform.position.x > alvoAtual.transform.position.x)
                {
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, tunel.position, velocidade * Time.deltaTime);
                    if (gameObject.transform.position == tunel.position)
                    {
                        gameObject.transform.position = new Vector3(-26f, -2.2f, -10f);
                        cam.transform.position = new Vector3(-26f, -2.2f, -10f);
                    }
                }
                else
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<CityTabManager>().TabOpenClose(false);
                    moverParaAlvo = false;
                }
            }
        }
    

}
