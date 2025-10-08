using UnityEngine;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour
{
    [SerializeField] private Color outlineColor;
    [SerializeField] private GameObject[] buttons;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons) {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                DisableButtonOutline();
                button.GetComponent<Outline>().enabled = true;
                button.GetComponent<Outline>().effectColor = outlineColor;
            });
        }
    }

    public void DisableButtonOutline()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}