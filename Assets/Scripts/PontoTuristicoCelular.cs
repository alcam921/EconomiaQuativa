using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PontoTuristicoCelular : MonoBehaviour
{
    public TextMeshProUGUI placeText1, placeText2;
    public Image placeImage;
    public string text1, text2;
    public Sprite placeSprite;
    public GameObject pointer;
    private PontoTuristicoCelular _currentPoint;

    public void ReceiveValues(PontoTuristicoCelular ponto)
    {
        placeImage.sprite = ponto.placeSprite;
        placeImage.preserveAspect = true;
        placeText1.text = ponto.text1;
        placeText2.text = ponto.text2;
    }

    public void BlackOut()
    {
        gameObject.GetComponent<Image>().color = Color.black;
    }
    public void WhiteOut()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }

}
