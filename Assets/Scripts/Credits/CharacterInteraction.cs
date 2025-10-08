using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterInteraction : MonoBehaviour
{
    public CreditAnimationData data; // O scriptable com informações do personagem
    public GameObject checkMark;     // Referência ao check desativado
    public GameObject popUpPrefab;   // Prefab do pop-up
    public GameObject checkPrefab;       // Prefab do checkzinho    
    public Transform checkTarget;

    private bool isFound = false;
    private SpriteRenderer spriteRenderer;
    private Coroutine idleLoopCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (checkMark != null)
            checkMark.SetActive(false);

        // Começa o loop de idle
        idleLoopCoroutine = StartCoroutine(IdleLoop());
    }

    public void OnMouseDown()
    {

        if (isFound && !CreditsController.Instance.GameEnded) return;
        if (!CreditsController.Instance.CanInteract())
            return;

        // verifica se pode interagir

        // bloqueia os outros até popup fechar
        CreditsController.Instance.OnPopupOpened();

        // Para o idle
        if (idleLoopCoroutine != null)
            StopCoroutine(idleLoopCoroutine);

        // Começa a animação de revelar
        StartCoroutine(PlayRevealSequence());

    }

    IEnumerator IdleLoop()
    {
        int index = 0;
        while (true)
        {
            if (data.idleImages.Length > 0)
            {
                spriteRenderer.sprite = data.idleImages[index];
                index = (index + 1) % data.idleImages.Length;
            }
            yield return new WaitForSeconds(0.1f); // velocidade do idle
        }
    }

    IEnumerator PlayRevealSequence()
    {
        // Zoom da câmera
        CameraZoom.Instance.ZoomTo(transform.position);

        // Reproduz sequência de "remover máscara"
        for (int i = 0; i < data.removingMaskImages.Length; i++)
        {
            spriteRenderer.sprite = data.removingMaskImages[i];
            yield return new WaitForSeconds(0.1f); // velocidade de transição dos frames
        }

        // Espera um pouquinho depois da animação
        yield return new WaitForSeconds(0.3f);

        // Mostra o pop-up
        // GameObject popUp = Instantiate(popUpPrefab, FindObjectOfType<Canvas>().transform);
        // popUp.GetComponent<CreditPopup>().Setup(data, OnPopupClosed);
        if (FindObjectOfType<CreditPopup>() == null)
        {
            GameObject popUp = Instantiate(popUpPrefab, FindObjectOfType<Canvas>().transform);
            popUp.GetComponent<CreditPopup>().Setup(data, OnPopupClosed);
        }
    }

    public void OnPopupClosed()
    {
        isFound = true;

        // Sempre mostra o check
        if (checkPrefab != null && checkTarget != null)
        {
            GameObject flyCheck = Instantiate(checkPrefab, transform.position, Quaternion.identity);
            StartCoroutine(MoveCheckToTarget(flyCheck, checkTarget.GetComponent<RectTransform>().position));
        }

        // Se o jogo acabou, apenas não faz zoom novamente
        if (CreditsController.Instance.GameEnded)
        {
            CameraZoom.Instance.ResetZoom();
            return;
        }

        // Volta a câmera
        CameraZoom.Instance.ResetZoom();
    }
    IEnumerator MoveCheckToTarget(GameObject flyCheck, Vector3 targetPos)
    {
        float duration = 0.8f;
        float time = 0f;

        Vector3 startPos = flyCheck.transform.position;

        while (time < duration)
        {
            flyCheck.transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        flyCheck.transform.position = targetPos;
        Destroy(flyCheck);
        if (checkMark != null)
            checkMark.SetActive(true);
    }
}
