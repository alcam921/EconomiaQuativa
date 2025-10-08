using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class QuizButton
{
    [TextArea(3, 10)]
    public string line;
    // public Button button;

    public bool itsTrue;
}

[System.Serializable]
public class QuizList
{
    [TextArea(3, 10)] public string question;
    public TMP_Text questionText;
    public QuizButton[] answerButtons;


}
[System.Serializable]
public class QuizObject
{
    public QuizList[] quizList;
    public string sceneWhenWin;


}
public class Quiz : MonoBehaviour
{
    public QuizObject[] quizObject;
    public Button[] quizButtons;

    private int _levelIndex;
    private int _quizIndex = 0;
    private bool _canCreateAnswer = true;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject wrongCanvas;
    [SerializeField] private GameObject rightCanvas;

    public AudioSource audioSource;
    public AudioClip[] clipEffects;
    public ScoreManager score;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("levelIndex", 0);
        _levelIndex = PlayerPrefs.GetInt("levelIndex");
    }

    // Update is called once per frame
    void Update()
    {
        if (_canCreateAnswer)
        {

            CreateAnswers();
        }


        SetQuizText();

    }

    public void DisableCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
    }
    public void Wrong(GameObject canvas)
    {
        DisableCanvas(canvas);
        _quizIndex = 0;
        // Debug.Log("Cliquei");
        audioSource.Stop();
    }
    public void ResetGame(GameObject canvas)
    {
        DisableCanvas(canvas);
        _quizIndex = 0;
        _canCreateAnswer = true;
        audioSource.Stop();
    }
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);

    }

    void SetQuizText()
    {
        if (_quizIndex != quizObject[_levelIndex].quizList.Length)
        {
            levelText.text = $"{_quizIndex + 1} de {quizObject[_levelIndex].quizList.Length}";


        }
        else
        {
            levelText.text = $"{_quizIndex} de {quizObject[_levelIndex].quizList.Length}";
        }
    }

    void CreateAnswers()
    {
        _canCreateAnswer = false;
        // for(int i =0; i <= quizObject[_levelIndex].quizList.Length;i++){}
        foreach (Button answerButtons in quizButtons)
        {
            answerButtons.enabled = false;
        }

        if (_quizIndex != quizObject[_levelIndex].quizList.Length)
        {
            int _buttonsQuantity = 0;
            // Debug.Log(_quizIndex);
            quizObject[_levelIndex].quizList[_quizIndex].questionText.text = quizObject[_levelIndex].quizList[_quizIndex].question;
            foreach (QuizButton quiz in quizObject[_levelIndex].quizList[_quizIndex].answerButtons)
            {
                quizButtons[_buttonsQuantity].enabled = true;
                quizButtons[_buttonsQuantity].GetComponentInChildren<TMP_Text>().text = quiz.line;
                quizButtons[_buttonsQuantity].onClick.RemoveAllListeners();
                if (quiz.itsTrue)
                {
                    quizButtons[_buttonsQuantity].onClick.AddListener(() =>
                    {
                        for (int i = 0; i < quizButtons.Length; i++)
                        {
                            quizButtons[i].enabled = false;
                            if (quizButtons[i].GetComponentInChildren<TMP_Text>().text == quiz.line)
                            {
                                audioSource.clip = clipEffects[0];
                                audioSource.Play();
                                StartCoroutine(ButtonFlashGreen(i));
                            }
                        }

                    });

                }
                else
                {
                    quizButtons[_buttonsQuantity].onClick.AddListener(() =>
                    {
                        // quiz.button.GetComponent<Outline>().effectColor = Color.red;
                        for (int i = 0; i < quizButtons.Length; i++)
                        {
                            quizButtons[i].enabled = false;
                            if (quizButtons[i].GetComponentInChildren<TMP_Text>().text == quiz.line)
                            {
                                audioSource.clip = clipEffects[0];
                                audioSource.Play();
                                StartCoroutine(ButtonFlashRed(i));
                            }
                        }



                    });

                }
                _buttonsQuantity++;
            }
        }
        else
        {
            score.Win();
        }
    }
    public IEnumerator ButtonFlashGreen(int i)
    {
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(0.03f);
            quizButtons[i].image.color = Color.Lerp(quizButtons[i].image.color, Color.green, 0.5f);
        }
        yield return new WaitForSeconds(0.03f);
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(0.03f);
            quizButtons[i].image.color = Color.Lerp(quizButtons[i].image.color, Color.white, 0.5f);
        }
        yield return new WaitForSeconds(0.3f);
        _quizIndex++;
        _canCreateAnswer = true;
        ScoreManager.pontuar?.Invoke();

        for (int k = 0; k < quizButtons.Length; k++)
        {
            quizButtons[i].enabled = true;
        }


    }
    public IEnumerator ButtonFlashRed(int i)
    {
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(0.03f);
            quizButtons[i].image.color = Color.Lerp(quizButtons[i].image.color, Color.red, 0.5f);
        }
        yield return new WaitForSeconds(0.1f);
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(0.03f);
            quizButtons[i].image.color = Color.Lerp(quizButtons[i].image.color, Color.white, 0.5f);
        }
        yield return new WaitForSeconds(0.3f);
        _quizIndex++;
        _canCreateAnswer = true;

        for (int k = 0; k < quizButtons.Length; k++)
        {
            quizButtons[i].enabled = true;
        }
    }

}
