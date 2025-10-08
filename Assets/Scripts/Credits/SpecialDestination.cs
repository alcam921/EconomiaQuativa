using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecialDestination : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ParticleSystem[] fireEffect;
    [SerializeField] private ParticleSystem smokeEffect;

    [Header("Configuração de voo")]
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float rotationAngle = 25f;
    [SerializeField] private float zoomOutSize = 8f;
    [SerializeField] private float zoomSpeed = 1.5f;

    [Header("Fases de voo")]
    [SerializeField] private float verticalRise = 2f;
    [SerializeField] private float forwardAdvance = 3f;
    [SerializeField] private float tiltSpeed = 2f;

    [Header("Canvas")]
    [SerializeField] private Canvas scenario;
    [SerializeField] private Canvas paralax;

    [Header("Background Céu")]
    [SerializeField] private Image skyImage;
    [SerializeField] private float skyFadeDelay = 1f;
    [SerializeField] private float skyFadeDuration = 3f;

    private bool _isFlying = false;
    private bool isEnding = false;

    public void StartSequence(PointInfo info, Transform destino, Transform trem)
    {
        if (!_isFlying)
            StartCoroutine(FlySequence(info, destino, trem));
    }

    private IEnumerator FlySequence(PointInfo info, Transform destino, Transform trem)
    {
        _isFlying = true;

        // Ativa partículas
        if (fireEffect != null)
            foreach (var particle in fireEffect) particle.gameObject.SetActive(true);

        if (smokeEffect) smokeEffect.gameObject.SetActive(false);

        // Ajusta câmera
        float initialSize = mainCamera.orthographicSize;
        mainCamera.GetComponent<FollowingTrainCamera>().limiteCima = destino.position.y;

        // ====== FASE 1: SUBIDA ======
        yield return StartCoroutine(VerticalRise(trem, initialSize));

        // ====== FASE 2: AVANÇO RETO ======
        Vector3 forwardTarget = trem.position + Vector3.right * forwardAdvance;
        yield return StartCoroutine(MoveStraight(trem, forwardTarget, initialSize));

        // ====== FASE 3: DIAGONAL ======
        yield return StartCoroutine(MoveDiagonal(trem, forwardTarget, destino, info, initialSize));
    }

    private IEnumerator VerticalRise(Transform trem, float initialSize)
    {
        Vector3 riseTarget = trem.position + Vector3.up * verticalRise;
        float elapsedZoom = 0f;

        scenario.renderMode = RenderMode.WorldSpace;
        mainCamera.GetComponent<FollowingTrainCamera>().offset.x = 0f;

        while (Vector3.Distance(trem.position, riseTarget) > 0.05f)
        {
            trem.position = Vector3.MoveTowards(trem.position, riseTarget, flySpeed * Time.deltaTime);

            elapsedZoom += Time.deltaTime * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, zoomOutSize, elapsedZoom);

            yield return null;
        }
    }

    private IEnumerator MoveStraight(Transform trem, Vector3 forwardTarget, float initialSize)
    {
        float elapsedZoom = 0f;

        while (Vector3.Distance(trem.position, forwardTarget) > 0.05f)
        {
            trem.position = Vector3.MoveTowards(trem.position, forwardTarget, flySpeed * Time.deltaTime);

            elapsedZoom += Time.deltaTime * zoomSpeed;
            paralax.renderMode = RenderMode.WorldSpace;
            // mainCamera.orthographicSize = Mathf.Lerp(initialSize, zoomOutSize, elapsedZoom);

            yield return null;
        }
    }

    private IEnumerator MoveDiagonal(Transform trem, Vector3 forwardTarget, Transform destino, PointInfo info, float initialSize)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAngle);
        Vector3 targetPos = destino.position;
        float totalDist = Vector3.Distance(forwardTarget, targetPos);

        mainCamera.GetComponent<FollowingTrainCamera>().offset.y = 0f;

        if (skyImage != null)
            StartCoroutine(FadeInSky());

        while (Vector3.Distance(trem.position, targetPos) > 0.05f)
        {
            trem.position = Vector3.MoveTowards(trem.position, targetPos, flySpeed * Time.deltaTime);
            trem.rotation = Quaternion.Slerp(trem.rotation, targetRotation, Time.deltaTime * tiltSpeed);

            // Se já passou de 20% do caminho → começa fade e troca de cena
            float distPerc = Vector3.Distance(trem.position, forwardTarget) / totalDist;
            if (distPerc >= 0.2f && !isEnding)
            {
                isEnding = true;
                StartCoroutine(ChangeScene(info)); // 🚀 roda paralelo, trem continua
            }

            yield return null;
        }
    }

    private IEnumerator FadeInSky()
    {
        skyImage.color = new Color(1f, 1f, 1f, 0f);

        yield return new WaitForSeconds(skyFadeDelay);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / skyFadeDuration;
            skyImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, t));

            yield return null;
        }
    }

    private IEnumerator ChangeScene(PointInfo info)
    {
        FadeHandler.fade?.Invoke();
        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetString("lastScene", info.scene);
        SceneManager.LoadScene(info.scene);
    }
}
