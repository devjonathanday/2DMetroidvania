using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [Header("Boundaries")]
    public Transform topLeftBounds = null;
    public Transform bottomRightBounds = null;

    [Header("Visuals")]
    [SerializeField] CameraControls cam = null;
    [SerializeField] float screenFadeDuration = 0;

    void Awake()
    {
        //Assign self as static LevelManager instance
        if (instance == null) instance = this;

        StartCoroutine(FadeIn());
    }

    void OnDrawGizmos()
    {
        if (topLeftBounds != null)
        {
            Debug.DrawLine(topLeftBounds.position, topLeftBounds.position + (Vector3.right * 10000), Color.red);
            Debug.DrawLine(topLeftBounds.position, topLeftBounds.position + (Vector3.down * 10000), Color.red);
        }
        if (bottomRightBounds != null)
        {
            Debug.DrawLine(bottomRightBounds.position, bottomRightBounds.position + (Vector3.up * 10000), Color.red);
            Debug.DrawLine(bottomRightBounds.position, bottomRightBounds.position + (Vector3.left * 10000), Color.red);
        }
    }

    public IEnumerator FadeIn()
    {
        cam.FadeScreen(1);

        for (float t = 1; t > 0; t -= Time.deltaTime / screenFadeDuration)
        {
            cam.FadeScreen(t);
            yield return null;
        }

        cam.FadeScreen(0);
    }

    public IEnumerator FadeOut()
    {
        cam.FadeScreen(0);

        for (float t = 0; t < 1; t += Time.deltaTime / screenFadeDuration)
        {
            cam.FadeScreen(t);
            yield return null;
        }

        cam.FadeScreen(1);

        //Transition to queued scene
    }
}