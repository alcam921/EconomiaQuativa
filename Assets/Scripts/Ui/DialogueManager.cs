using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public delegate void SkipDialogue();
    public static SkipDialogue skipDialogue;
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();

    public bool dialogueActive = false;
    public float typingSpeed = 0.1f;
    float oldType;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        oldType = typingSpeed;
        if (Instance == null)
            Instance = this;
    }
    public void StartDialogue(Dialogue1 dialogue1)
    {
        dialogueActive = true;
        animator.Play("Show");
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue1.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
        DisplayNextDialogueLine();
    }
    public void DisplayNextDialogueLine()
    {
        DialogueTrigger.setListen?.Invoke();
        typingSpeed = oldType;
        if (lines.Count == 0)
        {
            DialogueTrigger.ending?.Invoke();
            EndDialogue();
            
            return;
        }
        DialogueLine currentLine = lines.Dequeue();
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));

        IEnumerator TypeSentence(DialogueLine dialogueLine)
        {
            dialogueArea.text = "";
            foreach (char letter in dialogueLine.line.ToCharArray())
            {
                dialogueArea.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            if(dialogueArea.text == dialogueLine.line)
            {
                skipDialogue?.Invoke();
            }
        }
        void EndDialogue()
        {
            dialogueActive = false;
            animator.Play("Hide");
            
        }
        
    }
    public void ChangeSpeed()
    {
        typingSpeed = 0;
    }
}
