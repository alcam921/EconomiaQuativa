using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuatiPositionManager : MonoBehaviour
{
    public static QuatiPositionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
     public void SavePosition(string mapaNome, Vector3 pos)
    {
        PlayerPrefs.SetFloat($"{mapaNome}_quatiPosX", pos.x);
        PlayerPrefs.SetFloat($"{mapaNome}_quatiPosY", pos.y);
        PlayerPrefs.SetFloat($"{mapaNome}_quatiPosZ", pos.z);
        PlayerPrefs.Save();
    }

    // Retorna a posição salva ou a posição default se não existir
    public Vector3 LoadPosition(string mapaNome, Vector3 defaultPos)
    {
        if (!PlayerPrefs.HasKey($"{mapaNome}_quatiPosX")) return defaultPos;
        float x = PlayerPrefs.GetFloat($"{mapaNome}_quatiPosX");
        float y = PlayerPrefs.GetFloat($"{mapaNome}_quatiPosY");
        float z = PlayerPrefs.GetFloat($"{mapaNome}_quatiPosZ");
        return new Vector3(x, y, z);
    }
}
