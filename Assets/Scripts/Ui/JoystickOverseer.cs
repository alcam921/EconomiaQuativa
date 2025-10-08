using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class JoystickOverseer : MonoBehaviour
{
    public List<Image> images;
    public OnScreenStick stone;
    public DynamicJoystick joystick;
    public delegate void Disabled();
    public static Disabled disabled;
    public delegate void Enabled();
    public static Enabled enabledd;

    private void OnEnable()
    {
        enabledd = FakeEnable;
        disabled = FakeDisable;
        Player.On?.Invoke(true);
    }
    private void OnDisable()
    {
        enabledd = null;
        disabled = null;
        Player.On?.Invoke(false);
    }
    public void FakeDisable()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].enabled = false;
        }
        stone.enabled = false;
        joystick.enabled = false;
    }
    public void FakeEnable()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].enabled = true;
        }
        stone.enabled = true;
        joystick.enabled = true;
    }
}
