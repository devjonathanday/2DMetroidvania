using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] int destinationIndex = 0;
    [SerializeField] string queuedSceneName = string.Empty;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager.instance.gameObject.SetActive(false);
            GameManager.instance.destinationIndex = destinationIndex;
            LevelManager.instance.FadeOut(queuedSceneName);
        }
    }
}