using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditPopup : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image studentImage;
    public Image charImage;
    public GameObject frontCardPanel; // frente da carta
    public GameObject backCardImage;  // verso da carta (apenas imagem grande)
    public Button closeButton;

    [Header("Social Buttons")]
    public Button linkedinButton;
    public Button gitButton;
    public Button instaButton;

    [Header("Refs")]
    public RectTransform cardTransform; // esse é o objeto "Card", filho do CardBG

    private System.Action onClose;
    private CreditAnimationData data;
    private int photoClickCount = 0;
    private const int maxClicks = 36;

    void Awake()
    {
        // botão começa desativado
        closeButton.gameObject.SetActive(false);
    }

    public void Setup(CreditAnimationData newData, System.Action onCloseCallback)
    {
        data = newData;
        onClose = onCloseCallback;

        nameText.text = data.studentName;
        descriptionText.text = data.studentRole;
        studentImage.sprite = data.studentImage;
        backCardImage.GetComponent<Image>().sprite = data.cardImage;

        if (data.linkedinURL == "")
            linkedinButton.gameObject.SetActive(false);
        if (data.instaURL == "")
            instaButton.gameObject.SetActive(false);
        if (data.gitURL == "")
            gitButton.gameObject.SetActive(false);
        // Debug.Log("ghjk");
        // Último sprite da animação do NPC
        if (data.removingMaskImages != null && data.removingMaskImages.Length > 0)
            charImage.sprite = data.removingMaskImages[data.removingMaskImages.Length - 1];

        // Configura links
        if (linkedinButton != null)
            linkedinButton.onClick.AddListener(() => OpenURL(data.linkedinURL));
        if (gitButton != null)
            gitButton.onClick.AddListener(() => OpenURL(data.gitURL));
        if (instaButton != null)
            instaButton.onClick.AddListener(() => OpenURL(data.instaURL));

        // Clique 36x na foto -> muda descrição
        if (data.hasDesc)
        {
            Button studentButton = studentImage.GetComponent<Button>();
            if (studentButton != null)
                studentButton.onClick.AddListener(OnStudentImageClick);

        }
        else
        {
            Destroy(studentImage.GetComponent<Button>());
        }

        // Começa animação da carta
        StartCoroutine(PlayCardAnimation());
    }

    IEnumerator PlayCardAnimation()
    {
        // inicial: escala 0
        cardTransform.localScale = Vector3.zero;
        cardTransform.localRotation = Quaternion.identity;

        // mostra verso primeiro
        backCardImage.SetActive(true);
        frontCardPanel.SetActive(false);

        // 1 - crescer até escala 1
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            cardTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        cardTransform.localScale = Vector3.one;

        // 2 - esperar um pouco no verso
        yield return new WaitForSeconds(1f);

        // 3 - flip para frente
        yield return StartCoroutine(FlipCardToFront());

        // 4 - liberar botão de fechar
        yield return new WaitForSeconds(0.5f);
        closeButton.gameObject.SetActive(true);
        closeButton.onClick.AddListener(ClosePopup);
    }

    IEnumerator FlipCardToFront()
    {
        float duration = 0.8f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float y = Mathf.Lerp(0f, -180f, t);
            cardTransform.localRotation = Quaternion.Euler(0, y, 0);

            // troca exatamente na metade
            if (t >= 0.5f && !frontCardPanel.activeSelf)
            {
                backCardImage.SetActive(false);
                frontCardPanel.SetActive(true);
            }

            yield return null;
        }

        // garantir que termina na frente (180 graus)
        cardTransform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void OnStudentImageClick()
    {
        photoClickCount++;
        if (photoClickCount >= maxClicks)
        {
            descriptionText.fontSizeMax = 72f;
            descriptionText.text = data.studentDescription;
        }
    }

    void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }

    void ClosePopup()
    {

        Destroy(gameObject);
        // libera interação global
        CreditsController.Instance.OnPopupClosed();
        if (!CreditsController.Instance.GameEnded)
        {
            CreditsController.Instance.RegisterFound();
            onClose?.Invoke();
        }
    }
}
