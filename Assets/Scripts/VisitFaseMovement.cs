using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VisitFaseMovement : MonoBehaviour
{
    [SerializeField] private Image polaridImage;
    [SerializeField] private TMP_Text polaridText;
    [SerializeField] private Button polaridBtn;
    [SerializeField] private GameObject mapInformations;

    private int _pointIndex;
    public static VisitFaseMovement Instance = null;

    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(Instance);
    }

    public void SetPolaroidInformations(ProximityPoint polaroidInfo)
    {

        mapInformations.SetActive(true);
        mapInformations.GetComponent<Animator>().SetBool("Open", true);
        polaridImage.sprite = polaroidInfo.img;
        polaridText.text = polaroidInfo.pointText;
        polaridBtn.onClick.AddListener(() =>
        {
            if (polaroidInfo.pointScene == "DialogueScene")
            {
                PlayerPrefs.SetInt("levelIndex", _pointIndex);

            }
            SceneManager.LoadScene(polaroidInfo.pointScene);

        });
    }
    public void ClosePolaroidInformations()
    {
        mapInformations.SetActive(false);
    }
}
