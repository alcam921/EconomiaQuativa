using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public static CameraZoom Instance;
    private Vector3 originalPos;
    private float originalSize;

    private Camera cam;
    private Coroutine zoomCoroutine;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
        originalPos = cam.transform.position;
        originalSize = cam.orthographicSize;
    }

    public void ZoomTo(Vector3 targetPos)
    {
        // se já estiver rodando um zoom, para
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(SmoothZoom(
            new Vector3(targetPos.x, targetPos.y, cam.transform.position.z),
            2.5f, // tamanho do zoom
            0.5f  // duração da transição em segundos
        ));
    }

    public void ResetZoom()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(SmoothZoom(originalPos, originalSize, 0.5f));
    }

    private IEnumerator SmoothZoom(Vector3 targetPos, float targetSize, float duration)
    {
        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration); // suaviza curva

            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        cam.transform.position = targetPos;
        cam.orthographicSize = targetSize;
        zoomCoroutine = null;
    }
}
