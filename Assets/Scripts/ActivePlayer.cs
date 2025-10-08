using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject objetoParaAtivar;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Animator _cameraAnim;

    [Header("UI")]
    [Tooltip("Ui's to enable and desable")]
    [SerializeField] private GameObject _uiToAnable;

    private bool _hasAnim = true;
    void Awake()
    {
        if (PlayerPrefs.HasKey($"{SceneManager.GetActiveScene().name}Anim"))
        {

            _hasAnim = false;
            _cameraAnim.enabled = false;
            _uiToAnable.SetActive(true);
        }
    }
    private void Start()
    {
        if (_cameraAnim.enabled != false)
        {
            Player.On?.Invoke(false);
        }
        
    }
    public void AtivarRenderer()
    {
        if (objetoParaAtivar != null && _hasAnim)
        {
            var renderer = objetoParaAtivar.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
                if (player != null)
                {
                    _cameraAnim.enabled = false;
                        player.MoverAteDestino();
                    if (_uiToAnable)
                    _uiToAnable.SetActive(true);
                    PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Anim", 1);
                    Player.On?.Invoke(true);
                }
            }
        }
    }
    IEnumerator StartAnim()
    {
        var renderer = objetoParaAtivar.GetComponent<Renderer>();
        if (renderer != null)
        {
            _cameraAnim.enabled = false;
            yield return new WaitForSeconds(0.01f);
            renderer.enabled = true;
            if (player != null)
            {
                player.MoverAteDestino();
            }
        }
    }
}
