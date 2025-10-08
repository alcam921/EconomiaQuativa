using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextoEstacoes : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI texto;
    [SerializeField] TextMeshProUGUI textoBotao;
    [SerializeField] Transform trem, destino1, destino2, destino3, destino4, destino5;
    [SerializeField] Animator caixaTexto;
    [SerializeField] GameObject box;
    [SerializeField] Button botao;
    public string[] textos;
    public string[] cenas;
    // public string[] cenas;
    private int cena = 0;
    void Update()
    {
        PopOut();
    }

    private void PopOut()
    {
        switch (trem.position)
        {
            case  var value when value == destino1.position:
                
                //caixaTexto.SetBool("EmPosicao", true);
                botao.onClick.RemoveAllListeners();
                botao.onClick.AddListener(() => {
                    ChangeToOtherScene();
                });
                StartCoroutine(Esperar(0));
                cena = 0;
                break;
            case var value when value == destino2.position:
                //caixaTexto.SetBool("EmPosicao", true);
                botao.onClick.RemoveAllListeners();
                botao.onClick.AddListener(() => {
                    ChangeToOtherScene();
                });
                StartCoroutine(Esperar(1));
                cena = 1;
                break;
            case var value when value == destino3.position:
                //caixaTexto.SetBool("EmPosicao", true);
                botao.onClick.RemoveAllListeners();
                botao.onClick.AddListener(() => {
                    ChangeToOtherScene();
                });
                StartCoroutine(Esperar(2));
                cena = 2;
                break;
            case var value when value == destino4.position:
                //caixaTexto.SetBool("EmPosicao", true);
                botao.onClick.RemoveAllListeners();
                botao.onClick.AddListener(() => {
                    ChangeToOtherScene();
                });
                StartCoroutine(Esperar(3));
                cena = 3;
                break;
            case var value when value == destino5.position:
                //caixaTexto.SetBool("EmPosicao", true);
                botao.onClick.RemoveAllListeners();
                botao.onClick.AddListener(() => {
                    ChangeToOtherScene();
                });
                StartCoroutine(Esperar(4));
                cena = 4;
                // texto.text = "Destino5";
                break;
                default:
                //caixaTexto.SetBool("EmPosicao", false);
               StartCoroutine(Wait(0.5f));
                break;
        }
    }

     IEnumerator Esperar(int i)
    {
        yield return new WaitForSeconds(0.5f);
        box.SetActive(true);
        texto.text = textos[i];
        botao.image.color = new Color (1, 1, 1, 1);
        textoBotao.text = "Visitar";
    }
    IEnumerator Wait(float i)
    {
        yield return new WaitForSeconds(i);
        box.SetActive(false);
    }

    public void ChangeToOtherScene()
    {
        
        SceneManager.LoadScene(cenas[cena]);
    }
}
