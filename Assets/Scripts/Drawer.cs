using System;
using System.Collections;
using System.Collections.Generic;
using Equipment;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : MonoBehaviour
{
    [SerializeField] private Button _noteButton;
    [SerializeField] private Item _noteItem;

    private void Start()
    {
        _noteButton.onClick.AddListener(OnNoteButtonClicked);
    }

    private void OnNoteButtonClicked()
    {
        Debug.Log("LETTER");
        _noteButton.gameObject.SetActive(false);
        EquipmentManager.Instance.Equip(_noteItem);
    }
}
