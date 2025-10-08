using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public enum Minigame { quebraCabeca, encontreQuati, quiz, memoria }
    public Minigame minigame;
    private string[] _cityKeys = { "CampoGrande", "Dourados", "Bonito", "Corumba", "TresLagoas"  };
    public delegate void Pontuar();
    public static Pontuar pontuar;
    public delegate void PontosFinais();
    public static PontosFinais finais;
    public delegate void MoreTimer();
    public static MoreTimer moreTimer;
    public delegate void LessTimer();
    public static LessTimer lessTimer;
    public delegate void TimerControl();
    public static TimerControl timerControl;
    public delegate void CheckTutorial(bool a);
    public static CheckTutorial checkTutorial;
    public int currentPoints;
    public TextMeshProUGUI textPoints, textHighPoint, endText, fimJogo;
    public Slider timer;
    public float gameTime;
    private bool stopTimer;
    public float addTimer;
    public float timeLoss;
    public bool isFindQuati;
    public GameObject gameOver, UI, tabuleiro;
    private bool timerPaused = false;
    private float pausedSeconds;
    private float playedSeconds;
    public Image endPanel;
    private int cidadeIndex;
    public bool isTutorial;

    //variaveis de Audio Effect
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sceneAudio;
    [SerializeField] private AudioClip[] soundTrilha;
    [SerializeField] private AudioClip[] sceneSongs;
    [SerializeField] private string sceneToLeave;

    private void OnEnable()
    {
        pontuar = Pontuacao;
        finais = Fin;
        moreTimer = TimerAdd;
        lessTimer = TimerRemove;
        timerControl = PauseUnPause;
        checkTutorial = Tutorial;
    }
    private void OnDisable()
    {
        pontuar = null;
        finais = null;
        moreTimer = null;
        lessTimer = null;
        timerControl = null;
        checkTutorial = null;
    }
    private void Start()
    {
        cidadeIndex = PlayerPrefs.GetInt("indexCidade");
        sceneAudio.clip = sceneSongs[cidadeIndex];
        sceneAudio.Play();
        // audioSource.clip = soundTrilha[0];
        // audioSource.Play();
        UI.SetActive(true);
        gameOver.SetActive(false);
        stopTimer = false;
        timer.maxValue = gameTime;
        timer.value = gameTime;
        currentPoints = 0;
        if (minigame == Minigame.quiz)
        {
            GameObject obj = GameObject.Find("QuizManager");
            int maxQuestion = obj.GetComponent<Quiz>().quizObject[0].quizList.Count();
            textPoints.text = $"Pontuação: {currentPoints}/{maxQuestion}";
            textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(SceneManager.GetActiveScene().ToString())}/{maxQuestion}";
        }
        else
        {
            textPoints.text = $"Pontuação: {currentPoints}";
            textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
        }

        // text.text = $"Pontuação: {currentPoints}  Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
    }
    private void Update()
    {
        if (minigame != Minigame.quiz)
        {
            if (timerPaused == false)
            {
                float time = gameTime - Time.timeSinceLevelLoad;
                playedSeconds = Time.timeSinceLevelLoad - pausedSeconds;

                if (time <= 0 && stopTimer == false)
                {
                    stopTimer = true;
                    Fin();
                }
                if (stopTimer == false)
                {
                    timer.value = time;
                    timer.value = Mathf.Clamp(timer.value, 0f, timer.maxValue);
                }
            }
            else
            {
                pausedSeconds = Time.timeSinceLevelLoad - playedSeconds;
            }
        }
    }

    public void Pontuacao()
    {
        if (isFindQuati == true && timer.maxValue > 6)
        {
            timer.maxValue *= 0.9f;
        }
        //currentPoints++;
        // text.text = $"Pontuação: {currentPoints}  Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";

        currentPoints++;
        if (minigame == Minigame.quiz)
        {
            GameObject obj = GameObject.Find("QuizManager");
            int maxQuestion = obj.GetComponent<Quiz>().quizObject[0].quizList.Count();
            textPoints.text = $"Pontuação: {currentPoints}/{maxQuestion}";
            textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(SceneManager.GetActiveScene().ToString())}/{maxQuestion}";
        }
        else
        {
            textPoints.text = $"Pontuação: {currentPoints}";
            textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
        }

    }
    public void Fin()
    {
        if (currentPoints > PlayerPrefs.GetInt(minigame.ToString()))
        {
            PlayerPrefs.SetInt(minigame.ToString(), currentPoints);
        }
        textPoints.text = $"Pontuação: {currentPoints}";
        textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
        if (!PlayerPrefs.HasKey($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}"))
        {
            Debug.Log($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}");
            PlayerPrefs.SetInt($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}", 1);
        }
        // currentPoints = 0;
        // text.text = $"Pontuação: {currentPoints}  Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
        UI.SetActive(false);
        gameOver.SetActive(true);
        tabuleiro.SetActive(false);
        endText.text = $"Pontuação Mais alta: {PlayerPrefs.GetInt(minigame.ToString())} \n Pontuação atual: {currentPoints} ";
        currentPoints = 0;

        // audioSource.Stop();
        // audioSource.PlayOneShot(soundTrilha[1]);
        // audioSource.PlayOneShot(soundTrilha[1]);
        // endText.text = $"Pontuação: {currentPoints}  Mais alta: {PlayerPrefs.GetInt(minigame.ToString())}";
    }
    void TimerAdd()
    {
        if (gameTime + addTimer - Time.timeSinceLevelLoad > timer.maxValue)
        {
            gameTime = Time.timeSinceLevelLoad + timer.maxValue;
        }
        else
        {
            gameTime += addTimer;
        }


    }
    void TimerRemove()
    {
        gameTime -= timeLoss;
    }
    public void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Leave()
    {

        SceneManager.LoadScene(PlayerPrefs.GetString("lastScene"));

    }
    public void PauseUnPause()
    {
        if (!isTutorial)
        {
            if (timerPaused == true)
            {
                gameTime += pausedSeconds;
                pausedSeconds = 0;
            }
            timerPaused = !timerPaused;
        } else
        {
            timerPaused = true;
        }
    }
    public void Win()
    {
        Debug.Log(PlayerPrefs.GetInt(SceneManager.GetActiveScene().ToString()));
        Debug.Log(currentPoints + " vs " + PlayerPrefs.GetInt($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}_points"));
        if (currentPoints > PlayerPrefs.GetInt($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}_points"))
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().ToString(), currentPoints);

            
            Debug.Log($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}_points");
            PlayerPrefs.SetInt($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}_points", currentPoints);
        }
        if (!PlayerPrefs.HasKey($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}"))
        {
            Debug.Log($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}");
            PlayerPrefs.SetInt($"{_cityKeys[cidadeIndex]}_{minigame.ToString()}", 1);
        }

        textPoints.text = $"Pontuação: {currentPoints}";
        textHighPoint.text = $"Mais alta: {PlayerPrefs.GetInt(SceneManager.GetActiveScene().ToString())}";
        fimJogo.text = "Parabéns!";
        endPanel.color = new Color(0.0795529f, 0.5283019f, 0.1842737f, 0.8f);
        UI.SetActive(false);
        gameOver.SetActive(true);
        tabuleiro.SetActive(false);
        GameObject obj = GameObject.Find("QuizManager");
        int maxQuestion = obj.GetComponent<Quiz>().quizObject[0].quizList.Count();
        endText.text = $"Pontuação Mais alta: {PlayerPrefs.GetInt(SceneManager.GetActiveScene().ToString())} de {maxQuestion} questões \n Pontuação atual: {currentPoints} de {maxQuestion} questões ";
        currentPoints = 0;
    }
    public void Tutorial(bool a)
    {
        isTutorial = a;
    }
}
