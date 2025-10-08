using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public int id;
    public Sprite frente;
    public Sprite verso;

    private Image image;
    private Button button;
    private bool estaVirada = false;
    public delegate void CardFlip();
    public static event CardFlip cardFlip;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        transform.localScale = new Vector3(0.9f, 1f, 1f);
        MostrarVerso();
        button.onClick.AddListener(() => Virar());
    }

    public void MostrarVerso()
    {
        image.sprite = verso;
        estaVirada = false;
        button.interactable = true;
    }

    public void MostrarFrente()
    {
        image.sprite = frente;
        estaVirada = true;
        button.interactable = false;
    }

    public void Virar()
    {
        if (!estaVirada && MemoryGameController.instance.podeSelecionar)
            StartCoroutine(AnimarVirar());
            
    }

    public IEnumerator AnimarVirar()
    {
        MemoryGameController.instance.podeSelecionar = false;
        cardFlip?.Invoke();
        // Gira até metade
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }

        MostrarFrente();

        // Gira de volta até 1
        for (float i = 0; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }

        MemoryGameController.instance.CartaSelecionada(this);
    }

    public IEnumerator AnimarVirarVerso()
    {
        // Gira até metade
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }

        MostrarVerso();

        // Gira de volta até normal
        for (float i = 0; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator MoverPara(Vector3 novaPosicao, float duracao = 0.5f)
    {
        Vector3 posInicial = transform.position;
        float tempo = 0f;

        while (tempo < duracao)
        {
            transform.position = Vector3.Lerp(posInicial, novaPosicao, tempo / duracao);
            tempo += Time.deltaTime;
            yield return null;
        }

        transform.position = novaPosicao;
    }


}
