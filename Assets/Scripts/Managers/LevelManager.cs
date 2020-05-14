using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [Header("Boundaries")]
    public Transform topLeftBounds = null;
    public Transform bottomRightBounds = null;

    [Header("Visuals")]
    [SerializeField] float screenFadeDuration = 0;

    [System.Serializable]
    public class SpawnPoint
    {
        public string name;
        public Vector3 spawnLocation;
        public Vector3 cameraStartPos;
        public Door door;
    }

    public SpawnPoint[] spawnPoints = null;

    void Awake()
    {
        //Assign self as static LevelManager instance
        if (instance == null) instance = this;
    }

    void Start()
    {
        PlayerManager.instance.facingRight = GameManager.instance.playerFacingRight;

        FadeIn();
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

    IEnumerator FadeInCoroutine()
    {
        CameraControls.instance.FadeScreen(1);

        for (float t = 1; t > 0; t -= Time.deltaTime / screenFadeDuration)
        {
            CameraControls.instance.FadeScreen(t);
            yield return null;
        }

        CameraControls.instance.FadeScreen(0);
    }

    IEnumerator FadeOutCoroutine(string queuedScene)
    {
        GameManager.instance.playerFacingRight = PlayerManager.instance.facingRight;

        CameraControls.instance.FadeScreen(0);

        for (float t = 0; t < 1; t += Time.deltaTime / screenFadeDuration)
        {
            CameraControls.instance.FadeScreen(t);
            yield return null;
        }

        CameraControls.instance.FadeScreen(1);

        SceneManager.LoadScene(queuedScene);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }
    public void FadeOut(string queuedScene)
    {
        StartCoroutine(FadeOutCoroutine(queuedScene));
    }
}