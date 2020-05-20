using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    public static CameraControls instance;

    [Header("Reference Variables")]
    [SerializeField] float zPosition = 0;
    [Tooltip("The size of the camera in pixels, default is 320 x 180.")]
    [SerializeField] Vector2 size = Vector2.zero;
    //Position of the camera ignoring screen shake
    Vector3 initialPosition = Vector3.zero;
    //Desired position of the camera, tracks focus but stays within boundaries
    Vector3 desiredPosition = Vector3.zero;
    //Final position of the camera after smoothing calculations
    Vector3 finalPosition = Vector3.zero;
    [Tooltip("Number of decimal places used to round the final camera position.")]
    [SerializeField] int decimalPrecision = 0;
    //Cached variable for decimalPrecision, to avoid unnecessary Mathf.Pow() operations.
    float precision = 0;

    [Header("Gameplay Parameters")]
    [SerializeField] float positionLerp = 0;
    [SerializeField] Transform focus = null;

    [Header("Visuals")]
    [SerializeField] RawImage screenFader = null;
    Color screenFaderColor = Color.black;
    //Screen shake variables, x = intensity, y = duration
    [SerializeField] [ReadOnlyField] float screenShake;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        precision = Mathf.Pow(10, decimalPrecision);
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        //Set the desired position to the focus position, clamped to the level bounds
        desiredPosition.x = Mathf.Clamp(focus.position.x, LevelManager.instance.topLeftBounds.position.x + (size.x / 2), LevelManager.instance.bottomRightBounds.position.x - (size.x / 2));
        desiredPosition.y = Mathf.Clamp(focus.position.y, LevelManager.instance.bottomRightBounds.position.y + (size.y / 2), LevelManager.instance.topLeftBounds.position.y - (size.y / 2));

        //Set the final position to lerp between the current and desired positions
        finalPosition = Vector3.Lerp(initialPosition, desiredPosition, positionLerp);
        finalPosition = new Vector3(Mathf.Round(finalPosition.x * precision) / precision, Mathf.Round(finalPosition.y * precision) / precision, zPosition);

        //Set the transform and initial positions to the final position
        transform.position = initialPosition = finalPosition;

        //Add screenShake (if non-zero) to the transform position
        if (screenShake > 0)
            transform.position += new Vector3(Random.Range(-screenShake, screenShake), Random.Range(-screenShake, screenShake), 0);
    }

    public void FadeScreen(float amount)
    {
        screenFaderColor.a = amount;
        screenFader.color = screenFaderColor;
    }

    public void ScreenShake(float intensity, float duration)
    {
        StartCoroutine(ScreenShakeCoroutine(intensity, duration));
    }

    IEnumerator ScreenShakeCoroutine(float intensity, float duration)
    {
        screenShake = intensity;
        for (float t = duration; t > 0; t -= Time.deltaTime)
        {
            screenShake = (t / duration) * intensity;
            yield return null;
        }
        screenShake = 0;
    }
}