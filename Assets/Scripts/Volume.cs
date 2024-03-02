using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public static Volume Instance;
    
    private Bus _masterBus; // unused
    private Bus _musicBas;  // music slider
    private Bus _sfxBus;    // sfx slider
    private Bus _dialogBus; // sfx slider
    
    private void Awake()
    {
        Instance = this;
        
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBas = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _dialogBus = RuntimeManager.GetBus("bus:/Dialog");
    }

    private void Start()
    {
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        _musicBas.setVolume(PlayerPrefs.GetFloat("music", 1f));
        _sfxBus.setVolume(PlayerPrefs.GetFloat("sfx", 1f));
        _dialogBus.setVolume(PlayerPrefs.GetFloat("sfx", 1f));
    }
    
    
}
