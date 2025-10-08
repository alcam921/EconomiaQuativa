using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovimento : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVet;
    private Vector2 posToqueJoystick;
    private Vector2 posOriginalJoystick;
    private float raioJoystick;

    // Start is called before the first frame update
    void Start()
    {
        posOriginalJoystick = joystickBG.transform.position;
        raioJoystick = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }


    public void PointerDown()
    {
        posToqueJoystick = joystickBG.transform.position;
    }

    public void Arrasta(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 posArrasta = pointerEventData.position;
        joystickVet = (posArrasta - posToqueJoystick).normalized;
        float distJoystick = Vector2.Distance(posArrasta, posToqueJoystick);

        if (distJoystick < raioJoystick)
        {
            joystick.transform.position = posToqueJoystick + joystickVet * distJoystick;
        }
        else
        {
            joystick.transform.position = posToqueJoystick + joystickVet * raioJoystick;
        }
    }

    public void PointerUp()
    {
        joystickVet = Vector2.zero;
        joystick.transform.position = posOriginalJoystick;
        joystickBG.transform.position = posOriginalJoystick;
    }


}
