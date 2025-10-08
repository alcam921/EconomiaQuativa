using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressController : MonoBehaviour
{
    #region Fields
    [Header("Panels")]
    [Tooltip("Place the panels in the order they will be selected.")]
    [SerializeField] private GameObject[] _panelsMinigames;
    [SerializeField] private string[] _cityKeys = { "CampoGrande", "Dourados", "Bonito", "TresLagoas", "Corumba" };
    [SerializeField] private string[] _fullCityNames = { "Campo Grande", "Dourados", "Bonito", "Três Lagoas", "Corumbá" };
    [SerializeField] private int[] _quizMaxScores = {6,8,8,8,8};

    [Header("Buttons")]
    [SerializeField] private Button _nextPanel;
    [SerializeField] private Button _lastPanel;
    private int _currentPanelIndex = 0;
    private bool _isAnimating = false;

    [Header("Quiz Minigame")]
    [SerializeField] private Image[] _imagesQuizMinigames;
    [SerializeField] private TMP_Text[] _textMinigames;
    [SerializeField] private Slider _progressQuizMinigames;

    [Header("Where's Quati Minigame")]
    [SerializeField] private Image[] _imagesQuatiMinigames;
    [SerializeField] private TMP_Text _highTextQuatiMinigames; // Renamed for clarity
    [SerializeField] private Sprite[] _citySpriteQuatiMinigames;
    [SerializeField] private Slider _progressQuatiMinigames;

    [Header("Memory Minigame")]
    [SerializeField] private Image[] _imagesMemorysMinigames;
    [SerializeField] private TMP_Text _highTextMemoryMinigames;
    [SerializeField] private Sprite[] _citySpriteMemoryMinigames;
    [SerializeField] private Slider _progressMemoryMinigames;

    [Header("Fifteen Puzzle Minigame")]
    [SerializeField] private Image[] _imagesFifteenMinigames;
    [SerializeField] private TMP_Text _highTextFifteenMinigames;
    [SerializeField] private Sprite[] _citySpriteFifteenMinigames;
    [SerializeField] private Slider _progressFifteenMinigames;

    #endregion

    #region Unity
    void Start()
    {
        UpdateProgress();
        // The first panel should be the only one visible initially
        _panelsMinigames[_currentPanelIndex].SetActive(true);
        UpdateNavigationButtons();
    }
    #endregion

    #region Public
    public void NextPanel()
    {
        if (_isAnimating || _currentPanelIndex >= _panelsMinigames.Length - 1)
        {
            return;
        }

        StartCoroutine(AnimatePanel(_currentPanelIndex, _currentPanelIndex + 1, 1));
    }

    public void LastPanel()
    {
        if (_isAnimating || _currentPanelIndex <= 0)
        {
            return;
        }

        StartCoroutine(AnimatePanel(_currentPanelIndex, _currentPanelIndex - 1, -1));
    }

    private IEnumerator AnimatePanel(int startPanelIndex, int endPanelIndex, int direction)
    {
        _isAnimating = true;

        GameObject startPanel = _panelsMinigames[startPanelIndex];
        GameObject endPanel = _panelsMinigames[endPanelIndex];

        startPanel.SetActive(true);
        endPanel.SetActive(true);

        RectTransform startPanelTransform = startPanel.GetComponent<RectTransform>();
        RectTransform endPanelTransform = endPanel.GetComponent<RectTransform>();

        float screenWidth = Screen.width;
        endPanelTransform.anchoredPosition = new Vector2(direction * screenWidth, 0);

        float time = 0f;
        float duration = 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            startPanelTransform.anchoredPosition = new Vector2(-direction * screenWidth * t, 0);
            endPanelTransform.anchoredPosition = new Vector2(direction * screenWidth * (1 - t), 0);

            yield return null;
        }

        endPanelTransform.anchoredPosition = Vector2.zero;
        startPanel.SetActive(false);

        _currentPanelIndex = endPanelIndex;
        UpdateNavigationButtons();
        _isAnimating = false;
    }
    #endregion

    #region Private
    private void UpdateNavigationButtons()
    {
        _lastPanel.interactable = _currentPanelIndex > 0;
        _nextPanel.interactable = _currentPanelIndex < _panelsMinigames.Length - 1;
    }

    private void UpdateProgress()
    {
        for (int i = 0; i < _cityKeys.Length; i++)
        {
            string cityKey = _cityKeys[i];

            // --- Quiz ---
            HandleQuiz(cityKey, i);

            // --- Outros minigames ---
            HandleMinigame(cityKey, i, ScoreManager.Minigame.encontreQuati, _imagesQuatiMinigames, _highTextQuatiMinigames);
            HandleMinigame(cityKey, i, ScoreManager.Minigame.memoria, _imagesMemorysMinigames, _highTextMemoryMinigames);
            HandleMinigame(cityKey, i, ScoreManager.Minigame.quebraCabeca, _imagesFifteenMinigames, _highTextFifteenMinigames);
        }

        // Atualiza sliders de progresso
        _progressQuizMinigames.value = (float)GetCompletedQuizCount() / _cityKeys.Length;
        _progressQuatiMinigames.value = (float)GetCompletedMinigameCount(ScoreManager.Minigame.encontreQuati) / _cityKeys.Length;
        _progressMemoryMinigames.value = (float)GetCompletedMinigameCount(ScoreManager.Minigame.memoria) / _cityKeys.Length;
        _progressFifteenMinigames.value = (float)GetCompletedMinigameCount(ScoreManager.Minigame.quebraCabeca) / _cityKeys.Length;
    }
    private void HandleQuiz(string cityKey, int index)
    {
        string flagKey = $"{cityKey}_quiz";
        string flagPointsKey = $"{cityKey}_quiz_points";
        Debug.Log(flagKey);
        Debug.Log(flagPointsKey);
        // string sceneQuizKey = $"{cityKey}";
        int maxQuestions = GetMaxQuestionsForCity(cityKey);
        
        int highscore = PlayerPrefs.GetInt(flagPointsKey, 0);
        Debug.Log(flagPointsKey + highscore + "/" + maxQuestions);
        if (PlayerPrefs.HasKey(flagKey))
        {
            _imagesQuizMinigames[index].color = Color.white;
            // _imagesQuizMinigames[index].sprite = _citySpriteMinigames[index];
            _textMinigames[index].color = new Color(245,245,220,255);
            _textMinigames[index].text = $"{_fullCityNames[index]}\n{highscore}/{maxQuestions}";
        }
        else
        {
            _imagesQuizMinigames[index].color = Color.black;
            _textMinigames[index].color = Color.black;
            _textMinigames[index].text = $"{_fullCityNames[index]}\n-/-";
        }
    }

    private void HandleMinigame(string cityKey, int index, ScoreManager.Minigame minigame, Image[] images, TMP_Text textsFields)
    {
        string playerPrefKey = $"{cityKey}_{minigame}";
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            images[index].color = Color.white;
            textsFields.text = $"Maior Pontuação : {PlayerPrefs.GetInt(minigame.ToString())}";
        }
        else
        {
            images[index].color = Color.black;
            // textsFields[index].color = Color.black;
        }
    }

    private int GetCompletedQuizCount()
    {
        int count = 0;
        for (int i = 0; i < _cityKeys.Length; i++)
        {
            string sceneQuizKey = $"{_cityKeys[i]}_quiz_points";
            string playerPrefKey = $"{_cityKeys[i]}_quiz";
            if (PlayerPrefs.HasKey(playerPrefKey))
                count++;
            // int maxQuestions = GetMaxQuestionsForCity(_cityKeys[i]);
            // if (PlayerPrefs.GetInt(sceneQuizKey, 0) == maxQuestions)
            //     count++;
        }
        return count;
    }

    private int GetCompletedMinigameCount(ScoreManager.Minigame minigame)
    {
        int count = 0;
        for (int i = 0; i < _cityKeys.Length; i++)
        {
            string playerPrefKey = $"{_cityKeys[i]}_{minigame}";
            if (PlayerPrefs.HasKey(playerPrefKey))
                count++;
        }
        return count;
    }
    #endregion

    private int GetMaxQuestionsForCity(string cityKey)
    {
        int index = System.Array.IndexOf(_cityKeys, cityKey);
        return _quizMaxScores[index];
    }
}