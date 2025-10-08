using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TremMovement : MonoBehaviour
{
    private Transform alvoAtual;
    public float velocidade = 5f; // Velocidade de movimento
    private bool moverParaAlvo = false;
    [SerializeField] Transform tunel;
    [SerializeField] Transform startTunel;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float leftSmokeLimit;
    public Animator tremAnim;
    public Animator wagonAnim;
    
    public GameObject cam;
    
    public void DefDestination(Transform newDestination)
    {
        if (moverParaAlvo == false)
        {
            alvoAtual = newDestination;
            moverParaAlvo = true;
        }
    }

    private void Update()
    {
            if (moverParaAlvo == true && alvoAtual != null)
            {
                tremAnim.enabled = true;
                wagonAnim.enabled = true;
                if (gameObject.transform.position.x <= startTunel.position.x)
                {
                    smoke.enableEmission = false;
                }
                else
                {
                    smoke.enableEmission = true;

                }
                if (gameObject.transform.position.x < alvoAtual.transform.position.x)
                {
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, alvoAtual.transform.position, velocidade * Time.deltaTime);
                }
                else if (gameObject.transform.position.x > alvoAtual.transform.position.x)
                {
                    gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, tunel.position, velocidade * Time.deltaTime);
                    if (gameObject.transform.position.x + leftSmokeLimit >= tunel.position.x)
                    {
                        smoke.enableEmission = false;
                    }

                    if (gameObject.transform.position == tunel.position)
                    {
                        gameObject.transform.position = new Vector3(-26f, -2.2f, -10f);
                        cam.transform.position = new Vector3(-26f, -2.2f, -10f);
                    }
                }
                else
                {
                    tremAnim.enabled = false;
                    wagonAnim.enabled = false;
                    smoke.enableEmission = false;
                    // GameObject.FindGameObjectWithTag("GameController").GetComponent<CityTabManager>().TabOpenClose(false);
                    moverParaAlvo = false;
                }
            }
        
    }

}
