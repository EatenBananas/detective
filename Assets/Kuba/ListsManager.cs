using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lists;

    public void ShowList(int listId)
    {
        HideLists();
        _lists[listId].SetActive(true);
    }
    
    public void HideLists()
    {
        foreach (var list in _lists)
        {
            list.SetActive(false);
        }
    }

}
