using UnityEngine;

public class DeviceVerify : MonoBehaviour
{
    public DialogueSimpleTrigger capivara;
    public DialogueData computer, phone;
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            capivara.dialogue = phone;
        } else
        {
            capivara.dialogue = computer;
        }
    }

}
