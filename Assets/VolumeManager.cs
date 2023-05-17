using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)] 
    public float masterVolume = 1;
    [Range(0, 1)] 
    public float musicVolume = 1;
    [Range(0, 1)] 
    public float sfxVolume = 1;
    [Range(0, 1)] 
    public float dialogVolume = 1;


    private Bus _masterBus;
    private Bus _musicBas;
    private Bus _sfxBus;
    private Bus _dialogBus;

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _dialogSlider;

    public enum _volumeType
    {
        MASTER,
        MUSIC,
        SFX,
        DIALOG
    }

    [Header("Type")] [SerializeField] public _volumeType volumeType;


    private void Awake()
    {
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBas = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _dialogBus = RuntimeManager.GetBus("bus:/Dialog");
    }

    void Update()
    {
        _masterBus.setVolume(masterVolume);
        _musicBas.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
        _dialogBus.setVolume(dialogVolume);
    }

    public void OnSliderValueChanged(_volumeType volumeType)
    {
        switch (volumeType)
        {
            case _volumeType.MASTER:
                masterVolume = _masterSlider.value;
                break;
            case _volumeType.MUSIC:
                musicVolume = _musicSlider.value;
                break;
            case _volumeType.SFX:
                sfxVolume = _sfxSlider.value;
                break;
            case _volumeType.DIALOG:
                dialogVolume = _dialogSlider.value;
                break;
        }
    }
    
}
