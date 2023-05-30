using System;
using System.Collections;
using System.Collections.Generic;
using Equipment;
using Interactions;
using Interactions.Elements;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI [] _itemButtonTexts;
    [SerializeField] private Color _selectedColor = Color.red;
    [SerializeField] private RectTransform _cursorIcon;
    [SerializeField] private Image _cursorSprite;

    private bool _isCursorIconActive;
    
    private void Awake()
    {
        Instance = this;
        Debug.Assert(_interactableText != null, "_interactableText != null");
        Debug.Assert(_dialoguePanel != null, "_dialoguePanel != null");
        Debug.Assert(_dialogueNpcName != null, "_dialogueNpcName != null");
        Debug.Assert(_dialogueText != null, "_dialogueText != null");
        Debug.Assert(_optionsPanel != null, "_optionsPanel != null");
        Debug.Assert(_optionsText != null, "_optionsText != null");
    }

    private void Start()
    {
        HideDialogue();
        HideOptions();
        
        _cursorIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isCursorIconActive)
        {
            UpdateCursor();
        }
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

    public void ReloadEquipment(List<Item> items, int activeSlot)
    {
        foreach (var buttonText in _itemButtonTexts)
        {
            buttonText.text = string.Empty;
        }
        
        for (int i = 0; i < items.Count && i < _itemButtonTexts.Length; i++)
        {
            _itemButtonTexts[i].text = items[i].ItemName;
            _itemButtonTexts[i].color = i == activeSlot ? _selectedColor : Color.black;
        }

        _isCursorIconActive = activeSlot >= 0;
        _cursorIcon.gameObject.SetActive(_isCursorIconActive);
        if (_isCursorIconActive)
        {
            _cursorSprite.sprite = items[activeSlot].Icon;
        }

    }

    public void HideEquipment() => _equipmentPanel.SetActive(false);

    public void ShowEquipment() => _equipmentPanel.SetActive(true);

    public void HideOptions() => _optionsPanel.SetActive(false);

    private void UpdateCursor()
    {
        _cursorIcon.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var obj = hit.transform.gameObject;
                Debug.Log($"HIT {obj.name}");

                var interaction = obj.GetComponent<ItemInteraction>();
                if (interaction != null)
                {
                    EquipmentManager.Instance.Use(interaction);
                }
                
                // temp
                EquipmentManager.Instance.Select(-1);
            }
        }
    }
}
