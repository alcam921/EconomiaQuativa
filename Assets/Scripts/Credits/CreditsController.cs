using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    public static CreditsController Instance;

    #region Inspector Fields
    [Header("UI")]
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private RectTransform counterBox;
    [SerializeField] private CanvasGroup counterCanvas;

    [Header("Configuração")]
    [SerializeField] private List<CharacterInteraction> characters;

    [Header("Animação")]
    [SerializeField] private float animDuration = 0.5f;

    // Âncoras no estilo "padding" (quanto sobra dos lados)
    [SerializeField] private float tutorialAnchorPadding = 0.2f;
    [SerializeField] private Vector2 tutorialAnchorPaddingY = new Vector2(0.2f, 0.2f);
    [SerializeField] private float counterAnchorPadding = 0f;
    [SerializeField] private Vector2 counterAnchorPaddingY = new Vector2(0.2f, 0.2f);

    [Header("Fim do Jogo")]

    [SerializeField] private ParticleSystem confettiLeft;
    [SerializeField] private ParticleSystem confettiRight;
    [SerializeField] private Button[] buttons;
    #endregion

    #region Private Fields
    private bool isPopupOpen = false;
    private int foundCount = 0;
    private bool tutorialActive = true;
    private bool gameEnded = false;
    public bool GameEnded => gameEnded;
    #endregion

    #region Unity Methods
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.enabled = false;
        }
        ApplyAnchor(counterBox, tutorialAnchorPadding, tutorialAnchorPaddingY);

        if (counterText != null)
        {
            counterText.text =
                "Clique nos personagens!\nSeu objetivo é encontrar todas as pessoas que fizeram parte desse projeto.";
        }
    }
    #endregion

    #region Popup Control
    public bool CanInteract() => !isPopupOpen;
    public void OnPopupOpened() => isPopupOpen = true;
    public void OnPopupClosed() => isPopupOpen = false;
    #endregion

    #region Progress
    public void RegisterFound()
    {
        foundCount++;

        if (tutorialActive)
        {
            tutorialActive = false;
            StartCoroutine(TransitionToCounter());
        }
        else
        {
            UpdateCounter();
        }

        if (foundCount >= characters.Count)
        {
            UpdateCounter();
            StartCoroutine(EndCreditsDelayed());
        }
    }
    private IEnumerator EndCreditsDelayed()
    {
        yield return new WaitForSeconds(1f);
        EndCredits();
    }
    #endregion

    #region Animation
    private IEnumerator TransitionToCounter()
    {
        yield return FadeOut();

        UpdateCounter();

        float startX = tutorialAnchorPadding;
        float endX = counterAnchorPadding;
        Vector2 manualY = counterAnchorPaddingY;

        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animDuration;

            float currentX = Mathf.Lerp(startX, endX, t);
            ApplyAnchor(counterBox, currentX, manualY);

            yield return null;
        }

        ApplyAnchor(counterBox, endX, manualY);

        yield return FadeIn();
    }

    private IEnumerator TransitionToEnd()
    {
        yield return FadeOut();

        if (counterText != null)
        {
            counterText.text =
                "Parabéns!\nVocê encontrou todos os participantes e desenvolvedores do projeto Economia Quativa!";
        }

        // âncoras do estado atual (counter) → até as do estado final (end)
        float startX = counterAnchorPadding;
        float endX = 0.2f; // pode ajustar aqui se quiser sobra lateral

        Vector2 startY = counterAnchorPaddingY;
        Vector2 endY = new Vector2(0.8f, 0.95f); // valores finais de âncora vertical (ajusta como preferir)

        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animDuration;

            float currentX = Mathf.Lerp(startX, endX, t);
            Vector2 currentY = Vector2.Lerp(startY, endY, t);

            ApplyAnchor(counterBox, currentX, currentY);

            yield return null;
        }

        ApplyAnchor(counterBox, endX, endY);

        yield return FadeIn();
        if (confettiLeft != null) confettiLeft.Play();
        if (confettiRight != null) confettiRight.Play();
    }
    #endregion

    #region Helpers
    private void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = $"Encontrados: {foundCount}/{characters.Count}";
    }

    private void ApplyAnchor(RectTransform rect, float paddingX, Vector2 manualY)
    {
        if (rect == null) return;

        float minX = paddingX;
        float maxX = 1f - paddingX;

        float minY = manualY.x;
        float maxY = manualY.y;

        rect.anchorMin = new Vector2(minX, minY);
        rect.anchorMax = new Vector2(maxX, maxY);
        rect.anchoredPosition = Vector2.zero;
    }

    private IEnumerator FadeOut()
    {
        if (counterCanvas == null) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (animDuration / 2f);
            counterCanvas.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        counterCanvas.alpha = 0f;
    }

    private IEnumerator FadeIn()
    {
        if (counterCanvas == null) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (animDuration / 2f);
            counterCanvas.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        counterCanvas.alpha = 1f;
    }
    #endregion

    #region End Game
    public void EndCredits()
    {
        gameEnded = true;
        StartCoroutine(TransitionToEnd());
        foreach (Button btn in buttons)
        {
            btn.enabled = true;
        }
    }
    #endregion
}
