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
    [Header("Volume monitoring")]
    [Range(0, 1)] 
    public float masterVolume = 0;
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

    public enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX,
        DIALOG
    }

    [Header("Type")] [SerializeField] public VolumeType volumeType;


    private void Awake()
    {
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBas = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _dialogBus = RuntimeManager.GetBus("bus:/Dialog");
        ChangeVolumes();
        SetSlidersCorrect();
    }

    public void OnSliderValueChanged(int volumeType)
    {
        switch ((VolumeType)volumeType)
        {
            case VolumeType.MASTER:
                masterVolume = _masterSlider.value;
                break;
            case VolumeType.MUSIC:
                musicVolume = _musicSlider.value;
                break;
            case VolumeType.SFX:
                sfxVolume = _sfxSlider.value;
                break;
            case VolumeType.DIALOG:
                dialogVolume = _dialogSlider.value;
                break;
        }
        ChangeVolumes();
    }
    private void ChangeVolumes()
    {
        _masterBus.setVolume(masterVolume);
        _musicBas.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
        _dialogBus.setVolume(dialogVolume);
    }
    public void SetVolume(int enumNr)
    {
        switch ((VolumeType)enumNr)
        {
            case VolumeType.MASTER:
                MuteBus(enumNr);
                break;
            case VolumeType.MUSIC:
                MuteBus(enumNr);
                break;
            case VolumeType.SFX:
                MuteBus(enumNr);
                break;
            case VolumeType.DIALOG:
                MuteBus(enumNr);
                break;
        }
    }
    private void MuteBus(int muteNr)
    {
        float tempVolume = 0;
        
        switch ((VolumeType)muteNr)
        {
            case VolumeType.MASTER:
                tempVolume = isVolumeMuted(masterVolume);
                masterVolume = tempVolume;
                break;
            case VolumeType.MUSIC:
                tempVolume = isVolumeMuted(musicVolume);
                musicVolume = tempVolume;
                break;
            case VolumeType.SFX:
                tempVolume = isVolumeMuted(sfxVolume);
                sfxVolume = tempVolume;
                break;
            case VolumeType.DIALOG:
                tempVolume = isVolumeMuted(dialogVolume);
                dialogVolume = tempVolume;
                break;
        }
        SetSlidersCorrect();
    }

    private void SetSlidersCorrect()
    {
        _masterSlider.value = masterVolume;
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;
        _dialogSlider.value = dialogVolume;
    }
    private float isVolumeMuted(float volume)
    {
        if (volume == 0)
            return 1;
        return 0;
    }
}
