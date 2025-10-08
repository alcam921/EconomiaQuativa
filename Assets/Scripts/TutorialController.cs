using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public Animator camAnim;
    public Animator playerAnim;
    public Animator[] startAnim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCamAnimationToEnd());
    }

    private IEnumerator WaitForCamAnimationToEnd()
    {
        // Espera até que a animação NÃO esteja mais no estado atual
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = camAnim.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1f && !camAnim.IsInTransition(0);
        });

        // Agora pode desativar o Animator da câmera
        camAnim.enabled = false;
        StartCoroutine(WaitForPlayerAnimationToEnd());
    }
    private IEnumerator WaitForPlayerAnimationToEnd()
    {
        playerAnim.SetBool("Tutorial", true);
        // Espera até que a animação NÃO esteja mais no estado atual
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1f && !playerAnim.IsInTransition(0);
        });
        playerAnim.SetBool("Tutorial", false);
    }
}
