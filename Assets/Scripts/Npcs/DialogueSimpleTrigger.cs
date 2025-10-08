using UnityEngine;

public class DialogueSimpleTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    public ProximityPoint proximityPoint;
    public GameObject objectToTrigger;
    public bool wasInitiated = false;

    private bool dialogueStarted = false;

    public bool HasDialogueStarted() => dialogueStarted;

    public void MarkDialogueStarted()
    {
        
        dialogueStarted = true;
        Player.On?.Invoke(false);
    }

    public void ResetInteraction()
    {
        
        dialogueStarted = false;
        Player.On?.Invoke(true);
    }
}
