using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameobjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private bool _invert = false;
    
    private void OnEnable(){
        foreach (var go in _gameObjects)
        {
            go.SetActive(_invert);
        }
    }

    private void OnDisable()
    {
        foreach (var go in _gameObjects)
        {
            go.SetActive(!_invert);
        }
    }
}
