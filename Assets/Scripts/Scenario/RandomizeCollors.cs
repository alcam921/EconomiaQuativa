using UnityEngine;
using UnityEngine.UI;

public class RandomizeCollors : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Image[] imagesToColor;
    [SerializeField] private Material paticle;
    private float _timeLeft;
    private Color _targetColor;
    // Start is called before the first frame update
    void Start()
    {
        // _targetColor = new Color(colors[0].r, colors[0].g, colors[0].b);

        int randomColor = Random.Range(0, colors.Length-1);
        _targetColor = new Color(colors[randomColor].r, colors[randomColor].g, colors[randomColor].b);
        foreach(Image image in imagesToColor){

            image.color = _targetColor;
        }
        paticle.color = _targetColor;
        

        _timeLeft = 10.0f;
        foreach(Image image in imagesToColor){

            image.color = Color.Lerp(imagesToColor[0].color, _targetColor, Time.deltaTime / _timeLeft);
        }
        paticle.color = Color.Lerp(imagesToColor[0].color, _targetColor, Time.deltaTime / _timeLeft);
    }

}
