using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Interactions;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [SerializeField] private float _beforePause = 0.5f;
    [SerializeField] private float _afterPause = 0.5f;
    [SerializeField] private float _emergencyPause = 2f;
    
    
    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteDialogue(string path)
    {
        StartCoroutine(Play(path));
    }

    private IEnumerator Play(string path)
    {
        yield return new WaitForSeconds(_beforePause);

        if (string.IsNullOrEmpty(path))
        {
            yield return new WaitForSeconds(_emergencyPause);
        }
        else
        {
            StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
            //emitter.EventReference = EventReference.Find($"event:/{path}");
            emitter.EventReference = FMODUnity.RuntimeManager.PathToEventReference($"event:/{path}");
            emitter.Play();

            while (emitter.IsPlaying())
            {
                yield return null;
            }
        
            Destroy(emitter);
        }
        
        UIManager.Instance.HideDialogue();
        InteractionManager.Instance.CompleteElement();
    }
}
