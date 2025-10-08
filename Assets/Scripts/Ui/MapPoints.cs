using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[Serializable]
public class PointInfo
{
    public Image point;
    public bool state;
    public bool isSpecial;
    public string scene;
    public GameObject lockIcon; // Cadeado (novo)
}
public class MapPoints : MonoBehaviour
{
    [SerializeField] private GameObject mapSelector;
    // [SerializeField] Image cityImage;
    [SerializeField] TextMeshProUGUI textoBotao;
    [SerializeField] Transform trem, destino1, destino2, destino3, destino4, destino5, destinoCred;
    // [SerializeField] Animator caixaTexto;
    [SerializeField] GameObject box;
    [SerializeField] Button botao;
    public string[] scenes;
    public PointInfo[] pointInfos;
    public int scene = 0;
    public int currentLine = 0;
    [SerializeField] Transform[] positions;
    [SerializeField] private TremMovement tremMovement;
    private bool _stoped = true;
    private bool _start = false;
    private bool _buttonCloseClicked = false;
    [SerializeField] private GameObject openBoxButton;
    [SerializeField] private Color cityDisabledColor;
    public DialogueData dialogue;
    public DialogueSimpleTrigger dialogueTrigger;
    private bool _dialogueStarted = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstPlay"))
        {
            // Desbloqueia só o primeiro ponto
            for (int i = 0; i < pointInfos.Length; i++)
            {
                if (i == 0)
                    pointInfos[i].state = true;
                else
                    pointInfos[i].state = false;
            }
        }
        UpdateLocks();
        SetPointColor(0);
    }

    private void UpdateLocks()
    {
        for (int i = 0; i < pointInfos.Length; i++)
        {
            // Mostra cadeado se não estiver desbloqueado
            if (pointInfos[i].lockIcon != null)
                pointInfos[i].lockIcon.SetActive(!pointInfos[i].state);
        }
    }

    void Update()
    {
        PopOut();
        if (_stoped)
        {
            if (!_buttonCloseClicked)
            {
                openBoxButton.SetActive(false);
                OpenCloseBox(false);
            }
            else
            {
                openBoxButton.SetActive(true);
            }

            if (currentLine != scene)
            {
                _stoped = false;
            }
            else
            {
                _stoped = true;
            }

        }
    }

    private void PopOut()
    {
        switch (trem.position)
        {
            case var value when value == destino1.position:
                if (!_start)
                {
                    box.SetActive(true);
                    box.GetComponent<Animator>().SetTrigger("Open");
                    _start = true;
                }
                scene = 0;
                _stoped = true;
                if (!PlayerPrefs.HasKey("FirstPlay") && !_dialogueStarted)
                {
                    _dialogueStarted = true;
                    DialogueController.Instance.StartDialogue(dialogue, dialogueTrigger);
                }
                break;
            case var value when value == destino2.position:
                _stoped = true;
                scene = 1;
                break;
            case var value when value == destino3.position:
                _stoped = true;
                scene = 2;
                break;
            case var value when value == destino4.position:
                _stoped = true;
                scene = 3;
                break;
            case var value when value == destino5.position:
                _stoped = true;
                scene = 4;
                break;
            case var value when value == destinoCred.position:
                scene = 5;
                StartCoroutine(ChangeToOtherScene());
                break;

            default:
                if (!_start)
                {
                    tremMovement.DefDestination(positions[0]);
                    SetPointColor(0);
                }
                else
                {
                    OpenCloseBox(true);
                    botao.enabled = true;
                    openBoxButton.SetActive(false);
                    _stoped = false;
                }
                break;
        }
    }

    public void OpenCloseBox(bool state)
    {
        box.GetComponent<Animator>().SetBool("Close", state);
    }

    private void SetPointColor(int btnIndex)
    {
        if (pointInfos[btnIndex].state) botao.interactable = true;
        else botao.interactable = false;
        for (int i = 0; i < pointInfos.Length; i++)
        {
            pointInfos[i].point.color = cityDisabledColor;
            pointInfos[i].point.transform.localScale = new Vector3(1.001f, 1.001f, 1.001f);
        }
        pointInfos[btnIndex].point.color = Color.white;
        pointInfos[btnIndex].point.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void GoDestiny()
    {
        if (_stoped)
        {
            StartCoroutine(ChangeToOtherScene());
        }
        else
        {
            tremMovement.DefDestination(positions[currentLine]);

        }
        botao.enabled = true;
    }
    public void CloseBoxButton(bool state)
    {
        _buttonCloseClicked = state;
    }
    public IEnumerator ChangeToOtherScene()
    {
        // SceneManager.LoadScene(scenes[scene]);       
        FadeHandler.fade?.Invoke();
        yield return new WaitForSeconds(1);

        if (!PlayerPrefs.HasKey("FirstPlay") && scene == 0)
        {
            PlayerPrefs.SetInt("FirstPlay", 1); // Marca que já fez a primeira vez
            SceneManager.LoadScene("CampoGrandeTutorial2");
            yield break;
        }
        else
        {
            PlayerPrefs.SetInt("indexCidade", scene);
            PlayerPrefs.SetString("lastScene", pointInfos[scene].scene);
            SceneManager.LoadScene(pointInfos[scene].scene);

        }

    }

    public void SetPoint(int pointIndex)
    {
        currentLine = pointIndex;
        SetPointColor(currentLine);
        _stoped = false;
        botao.enabled = false;

        if (pointInfos[pointIndex].isSpecial)
        {
            // Destino especial -> delega para outro script
            var special = FindObjectOfType<SpecialDestination>();
            if (special != null)
            {
                special.StartSequence(pointInfos[pointIndex], positions[pointIndex], trem);
            }
        }
        else
        {
            GoDestiny();
        }
    }

    public void MoveToNextPoint()
    {
        if (currentLine < positions.Length - 1)
        {
            currentLine++;

        }
        else
        {
            currentLine = 0;
        }
        SetPointColor(currentLine);
    }
    public void MoveToLastPoint()
    {
        if (currentLine > 0)
        {
            currentLine--;
        }
        else
        {
            currentLine = positions.Length - 1;
        }
        SetPointColor(currentLine);
    }
}
