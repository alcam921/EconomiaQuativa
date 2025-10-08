using System.Collections;
using UnityEngine;

public class CheckFly : MonoBehaviour
{
    public float duration = 2f;

    public void FlyTo(Transform target, System.Action onComplete = null)
    {
        StartCoroutine(FlyRoutine(target, onComplete));
    }

    IEnumerator FlyRoutine(Transform target, System.Action onComplete)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 startPos = rect.position;
        Vector3 endPos = target.position;

        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // curva suave (ease out)
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);

            rect.position = Vector3.Lerp(startPos, endPos, eased);
            rect.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, eased);

            yield return null;
        }

        rect.position = endPos;
        onComplete?.Invoke();

        // destruir o check depois que chegou
        Destroy(gameObject);
    }
}
