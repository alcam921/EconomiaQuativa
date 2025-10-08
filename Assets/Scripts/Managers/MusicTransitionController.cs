using System.Collections;
using UnityEngine;

public class MusicTransitionController : MonoBehaviour
{
    #region PublicFields
    public AudioSource currentAudioSource;
    public AudioSource alternativeAudioSource;
    public float fadeDuration = 1.5f;
    #endregion

    #region PrivateFields
    private Coroutine transitionCoroutine;
    #endregion

    #region Unity

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Para garantir que não fiquem várias corrotinas rodando
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionMusic(currentAudioSource, alternativeAudioSource));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(TransitionMusic(alternativeAudioSource, currentAudioSource));
    }
    #endregion

    #region Private

    private IEnumerator TransitionMusic(AudioSource from, AudioSource to)
    {
        float time = 0f;
        float startFrom = from.volume;
        float startTo = to.volume;
        float targetVolume = PlayerPrefs.GetFloat(SoundManager.Musica, 1f);

        // Garante que a música esteja tocando
        if (!to.isPlaying) to.Play();

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            from.volume = Mathf.Lerp(startFrom, 0f, t);
            to.volume = Mathf.Lerp(startTo, targetVolume, t);

            yield return null;
        }
        from.volume = 0f;
        to.volume = targetVolume;
        from.Pause(); 
    }
    #endregion
}
