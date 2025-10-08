using UnityEngine;
using System.Collections;

public class Carta : MonoBehaviour
{
    public Sprite imagemFrente;
    public Sprite imagemVerso;
    private SpriteRenderer spriteRenderer;
    public bool estaVirada;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MostrarVerso();
    }

    public void MostrarFrente()
    {
        spriteRenderer.sprite = imagemFrente;
        estaVirada = true;
    }

    public void MostrarVerso()
    {
        spriteRenderer.sprite = imagemVerso;
        estaVirada = false;
    }

    public IEnumerator VirarCarta(float delay)
    {
        // Efeito de virada, encolhendo horizontalmente antes de trocar a imagem
        Vector3 escalaOriginal = transform.localScale;
        transform.localScale = new Vector3(0, escalaOriginal.y, escalaOriginal.z);
        yield return new WaitForSeconds(delay);
        MostrarVerso();  // Troca para o verso da carta
        transform.localScale = escalaOriginal; // Restaura o tamanho original
    }

    private void OnMouseDown()
    {
        if (!estaVirada && GameManager.instance != null)
        {
            MostrarFrente();
            GameManager.instance.CartaSelecionada(this);
        }
    }
}
