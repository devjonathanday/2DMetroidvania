using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] MapController.TransitionDirection transitionDirection;
    [SerializeField] string queuedSceneName = string.Empty;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LevelManager.instance.FadeOut(queuedSceneName);
        }
    }
}