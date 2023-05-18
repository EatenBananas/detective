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
                MuteBus(_masterBus);
                break;
            case VolumeType.MUSIC:
                MuteBus(_musicBas);
                break;
            case VolumeType.SFX:
                MuteBus(_sfxBus);
                break;
            case VolumeType.DIALOG:
                MuteBus(_dialogBus);
                break;
        }  
    }
    private void MuteBus(Bus bus)
    {
        bus.getVolume(out float volB);
        if (volB == 0)
        {
            bus.setVolume(1);

        }
        else
        {
            bus.setVolume(0);
        }
    }
    
}
