using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MemoryGameCardsByCity
{
    public Sprite[] figures;
}


public class MemoryGameController : MonoBehaviour
{
    public static MemoryGameController instance;

    [SerializeField] private Sprite verso;
    [SerializeField] private MemoryGameCardsByCity[] figuras;
    [SerializeField] private GameObject cartaPrefab;
    [SerializeField] private Transform gridPai;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundEffect;
    [SerializeField] private AudioClip flipCard;
    [SerializeField] private AudioClip createCard;
    [SerializeField] private string _reloadScene;
    [SerializeField] private Image bg;
    [SerializeField] private Sprite[] bgList;
    [SerializeField] private int cidadeIndex;
    [SerializeField] private string[] _cityKeys = { "CampoGrande", "Dourados", "Bonito", "TresLagoas", "Corumba" };

    private List<UICard> cartas = new List<UICard>();
    private List<Sprite> figurasList = new List<Sprite>();

    private UICard palpite1, palpite2;
    private int acertos;
    public bool podeSelecionar = true;

    private int rodadaAtual = 1;
    [SerializeField] private int maxRodadas = 5;
    [SerializeField] private GameObject tutorialPanel; // sua caixa de texto/tutorial
    private bool isTutorial = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("indexCidade"))
            cidadeIndex = PlayerPrefs.GetInt("indexCidade");
        else cidadeIndex = System.Array.IndexOf(_cityKeys, PlayerPrefs.GetString("lastScene"));
        bg.sprite = bgList[cidadeIndex];
        // figuras = Resources.LoadAll<Sprite>("Sprites");
        for (int a = 0; a < gridPai.GetComponentsInChildren<UICard>().Length; a++)
        {
            // Debug.Log("sdf");
            Destroy(gridPai.GetComponentsInChildren<UICard>()[a].gameObject);
        }
        CreateCards();

        if (!PlayerPrefs.HasKey("Tutorial_Memory"))
        {
            ScoreManager.checkTutorial?.Invoke(true);
            isTutorial = true;
            ScoreManager.timerControl?.Invoke();
            tutorialPanel.SetActive(true);
            podeSelecionar = true; // impede clique
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    void OnEnable()
    {
        UICard.cardFlip += FlipSound;
    }
    void OnDisable()
    {
        UICard.cardFlip -= FlipSound;
    }

    void FlipSound()
    {
        audioSource.clip = flipCard;
        audioSource.Play();
    }

    public void CreateCards()
    {
        List<Sprite> duplicadas = new List<Sprite>();
        for (int i = 0; i < figuras.Length && duplicadas.Count < 10; i++) // 6 pares
        {
            duplicadas.Add(figuras[cidadeIndex].figures[i]);
            duplicadas.Add(figuras[cidadeIndex].figures[i]); // duplica
        }

        figurasList = Embaralhar(duplicadas);

        audioSource.clip = createCard;
        audioSource.Play();

        for (int i = 0; i < figurasList.Count; i++)
        {
            GameObject obj = Instantiate(cartaPrefab, gridPai);
            obj.GetComponent<Image>().sprite = verso;
            UICard carta = obj.GetComponent<UICard>();
            carta.frente = figurasList[i];
            carta.verso = verso;
            carta.id = i;
            cartas.Add(carta);
        }
    }

    public void CartaSelecionada(UICard carta)
    {
        if (isTutorial)
        {
            ScoreManager.checkTutorial?.Invoke(false);
            isTutorial = false;
            PlayerPrefs.SetInt("Tutorial_Memory", 1);
            ScoreManager.timerControl?.Invoke();
            tutorialPanel.SetActive(false);

        }

        if (palpite1 == null)
        {
            palpite1 = carta;
            podeSelecionar = true;
        }
        else if (palpite2 == null && carta != palpite1)
        {
            palpite2 = carta;
            StartCoroutine(VerificarMatch());
        }
    }

    IEnumerator VerificarMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (palpite1.frente.name == palpite2.frente.name)
        {
            ScoreManager.pontuar?.Invoke();
            audioSource.clip = soundEffect[1]; // certo
            acertos++;
            palpite1.GetComponent<Button>().interactable = false;
            palpite2.GetComponent<Button>().interactable = false;
        }
        else
        {
            audioSource.clip = soundEffect[2]; // errado
            yield return new WaitForSeconds(0.3f);
            // palpite1.MostrarVerso();
            // palpite2.MostrarVerso();
            if (palpite1 != null)
            {
                StartCoroutine(palpite1?.AnimarVirarVerso());
            }
            if (palpite2 != null)
            {
                StartCoroutine(palpite2?.AnimarVirarVerso());
            }
        }

        audioSource.Play();
        palpite1 = palpite2 = null;
        podeSelecionar = true;

        if (acertos == figurasList.Count / 2)
        {
            rodadaAtual++;
            ScoreManager.pontuar?.Invoke();
            if (rodadaAtual > maxRodadas)
            {
                SceneManager.LoadScene(_reloadScene);
            }
            else
            {
                ScoreManager.timerControl?.Invoke();
                StartCoroutine(ReembaralharCartas());

                ScoreManager.moreTimer?.Invoke();
            }
        }
    }

    List<Sprite> Embaralhar(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int rand = Random.Range(0, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
        return list;
    }

    IEnumerator ReembaralharCartas()
    {
        podeSelecionar = false;

        // Virar todas as cartas com animação
        foreach (var carta in cartas)
        {
            if (carta != null)
            {
                yield return StartCoroutine(carta.AnimarVirarVerso());
            }
        }

        yield return new WaitForSeconds(0.5f);
        for (int a = 0; a < gridPai.GetComponentsInChildren<UICard>().Length; a++)
        {
            // Debug.Log("sdf");
            Destroy(gridPai.GetComponentsInChildren<UICard>()[a].gameObject);
        }

        CreateCards();
        // yield return new WaitForSeconds(0.05f);

        palpite1 = null;
        palpite2 = null;
        acertos = 0;
        podeSelecionar = true;
        ScoreManager.timerControl?.Invoke();
    }

}
