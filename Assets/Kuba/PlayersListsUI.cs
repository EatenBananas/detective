using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersListsUI : MonoBehaviour
{
    [SerializeField] private List<Button> _playersLists;
    [SerializeField] private ListsManager _listManager;
    private Button _click;

    private void Start()
    {

        for (int i = 0; i < _playersLists.Count; i++)
        {
            int playerIndex = i;
            _playersLists[playerIndex].onClick.AddListener (() => OnButtonListImageUIClick(playerIndex));
        }
    }
    public void OnButtonListImageUIClick(int listId) => _listManager.ShowList(listId);
}