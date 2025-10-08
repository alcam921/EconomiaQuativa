using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Especificações")]
    [SerializeField] private AudioMixer _mixer;

    public const string Musica = "MusicaVol";
    public const string EfeitosSonoros = "EfeitosVol";
    public const string Geral = "MasterVol";

    private float _mV;
    private float _sfxV;
    private float _mainV;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void JogadorVol()
    {
        _mV = PlayerPrefs.GetFloat(Musica, 1f);
        _sfxV = PlayerPrefs.GetFloat(EfeitosSonoros, 1f);
        _mainV = PlayerPrefs.GetFloat(Geral, 1f);

        _mixer.SetFloat(ManagerVolume.MUSICA, Mathf.Log10(_mV) * 20);
        _mixer.SetFloat(ManagerVolume.EFEITOS, Mathf.Log10(_sfxV) * 20);
        //_mixer.SetFloat(ManagerVolume.GERAL, Mathf.Log10(_mainV) * 20);
    }
}