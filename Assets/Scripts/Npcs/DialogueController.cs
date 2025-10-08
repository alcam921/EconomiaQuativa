using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance = null;

    [Header("UI")]
    public GameObject dialoguePanelObject;
    public RectTransform dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text characterNameText;
    public Image characterIcon;
    public Button advanceButton;
    public Button backButton;
    public Button skipButton;

    [Header("Slide Config")]
    public Vector2 shownPosition;
    public Vector2 hiddenPosition;

    [Header("Typing Settings")]
    public float typingSpeed = 0.02f;

    [Header("Visit Point Infos")]
    [SerializeField] private GameObject _pointObejct;
    [SerializeField] private Image _pointImage;
    [SerializeField] private TMP_Text _pointName;
    [SerializeField] private Button _pointButton;

    [Header("Painel Tutorial")]
    [SerializeField] private bool _isTutorial = false;
    [SerializeField] private GameObject painelTutorial;
    [SerializeField] private GameObject painelBack;
    [SerializeField] private GameObject transition;
    [Header("Celular")]
    [SerializeField] private GameObject celular;
    [Header("Movimento")]
    [SerializeField] private GameObject btn_moviment;

    private List<DialogueLines> linesList = new();
    private DialogueSimpleTrigger currentTrigger;
    private int currentIndex = 0;
    private HashSet<int> linesRead = new();

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullText;

    private bool _showingPoint = false;

    private Vector3 _iconAdjust;

    private bool painelAtivadoNoPrimeiroDialogo = false;

    [SerializeField] private NPCFacePlayer npcTutorial;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        dialoguePanelObject.SetActive(false);

        advanceButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();
        _pointButton.onClick.RemoveAllListeners();

        advanceButton.onClick.AddListener(OnAdvancePressed);
        backButton.onClick.AddListener(OnBackPressed);
        skipButton.onClick.AddListener(OnSkipPressed);
        _pointButton.onClick.AddListener(LoadSceneFromPoint);
    }

    public void StartDialogue(DialogueData data, DialogueSimpleTrigger trigger)
    {
        currentTrigger = trigger;
        linesList.Clear();
        linesRead.Clear();

        linesList.AddRange(data.lines);
        currentIndex = 0;
        _iconAdjust = data.adjustCharacterIcon;

        StartCoroutine(ShowDialoguePanel());
    }

    private IEnumerator ShowDialoguePanel()
    {
        dialoguePanelObject.SetActive(true);
        yield return SlideDialoguePanel(hiddenPosition, shownPosition, 0.4f);

        advanceButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        _pointObejct.SetActive(false);

        DisplayCurrentLine();
    }

    private void OnAdvancePressed()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentFullText;
            isTyping = false;
            return;
        }

        if (currentIndex >= linesList.Count) return;
        currentIndex++;

        // Desativa o painel ao avançar do quarto diálogo
        if(_isTutorial)
            if (painelTutorial.activeSelf && currentIndex != 3)
            {
                painelTutorial.SetActive(false);
            }

        if (currentTrigger?.proximityPoint != null && currentIndex >= linesList.Count)
        {
            ShowPointInformation();
        }
    
        else
        {
            if(currentIndex >= linesList.Count - 1)skipButton.GetComponent<Animator>().enabled = true;
            
            
            DisplayCurrentLine();
        }
    }

    private void OnBackPressed()
    {
        // skipButton.GetComponent<Animator>().enabled = currentIndex >= linesList.Count - 1;
        if (_showingPoint)
        {
            _showingPoint = false;
            _pointObejct.SetActive(false);
            currentIndex = linesList.Count - 1;
            DisplayCurrentLine();
            return;
        }
        if (currentIndex >= linesList.Count)
        {
            currentIndex = linesList.Count - 1;
            DisplayCurrentLine();
            return;
        }
        if (currentIndex > 0)
        {
            currentIndex--;
            DisplayCurrentLine();
        }
    }

    private void OnSkipPressed()
    {
        CloseDialogueManually();
    }

    private void DisplayCurrentLine()
    {
        if (currentIndex < 0 || currentIndex >= linesList.Count) return;

        DialogueLines lineData = linesList[currentIndex];

        characterNameText.text = lineData.character.characterName;
        characterIcon.transform.localPosition = _iconAdjust;
        if (lineData.character.characterName == "Quati")
            characterIcon.transform.localPosition = new Vector3(0, -143, 0);
        characterIcon.sprite = lineData.character.characterIcon;
        characterIcon.preserveAspect = true;
        characterIcon.color = Color.white;

        backButton.gameObject.SetActive(currentIndex > 0);
        advanceButton.gameObject.SetActive(currentIndex < linesList.Count - 1 || (currentTrigger?.proximityPoint != null && currentIndex == linesList.Count - 1));

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (!linesRead.Contains(currentIndex))
        {
            if (lineData.skip == false)
            {
                typingCoroutine = StartCoroutine(TypeText(lineData.line));
            }
            else
            {
                dialogueText.text = lineData.line;
            }
            linesRead.Add(currentIndex);
        }
        else
        {
            dialogueText.text = lineData.line;
            isTyping = false;
        }
        if (currentIndex >= linesList.Count - 1 && currentTrigger?.proximityPoint == null)
        {
            advanceButton.gameObject.SetActive(false);

            if (_isTutorial)
            {
                backButton.enabled = false;
                PlayerPrefs.SetInt("FirstPlay", 1);
                StartCoroutine(EndTutorial());
            }
            
        }
        if(_isTutorial)
        if (npcTutorial.barracaTutorial ==  true)
        {
            painelTutorial.SetActive(currentIndex == 3);
            painelBack.SetActive(currentIndex != 3);
            celular.SetActive(currentIndex >= 3);
                if (Application.isMobilePlatform)
                {
                    btn_moviment.SetActive(currentIndex >= 11);
                }
                
                             
        }
    }
    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(2f);
        transition.SetActive(true);
        yield return new WaitForSeconds(1f);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("CampoGrande");
        while(!loadOperation.isDone){
            yield return null;
        }

    }
    

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        currentFullText = line;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowPointInformation()
    {
        advanceButton.gameObject.SetActive(false);
        // backButton.gameObject.SetActive(false);

        if (currentTrigger?.proximityPoint != null)
        {
            _showingPoint = true;
            _pointObejct.SetActive(true);
            _pointImage.sprite = currentTrigger.proximityPoint.img;
            _pointName.text = currentTrigger.proximityPoint.pointName;
        }
    }

    public void LoadSceneFromPoint()
    {
        if (currentTrigger?.proximityPoint != null)
        {
            string sceneName = currentTrigger.proximityPoint.pointScene;
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Cena não definida no ProximityPoint: " + currentTrigger.proximityPoint.name);
            }
        }
    }

    public void CloseDialogueManually()
    {
        StartCoroutine(HideDialoguePanel());
    }

    private IEnumerator HideDialoguePanel()
    {
        advanceButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        _pointObejct.SetActive(false);

        yield return SlideDialoguePanel(shownPosition, hiddenPosition, 0.4f);

        dialogueText.text = "";
        characterNameText.text = "";
        characterIcon.sprite = null;
        characterIcon.color = new Color(0f, 0f, 0f, 0f);

        dialoguePanelObject.SetActive(false);

        currentTrigger?.ResetInteraction();
        // FindObjectOfType<DialogueProximityStarter>()?.RefreshUI();
    }

    private IEnumerator SlideDialoguePanel(Vector2 from, Vector2 to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            dialoguePanel.anchoredPosition = Vector2.Lerp(from, to, time / duration);
            yield return null;
        }

        dialoguePanel.anchoredPosition = to;
    }


}