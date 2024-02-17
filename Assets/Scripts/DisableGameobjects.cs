using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameobjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    
    private void OnEnable(){
        foreach (var go in _gameObjects)
        {
            go.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var go in _gameObjects)
        {
            go.SetActive(true);
        }
    }
}
