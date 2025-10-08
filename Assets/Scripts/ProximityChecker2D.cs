using UnityEngine;

public class ProximityChecker2D : MonoBehaviour
{
    public Transform player;
    public GameObject uiElement;
    public float proximityDistance = 5f; // Dist�ncia para verificar a proximidade

    void Update()
    {
        // Verifica a dist�ncia entre o jogador e o objeto atual
        float distance = Vector2.Distance(transform.position, player.position);

        // Se a dist�ncia for menor ou igual � dist�ncia de proximidade, exibe o UI element
        if (distance <= proximityDistance)
        {
            uiElement.SetActive(true);
        }
        else
        {
            uiElement.SetActive(false);
        }
    }
}
