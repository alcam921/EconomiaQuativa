using UnityEngine;

public class FadeHandler : MonoBehaviour
{
    public delegate void FadeDelegate();
    public static FadeDelegate fade;
    public delegate void UnFadeDelegate();
    public static UnFadeDelegate unFade;
    [SerializeField] private Animator animator;


    private void OnEnable()
    {
        fade = DoFade;
        unFade = DoUnFade;
    }
    private void OnDisable()
    {
        fade = null;
        unFade = null;
    }
    private void Start()
    {
        DoUnFade();
    }

    public void DoFade()
    {
        animator.Play("Fade");
    }
    public void DoUnFade()
    {
        animator.Play("Unfade");
    }

}
