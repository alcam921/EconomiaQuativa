using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public void IdaProMapa(string mapa)
    {
        SceneManager.LoadScene(mapa); 
    }
}
