using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxGroup : MonoBehaviour
{
    [Header("All backgrounds should be positioned relative to the camera.")]
    [SerializeField] Camera cam = null;
    [System.Serializable]
    public class Background
    {
        public Transform transform = null;
        [Tooltip("If set to 1, follows camera position exactly.")]
        public float offsetMultiplier = 0;
        [HideInInspector] public Vector3 originalPosition = Vector3.zero;
    }
    public Background[] backgrounds = new Background[1];

    void Awake()
    {
        InitializeBackgroundPositions();
        UpdateBackgroundPositions();
    }

    void Update()
    {
        UpdateBackgroundPositions();
    }

    //TODO Test if this function would work better as a class constructor, rather than a function
    void InitializeBackgroundPositions()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].originalPosition = backgrounds[i].transform.position - cam.transform.position;
        }
    }

    void UpdateBackgroundPositions()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = (backgrounds[i].originalPosition + cam.transform.position) * backgrounds[i].offsetMultiplier;
        }
    }
}