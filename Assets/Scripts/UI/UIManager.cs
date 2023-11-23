using System;
using System.Collections;
using System.Collections.Generic;
using Equipment;
using Interactions;
using Interactions.Elements;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Interactions")]
    [SerializeField] private TextMeshProUGUI _interactableText;
    [Header("Dialogues")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueNpcName;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [Header("Choices")] 
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private TextMeshProUGUI _optionsText;
    [Header("Equipment")]
    [SerializeField] private GameObject _equipmentPanel;
    [SerializeField] private Image[] _equipmentItems;
    [SerializeField] private Button _foldEqButton;
    [SerializeField] private GameObject _foldEqIcon;
    [SerializeField] private GameObject _unfoldEqIcon;
    [SerializeField] private float _foldEqOffset = 200f;
    [SerializeField] private Color _selectedColor = Color.red;
    [SerializeField] private RectTransform _cursorIcon;

    [Header("Pie Menu")] 
    [SerializeField] private GameObject _pieMenuPanel;
    [SerializeField] private PieMenu _pieMenu;

    private bool _eqFolded = false;
    
    private void Awake()
    {
        Instance = this;
        Debug.Assert(_interactableText != null, "_interactableText != null");
        Debug.Assert(_dialoguePanel != null, "_dialoguePanel != null");
        Debug.Assert(_dialogueNpcName != null, "_dialogueNpcName != null");
        Debug.Assert(_dialogueText != null, "_dialogueText != null");
        Debug.Assert(_optionsPanel != null, "_optionsPanel != null");
        Debug.Assert(_optionsText != null, "_optionsText != null");
        Debug.Assert(_pieMenuPanel != null, "_pieMenuPanel != null");
        Debug.Assert(_pieMenu != null, "_pieMenu!= null");
    }

    private void Start()
    {
        HideDialogue();
        HideOptions();
        HidePieMenu();
        
        _cursorIcon.gameObject.SetActive(false);
        _foldEqButton.onClick.AddListener(FoldEqButtonClicked);
    }
    
    public void ShowInteractableText(string text)
    {
        _interactableText.text = text;
    }

    public void HideInteractableText()
    {
        _interactableText.text = string.Empty;
    }

    public void ShowDialogue(DialogueNpc dialogueNpc, string dialogueText)
    {
        _dialoguePanel.SetActive(true);
        _dialogueNpcName.text = dialogueNpc.NpcName;
        _dialogueText.text = dialogueText;
    }

    public void HideDialogue() => _dialoguePanel.SetActive(false);

    public void ShowOptions(List<string> options)
    {
        _optionsPanel.SetActive(true);

        var text = "";
        for (int i = 0; i < options.Count; i++)
        {
            text += $"{i + 1}. {options[i]} \n";
        }

        _optionsText.text = text;
        
    }

    public void ReloadEquipment(List<Item> items)
    {
        foreach (var image in _equipmentItems)
        {
            image.enabled = false;
        }

        for (int i = 0; i < _equipmentItems.Length && i < items.Count; i++)
        {
            _equipmentItems[i].sprite = items[i].Icon;
            _equipmentItems[i].enabled = true;
        }
    }

    public void HideEquipment() => _equipmentPanel.SetActive(false);

    public void ShowEquipment() => _equipmentPanel.SetActive(true);

    public void HideOptions() => _optionsPanel.SetActive(false);

    public void EqItemDropped(int slot)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var obj = hit.transform.gameObject;
            Debug.Log($"HIT {obj.name}");

            var interaction = obj.GetComponent<ItemInteraction>();
            if (interaction != null)
            {
                EquipmentManager.Instance.Use(interaction, slot);
            }
        }
        EquipmentManager.Instance.ReloadEquipment();
    }

    public void ShowPieMenu(List<PieMenuOption> options)
    {
        _pieMenu.Show(options);
        _pieMenuPanel.SetActive(true);
        HideEquipment();
        HideInteractableText();
    }

    public void HidePieMenu()
    {
        _pieMenuPanel.SetActive(false);
    }

    private void FoldEqButtonClicked()
    {
        var offset = _eqFolded ? -_foldEqOffset :_foldEqOffset;
        
        var position = _equipmentPanel.transform.position;
        position = new Vector3(position.x, position.y - offset, position.z);
        _equipmentPanel.transform.position = position;
        
        _foldEqIcon.SetActive(!_eqFolded);
        _unfoldEqIcon.SetActive(_eqFolded);

        _eqFolded = !_eqFolded;
    }
}
