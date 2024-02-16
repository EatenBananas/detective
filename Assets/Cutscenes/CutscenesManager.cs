using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> cutscenesList;

    public void TurnOnCutsceneX(int ccX)
    {
        ccX -= 1;
        foreach (var cutscene in cutscenesList)
        {
            cutscenesList[ccX].SetActive(false);
            if (cutscene == cutscenesList[ccX])
                cutscenesList[ccX].SetActive(true);
            Debug.Log(cutscenesList[ccX]);
        }
    }
    private void Start()
    {
        TurnOnCutsceneX(1);
    }
}
