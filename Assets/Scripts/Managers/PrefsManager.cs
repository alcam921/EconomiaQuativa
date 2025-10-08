using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CampoGrande")
        {
            PlayerPrefs.SetInt("indexCidade", 0);
        } else if (SceneManager.GetActiveScene().name == "Dourados") 
        {
            PlayerPrefs.SetInt("indexCidade", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Bonito")
        {
            PlayerPrefs.SetInt("indexCidade", 2);
        }
        else if (SceneManager.GetActiveScene().name == "Corumba")
        {
            PlayerPrefs.SetInt("indexCidade", 3);
        }
        else if (SceneManager.GetActiveScene().name == "TresLagoas")
        {
            PlayerPrefs.SetInt("indexCidade", 4);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
