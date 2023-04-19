using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lists;
    [SerializeField] private bool isReading = false;

    public void ShowList(int listId)
    {
        HideLists();
        _lists[listId].SetActive(true);
        isReading = true;
    }
    
    public void HideLists()
    {
        foreach (var list in _lists)
        {
            list.SetActive(false);
        }
        isReading = false;
    }

}
