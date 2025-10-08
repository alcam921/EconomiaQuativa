using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ManagerVolume : MonoBehaviour
{
    [Header("Especificações")]
    [SerializeField] private AudioMixer _audio;
    //[SerializeField] private Slider _geralS;
    [SerializeField] private Slider _musicaS;
    [SerializeField] private Slider _efeitoS;

    public const string MUSICA = "MusicaVol";
    public const string EFEITOS = "EfeitosVol";
    //public const string GERAL = "MasterVol";

    void Awake()
    {
        //_geralS.onValueChanged.AddListener(MasterVol);
        _musicaS.onValueChanged.AddListener(MusicaVol);
        _efeitoS.onValueChanged.AddListener(EfeitosVol);
    }
    void Start()
    {
        //_geralS.value = PlayerPrefs.GetFloat(SoundManager.Geral, 1f);
        _musicaS.value = PlayerPrefs.GetFloat(SoundManager.Musica, 1f);
        _efeitoS.value = PlayerPrefs.GetFloat(SoundManager.EfeitosSonoros, 1f);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(SoundManager.Musica, _musicaS.value);
        PlayerPrefs.SetFloat(SoundManager.EfeitosSonoros, _efeitoS.value);
        //PlayerPrefs.SetFloat(SoundManager.Geral, _geralS.value);
    }

    public void MusicaVol(float config)
    {
        float volume = config > 0 ? Mathf.Log10(config) * 20 : -80f;

        _audio.SetFloat("MusicaVol", volume);
    }
    public void EfeitosVol(float config)
    {
        float volume = config > 0 ? Mathf.Log10(config) * 20 : -80f;
        _audio.SetFloat("EfeitosVol", volume);
    }

    public void MasterVol(float config)
    {
        float volume = config > 0 ? Mathf.Log10(config) * 20 : -80f;
        _audio.SetFloat("MasterVol", volume);
    }

    public void SampleSfx(AudioSource audio)
    {
        if (!audio.isPlaying)
        {

        }
    }
}