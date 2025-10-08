using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{

    private bool _open = false;

    [SerializeField] private GameObject _phone;
    [SerializeField] private GameObject _phoneIcon;
    [SerializeField] private GameObject _phonePoint;
    [SerializeField] private GameObject _joystick;
    private Animator _anim;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;
    private bool _canOpen = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenClosePhone(Button btn)
    {
        if (_canOpen)
            StartCoroutine(OpenCloseAsync(btn));
    }
    IEnumerator OpenCloseAsync(Button btn)
    {
        btn.interactable = false;
        _canOpen = false;

        if (_open)
        {
            _phone.GetComponent<Animator>().SetBool("Opened", false);
            _phoneIcon.SetActive(true);
            _audioSource.PlayOneShot(_closeSound);

            yield return new WaitForSeconds(1f);
            _joystick.gameObject.SetActive(true);
            _phonePoint.SetActive(false);
            _phone.SetActive(false);
            _open = false;
        }
        else
        {
            _audioSource.PlayOneShot(_openSound);
            _joystick.gameObject.SetActive(false);
            _phone.SetActive(true);
            _phone.GetComponent<Animator>().SetBool("Opened", true);

            yield return new WaitForSeconds(1f);
            _phoneIcon.SetActive(false);

            _open = true;
        }
        btn.interactable = true;
        _canOpen = true;
    }


}
