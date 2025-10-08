using UnityEngine;
using UnityEngine.UI;

public class EncontreGameManager : MonoBehaviour
{
    [SerializeField] GameObject Impostor;
    [SerializeField] Sprite[] Impostores;
    [SerializeField] int impostorIndex;

    [SerializeField] Transform Quati;
    public int ImpsQuantia = 20;
    public float limiteEsquerda = -35f;
    public float limiteDireita = 35f;
    public float limiteCima = 15f;
    public float limiteBaixo = -15f;
    private bool start = false;
    [SerializeField] public bool clickOutSide = true;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clickQuati;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Image background;
    private int cidadeIndex;

    [SerializeField] private GameObject tutorialHighlight; // Quadrado/seta para destacar o Quati
    [SerializeField] private GameObject tutorialDialogue; // Quadrado/seta para destacar o Quati
    private bool isTutorial = false;

    void Awake()
    {
        cidadeIndex = PlayerPrefs.GetInt("indexCidade");
        background.sprite = images[cidadeIndex];
        Posicionar();
    }
    void Start()
    {

        // Checa se é a primeira vez jogando este minigame
        if (!PlayerPrefs.HasKey("Tutorial_EncontreQuati"))
        {
            ScoreManager.checkTutorial?.Invoke(true);
            isTutorial = true;
            start = false; // impede o jogo de rodar
            ScoreManager.timerControl?.Invoke(); // pausa o timer
            tutorialHighlight.SetActive(true); // mostra destaque
            tutorialDialogue.SetActive(true);
            // tutorialHighlight.transform.position = new Vector2(Quati.position.x, Quati.position.y / 4);
        }
        else
        {
            tutorialDialogue.SetActive(false);
            tutorialHighlight.SetActive(false);
        }
    }
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Salva as mudanças
    }

    void Update()
    {
        if (start)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "miss")
                    {
                        ScoreManager.lessTimer?.Invoke();
                        audioSource.clip = clickQuati[1];
                        audioSource.Play();

                    }
                }

            }

        }
    }

    public void Posicionar()
    {
        if (start)
        {
            // audioSource.clip = clickQuati;
            clickOutSide = false;
            audioSource.clip = clickQuati[0];
            audioSource.Play();
            GameObject[] imps = GameObject.FindGameObjectsWithTag("Impostor"); //Lembre-se de colocar a tag no impostor

            for (int i = 0; i < imps.Length; i++)
            {
                Destroy(imps[i]);
            }
            // audioSource.Play();
        }
        Quati.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(limiteEsquerda, limiteDireita), Random.Range(limiteBaixo, limiteCima), 10));
        for (int i = 0; i < ImpsQuantia; i++)
        {
            Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(limiteEsquerda, limiteDireita), Random.Range(limiteBaixo, limiteCima), 10));
            GameObject impost = Instantiate(Impostor, point, Quaternion.identity);
            impost.GetComponent<SpriteRenderer>().sprite = Impostores[Random.Range(0, Impostores.Length)];
            if (Vector2.Distance(impost.transform.position, Quati.position) < 1)
            {

                for (int k = 0; k < 10; k++)
                {
                    Debug.Log(k);
                    impost.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(limiteEsquerda, limiteDireita), Random.Range(limiteBaixo, limiteCima), 10));
                    if (Vector2.Distance(impost.transform.position, Quati.position) > 1)
                    {
                        break;
                    }
                }

            }
        }
        start = true;
        clickOutSide = true;

    }
    public void CheckClick()
    {
        if (isTutorial)
        {
            // Primeira vez, inicia o jogo agora
            ScoreManager.checkTutorial?.Invoke(false);
            isTutorial = false;
            PlayerPrefs.SetInt("Tutorial_EncontreQuati", 1);
            tutorialHighlight.SetActive(false);
            tutorialDialogue.SetActive(false);
            ScoreManager.timerControl?.Invoke(); // resume o timer
            start = true;
            return; // não pontua ainda
        }
        if (ImpsQuantia < 50)
        {
            ImpsQuantia += 5;
        }
        clickOutSide = false;
        ScoreManager.pontuar?.Invoke();
        ScoreManager.moreTimer?.Invoke();
    }
}
