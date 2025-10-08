using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapaTeleport : MonoBehaviour
{
    private bool canTeleport;
    [SerializeField] private string sceneName;
    [SerializeField] private Animator animator;
    void Start()
    {
        StartCoroutine(TurnOn());
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canTeleport)
        {
           StartCoroutine(ImFaded());
        }
    }

    public IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(0.5f);
        canTeleport = true;
    }
    public IEnumerator ImFaded()
    {
        FadeHandler.fade?.Invoke();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
