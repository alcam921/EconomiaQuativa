using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DialogueBox
{
    [TextArea(3, 10)]
    public string line;
    public GameObject conversationBox;
    public GameObject character;
}
[System.Serializable]
public class Dialogues
{
    public DialogueBox[] dialogues;
    public Sprite backgroundSprite;
    public string sceneWhenEnd;
}

public class Dialogue : MonoBehaviour
{

    [SerializeField] private Dialogues[] dialoguesList;
    [SerializeField] private Image backgroundDialogue;

    [SerializeField] private float typingSpeed;
    [SerializeField] private float endDialogueSpeed;
    [SerializeField] private bool canTalk = true;
    [SerializeField] private bool canType = true;
    [SerializeField] private bool isComplete = false;
    

    [SerializeField] private int _dialogIndex = 0;
    private int _levelIndex;

    private GameObject _currentTextArea;
    private DialogueBox _dialogueLine;




    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.SetInt("levelIndex", 1);
        _levelIndex = PlayerPrefs.GetInt("levelIndex");
        SetBackgroundImage();
    }
    void SetBackgroundImage(){
        backgroundDialogue.sprite = dialoguesList[_levelIndex].backgroundSprite;

    }

    // Update is called once per frame
    void Update()
    {

        CheckDialogue();
        if(canTalk){
            StartCoroutine(TypeSentence(dialoguesList[_levelIndex].dialogues[_dialogIndex]));
        }
          
    }

    void CheckDialogue(){
        if(_dialogIndex >= dialoguesList[_levelIndex].dialogues.Length){
            StartCoroutine(EndDialogue());
        }
    }
    public void SkipDialogue(){
        StopAllCoroutines();
        canTalk = false;
        if (dialoguesList[_levelIndex].sceneWhenEnd != "")
        {
            SceneManager.LoadScene(dialoguesList[_levelIndex].sceneWhenEnd);
        }
    }

    IEnumerator TypeSentence(DialogueBox dialogueLine)
    {   
        canTalk = false;

        _dialogueLine = dialogueLine;

        _currentTextArea = _dialogueLine.conversationBox.gameObject;
        _currentTextArea.SetActive(true);
        _dialogueLine.character.SetActive(true);
        TMP_Text currentTextLine = dialogueLine.conversationBox.GetComponentInChildren<TMP_Text>();
        currentTextLine.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            isComplete = false;
            if(canType){

                currentTextLine.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }else{
                break;
            }


        }
        if(!canType){
            currentTextLine.text = "";
            currentTextLine.text = dialogueLine.line;
            isComplete = true;
        }
        canType = true;
            isComplete = true;

        yield return new WaitForSeconds(endDialogueSpeed);      
    }
    

    public IEnumerator EndDialogue(){
        canTalk = false;
        yield return new WaitForSeconds(1f);
        if (dialoguesList[_levelIndex].sceneWhenEnd != "")
        {
            SceneManager.LoadScene(dialoguesList[_levelIndex].sceneWhenEnd);
        }
    }
    //Isso apenas serve
    public void ChangeToNextDialogue(){
        
        canType = false;
       if(isComplete){
        StopAllCoroutines();
        CheckDialogue();
        canTalk = true;
        _dialogIndex++;
        canType = true;
        }
      
    }

    void DisableDialogue(){
        _dialogueLine.character.SetActive(false);
        _currentTextArea.SetActive(false);   
    }

 }
