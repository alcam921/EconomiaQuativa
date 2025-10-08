using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuinzeGameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform pecaPrefab;
    [SerializeField] private Transform[] pecasPrefabs;

    public List<Transform> pecas;
    private int localVazio;
    private int tamanho;
    private bool embaralhando = false;
    private bool primeiraVez = true;

    //variaveis de Audio Effect
    [SerializeField] private AudioSource audioEffect;
    [SerializeField] private AudioClip[] soundEffect;
    [SerializeField] private ScoreManager _score;
    [SerializeField] private GameObject pause;
    private int cidadeIndex;

    [SerializeField] private GameObject tutorialPanel; // sua caixa de texto/tutorial
    private bool isTutorial = false;


    // Start is called before the first frame update

    private void CriarPecasJogo(float larguraVao)
    {
        float largura = 1 / (float)tamanho;
        for (int linha = 0; linha < tamanho; linha++)
        {
            for (int coluna = 0; coluna < tamanho; coluna++)
            {
                Transform peca = Instantiate(pecaPrefab, gameTransform);
                pecas.Add(peca);

                peca.localPosition = new Vector3(-1 + (2 * largura * coluna) + largura, +1 - (2 * largura * linha) - largura, 0);
                peca.localScale = ((2 * largura) - larguraVao) * Vector3.one;
                peca.name = $"{(linha * tamanho) + coluna}";

                if ((linha == tamanho - 1) && (coluna == tamanho - 1))
                {
                    localVazio = (tamanho * tamanho) - 1;
                    peca.gameObject.SetActive(false);
                }
                else
                {
                    float vao = larguraVao / 2;
                    Mesh mesh = peca.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((largura * coluna) + vao, 1 - ((largura * (linha + 1)) - vao));
                    uv[1] = new Vector2((largura * (coluna + 1)) - vao, 1 - ((largura * (linha + 1)) - vao));
                    uv[2] = new Vector2((largura * coluna) + vao, 1 - ((largura * linha) + vao));
                    uv[3] = new Vector2((largura * (coluna + 1)) - vao, 1 - ((largura * linha) + vao));
                    mesh.uv = uv;
                }
            }
        }
    }
    void Start()
    {
        cidadeIndex = PlayerPrefs.GetInt("indexCidade");
        pecaPrefab = pecasPrefabs[cidadeIndex];
        pecas = new List<Transform>();
        tamanho = 3;
        CriarPecasJogo(0.01f);

        if (!PlayerPrefs.HasKey("Tutorial_Quinze"))
        {
            ScoreManager.checkTutorial?.Invoke(true);
            isTutorial = true;
            ScoreManager.timerControl?.Invoke();
            tutorialPanel.SetActive(true);

        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
            if (!embaralhando && VerificarCompleto())
            {
                embaralhando = true;
                ScoreManager.timerControl?.Invoke();
                StartCoroutine(EsperarEmbaralhar(2));
            }
            if (Input.GetMouseButtonDown(0) && embaralhando == false)
            {
                RaycastHit2D acerto = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (acerto)
                {
                    if (isTutorial)
                    {
                    ScoreManager.checkTutorial?.Invoke(false);
                    isTutorial = false;
                        PlayerPrefs.SetInt("Tutorial_Quinze", 1);
                        ScoreManager.timerControl?.Invoke();
                        tutorialPanel.SetActive(false);

                    }
                    primeiraVez = false;
                    for (int i = 0; i < pecas.Count; i++)
                    {
                        if (pecas[i] == acerto.transform)
                        {
                            if (TrocarSeValido(i, -tamanho, tamanho)) { break; }
                            if (TrocarSeValido(i, +tamanho, tamanho)) { break; }
                            if (TrocarSeValido(i, -1, 0)) { break; }
                            if (TrocarSeValido(i, +1, tamanho - 1)) { break; }
                            // Debug.Log("Nao mova a peca");
                        }
                    }
                }
            }
        
    }

    private bool TrocarSeValido(int i, int offset, int colCheck)
    {
        audioEffect.clip = soundEffect[0];
        if (((i % tamanho) != colCheck) && ((i + offset) == localVazio))
        {
            (pecas[i], pecas[i + offset]) = (pecas[i + offset], pecas[i]);

            (pecas[i].localPosition, pecas[i + offset].localPosition) = ((pecas[i + offset].localPosition, pecas[i].localPosition));

            localVazio = i;
            audioEffect.Play();
            return true;
        }
        // Debug.Log("Mova a peca");
        return false;
    }
    private bool VerificarCompleto()
    {
        for (int i = 0; i < pecas.Count; i++)
        {
            if (pecas[i].name != $"{i}")
            {
                return false;
            }
        }
        // Debug.Log("Completo");
        return true;
    }
    private IEnumerator EsperarEmbaralhar(float duracao)
    {
        pause.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(duracao);
        Embaralhar();
        embaralhando = false;
        ScoreManager.timerControl?.Invoke();
        pause.GetComponent<Button>().interactable = true;
    }

    private void Embaralhar()
    {
        int conta = 0;
        int ultimo = 0;
        while (conta < (tamanho * tamanho * tamanho))
        {
            int aleat = Random.Range(0, tamanho * tamanho);
            if (aleat == ultimo) { continue; }
            ultimo = localVazio;
            if (TrocarSeValido(aleat, -tamanho, tamanho))
            {
                conta++;
            }
            else if (TrocarSeValido(aleat, +tamanho, tamanho))
            {
                conta++;
            }
            if (TrocarSeValido(aleat, -1, 0))
            {
                conta++;
            }
            if (TrocarSeValido(aleat, +1, tamanho - 1))
            {
                conta++;
            }
        }
        if (primeiraVez == false)
        {
            // Debug.Log(primeiraVez);
            audioEffect.clip = soundEffect[1];
            audioEffect.Play();
            
            ScoreManager.pontuar?.Invoke();
            ScoreManager.moreTimer?.Invoke();
            pecasPrefabs[cidadeIndex] = null;
            Debug.Log(cidadeIndex + pecasPrefabs.Length);
            if (cidadeIndex + 1 == pecasPrefabs.Length)
            {
                
                cidadeIndex = 0;
            }
            else
            {
                cidadeIndex++;
            }
            if (pecasPrefabs[cidadeIndex] == null)
            {
                ScoreManager.finais.Invoke();
            } else
            {
                pecaPrefab = pecasPrefabs[cidadeIndex];
                

                    pecas.Clear();
                for (int i = 0; i < gameTransform.transform.childCount; i++)
                {
                    Destroy(gameTransform.transform.GetChild(i).gameObject);
                }
                primeiraVez = true;
                CriarPecasJogo(0.01f);
            }
        }
    }
}
