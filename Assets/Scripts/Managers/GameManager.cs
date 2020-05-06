using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public MapController.TransitionDirection transitionDirection;
    public bool playerFacingRight = true;

    void Awake()
    {
        //Ensure only one GameManager instance exists in the scene
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Assign self as static GameManager instance
        if (instance == null) instance = this;
    }

    void Start()
    {
        PlayerManager.instance.facingRight = playerFacingRight;
    }
}
