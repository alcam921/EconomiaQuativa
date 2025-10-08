using UnityEngine;

public class AdicionarCartas : MonoBehaviour
{
    [SerializeField] private Transform espacoPuzzle;
    [SerializeField] private GameObject btn;
    private void Awake()
    {
        for (int i = 0; i < 12; i++) //alterar o numero do segundo argumento altera o n�mero de cartas que ir�o aparecer
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(espacoPuzzle, false);
        }
    }
}
