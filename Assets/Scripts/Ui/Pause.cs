using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public void BackToScene(GameObject canvas ){
        Player.On?.Invoke(true);
        canvas.SetActive(false);
    }
    public void OpenPause(GameObject canvas ){
        Player.On?.Invoke(false);
        canvas.SetActive(true);
    }
    public void ChangeScene(){
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("lastScene")) == false)
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("lastScene"));
        } else
        {
            SceneManager.LoadScene("Mapa");
        }
    }
}
