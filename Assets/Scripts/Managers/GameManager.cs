using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configura��es do Jogo")]
    public List<Sprite> imagensCartas;
    public GameObject cartaPrefab;
    public Transform areaDeJogo;

    // Ajuste o grid para 3x4 e defina espa�amento padr�o
    public int linhas = 3;
    public int colunas = 4;
    public float espacoEntreCartasX = 3.0f;  // Espa�amento padr�o
    public float espacoEntreCartasY = 3.0f;  // Espa�amento padr�o
    public float escalaCarta = 1.0f;  // Escala ajust�vel das cartas no Inspector

    public float tempoExibicaoInicial = 2f;

    private Carta primeiraCartaSelecionada;
    private Carta segundaCartaSelecionada;
    private bool podeSelecionar = false;
    private List<Carta> cartasNoJogo = new List<Carta>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GerarCartas();
        StartCoroutine(MostrarTodasCartasNoInicio());
    }

    private void GerarCartas()
    {
        // Duplicar imagens para fazer pares
        List<Sprite> imagensDuplicadas = new List<Sprite>();
        foreach (var imagem in imagensCartas)
        {
            imagensDuplicadas.Add(imagem);
            imagensDuplicadas.Add(imagem);  // Duplicando cada imagem para formar pares
        }

        // Embaralhar a lista de imagens
        imagensDuplicadas = Embaralhar(imagensDuplicadas);

        // Calcula a posi��o inicial para centralizar as cartas na �rea de jogo
        float posicaoInicialX = -((colunas - 1) * espacoEntreCartasX) / 2;
        float posicaoInicialY = ((linhas - 1) * espacoEntreCartasY) / 2;

        int index = 0;
        for (int i = 0; i < linhas; i++)
        {
            for (int j = 0; j < colunas; j++)
            {
                if (index < imagensDuplicadas.Count)
                {
                    // Calcula a posi��o da carta com base no �ndice de linha e coluna
                    Vector3 posicaoCarta = new Vector3(
                        posicaoInicialX + j * espacoEntreCartasX,
                        posicaoInicialY - i * espacoEntreCartasY,
                        0
                    );

                    // Instancia a carta e posiciona na posi��o calculada
                    GameObject novaCarta = Instantiate(cartaPrefab, areaDeJogo);
                    novaCarta.transform.localPosition = posicaoCarta;

                    // Define a imagem da frente da carta
                    Carta cartaScript = novaCarta.GetComponent<Carta>();
                    cartaScript.imagemFrente = imagensDuplicadas[index];
                    cartasNoJogo.Add(cartaScript);
                    index++;
                }
            }
        }

        AjustarEscalaDasCartas();
    }

    private void AjustarEscalaDasCartas()
    {
        // Calcula a escala baseada no tamanho do grid e no tamanho da �rea de exibi��o
        float larguraGrid = (colunas - 1) * espacoEntreCartasX;
        float alturaGrid = (linhas - 1) * espacoEntreCartasY;

        // Define uma escala m�nima para as cartas para caberem na tela
        float escalaX = areaDeJogo.localScale.x / (larguraGrid + espacoEntreCartasX);
        float escalaY = areaDeJogo.localScale.y / (alturaGrid + espacoEntreCartasY);
        float escalaFinal = Mathf.Min(escalaX, escalaY, 1f) * escalaCarta;  // Multiplica pela escala ajust�vel

        foreach (var carta in cartasNoJogo)
        {
            carta.transform.localScale = new Vector3(escalaFinal, escalaFinal, 1);
        }
    }

    private IEnumerator MostrarTodasCartasNoInicio()
    {
        foreach (var carta in cartasNoJogo)
        {
            carta.MostrarFrente();
        }

        yield return new WaitForSeconds(tempoExibicaoInicial);

        StartCoroutine(VirarTodasCartasGradualmente());
    }

    private IEnumerator VirarTodasCartasGradualmente()
    {
        podeSelecionar = false;

        for (int i = 0; i < cartasNoJogo.Count; i++)
        {
            StartCoroutine(cartasNoJogo[i].VirarCarta(0.2f));
            yield return new WaitForSeconds(0.1f);
        }

        podeSelecionar = true;
    }

    public void CartaSelecionada(Carta carta)
    {
        if (!podeSelecionar || carta == primeiraCartaSelecionada) return;

        if (primeiraCartaSelecionada == null)
        {
            primeiraCartaSelecionada = carta;
        }
        else
        {
            segundaCartaSelecionada = carta;
            podeSelecionar = false;
            StartCoroutine(VerificarPar());
        }
    }

    private IEnumerator VerificarPar()
    {
        yield return new WaitForSeconds(1f);

        if (primeiraCartaSelecionada.imagemFrente == segundaCartaSelecionada.imagemFrente)
        {
            primeiraCartaSelecionada = null;
            segundaCartaSelecionada = null;
        }
        else
        {
            primeiraCartaSelecionada.MostrarVerso();
            segundaCartaSelecionada.MostrarVerso();
            primeiraCartaSelecionada = null;
            segundaCartaSelecionada = null;
        }

        podeSelecionar = true;
    }

    private List<Sprite> Embaralhar(List<Sprite> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            Sprite temp = lista[i];
            int randomIndex = Random.Range(i, lista.Count);
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
        return lista;
    }

    private List<Vector3> EmbaralharPosicoes(List<Vector3> posicoes)
    {
        for (int i = 0; i < posicoes.Count; i++)
        {
            Vector3 temp = posicoes[i];
            int randomIndex = Random.Range(i, posicoes.Count);
            posicoes[i] = posicoes[randomIndex];
            posicoes[randomIndex] = temp;
        }
        return posicoes;
    }
}
