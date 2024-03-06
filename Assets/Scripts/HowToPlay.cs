using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private Button _button;
    private void Start()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("new", 1) == 1);
        _button.onClick.AddListener(()=>gameObject.SetActive(false));
    }
}
