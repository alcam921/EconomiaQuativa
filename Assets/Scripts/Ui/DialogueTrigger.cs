using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
[System.Serializable]
public class Dialogue1
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Button btn;
    public delegate void End();
    public static End ending;
    public Dialogue1 dialogue1;
    public delegate void SetListen();
    public static SetListen setListen;
    Npc npc1;
    private void OnEnable()
    {
        setListen = SetListener;
        ending = EndDialogue1;
        DialogueManager.skipDialogue = Skip;
    }
    private void OnDisable()
    {
        setListen = null;
        ending = null;
        DialogueManager.skipDialogue = null;
    }
    private void Start()
    {
        npc1 = GetComponent<Npc>();
    }
    public void TriggerDialogue()
    {
        npc1.canMove = false;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(DialogueManager.Instance.ChangeSpeed);
        DialogueManager.Instance.StartDialogue(dialogue1);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
            if (collision.tag == "Player")
        {           
            btn.onClick.AddListener(TriggerDialogue);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            npc1.canMove = true;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(TriggerDialogue);
        }
    }

    void Skip()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(DialogueManager.Instance.DisplayNextDialogueLine);

    }
    void EndDialogue1()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(TriggerDialogue);
    }
    void SetListener()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(DialogueManager.Instance.ChangeSpeed);
    }
}
