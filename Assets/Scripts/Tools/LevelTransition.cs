using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] MapController.TransitionDirection transitionDirection = new MapController.TransitionDirection();
    [SerializeField] string queuedSceneName = string.Empty;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LevelManager.instance.FadeOut(queuedSceneName);
            PlayerManager.instance.gameObject.SetActive(false);
            GameManager.instance.transitionDirection = transitionDirection;
        }
    }
}