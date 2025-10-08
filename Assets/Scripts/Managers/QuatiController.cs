using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuatiController : MonoBehaviour
{
    [SerializeField] private string mapaAtual; // CampoGrande, Dourados, Bonito, TresLagoas, Corumba
    [SerializeField] private Vector3 posicaoPadrao = Vector3.zero;
    [SerializeField] private Transform _target;

    private void Start()
    {
        // Carrega a posição salva para o mapa atual
        PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
        Vector3 posSalva = QuatiPositionManager.instance.LoadPosition(mapaAtual, posicaoPadrao);
        _target.position = posSalva;
    }

    // Chame este método sempre que quiser salvar a posição atual do Quati (exemplo: ao trocar de cena)
    public void SalvarPosicaoAtual()
    {
        QuatiPositionManager.instance.SavePosition(mapaAtual, _target.position);
    }
}
