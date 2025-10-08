using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueProximityStarter : MonoBehaviour
{
    public Button interactButton;

    private DialogueSimpleTrigger currentTrigger;

    private void Start()
    {
        // interactButton.gameObject.SetActive(false);
        // interactButton.onClick.RemoveAllListeners();
        // interactButton.onClick.AddListener(StartDialogue);
    }

    private void Update()
    {
        // if (currentTrigger != null && !currentTrigger.HasDialogueStarted())
        // {
        //     interactButton.gameObject.SetActive(true);
        // }
        // else
        // {
        //     interactButton.gameObject.SetActive(false);
        // }
    }

    private void StartDialogue()
    {
        if (currentTrigger == null || currentTrigger.HasDialogueStarted())
            return;

        currentTrigger.MarkDialogueStarted();
        DialogueController.Instance.StartDialogue(currentTrigger.dialogue, currentTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DialogueSimpleTrigger trigger))
        {
            currentTrigger = trigger;
            if(PlayerPrefs.HasKey($"Point{currentTrigger.dialogue.name}{SceneManager.GetActiveScene().name}"))currentTrigger.wasInitiated = true;
            
            if (!currentTrigger.wasInitiated)
            {
                currentTrigger.wasInitiated = true;
                Debug.Log($"{currentTrigger.dialogue.name}");
                PlayerPrefs.SetInt($"Point{currentTrigger.dialogue.name}{SceneManager.GetActiveScene().name}", 1);
                PlayerPrefs.Save();
                StartDialogue();
            }
            else
            {
                EventTrigger eventTrigger;
                if (trigger.objectToTrigger != null)
                    eventTrigger = trigger.objectToTrigger.GetComponent<EventTrigger>();
                else
                {
                    eventTrigger = trigger.GetComponent<EventTrigger>();
                    if (eventTrigger == null)
                        eventTrigger = trigger.gameObject.AddComponent<EventTrigger>();

                }

                eventTrigger.triggers.Clear();

                EventTrigger.Entry entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };

                entry.callback.AddListener((data) =>
                {
                    if (!trigger.HasDialogueStarted())
                    {
                        currentTrigger = trigger;
                        StartDialogue();
                    }
                });

                eventTrigger.triggers.Add(entry);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DialogueSimpleTrigger trigger) && trigger == currentTrigger)
        {
            currentTrigger = null;
        }
    }

    // Este método pode ser chamado pelo DialogueController
    public void RefreshUI()
    {
        // Força a checagem do botão após fim do diálogo
        if (currentTrigger != null && !currentTrigger.HasDialogueStarted())
        {
            // interactButton.gameObject.SetActive(true);

        }
    }
}
