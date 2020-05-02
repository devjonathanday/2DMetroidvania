using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    [Header("Reference Variables")]
    [SerializeField] float zPosition = 0;
    [Tooltip("The size of the camera in pixels, default is 320 x 180.")]
    [SerializeField] Vector2 size = Vector2.zero;
    Vector3 desiredPosition = Vector3.zero;
    Vector3 finalPosition = Vector3.zero;
    [Tooltip("Number of decimal places used to round the final camera position.")]
    [SerializeField] int decimalPrecision = 0;
    //Cached variable for decimalPrecision, to avoid unnecessary Mathf.Pow() operations.
    float precision = 0;

    [Header("Gameplay Parameters")]
    [SerializeField] float positionLerp = 0;
    [SerializeField] float minimumTranslation = 0;
    [SerializeField] Transform focus = null;

    [Header("Visuals")]
    [SerializeField] RawImage screenFader = null;
    Color screenFaderColor = Color.black;

    void Start()
    {
        precision = Mathf.Pow(10, decimalPrecision);
    }

    void FixedUpdate()
    {
        //Set the desired position to the focus position, clamped to the level bounds
        desiredPosition.x = Mathf.Clamp(focus.position.x, LevelManager.instance.topLeftBounds.position.x + (size.x / 2), LevelManager.instance.bottomRightBounds.position.x - (size.x / 2));
        desiredPosition.y = Mathf.Clamp(focus.position.y, LevelManager.instance.bottomRightBounds.position.y + (size.y / 2), LevelManager.instance.topLeftBounds.position.y - (size.y / 2));

        //Set the final position to lerp between the current and desired positions
        finalPosition = Vector3.Lerp(transform.position, desiredPosition, positionLerp);
        finalPosition = new Vector3(Mathf.Round(finalPosition.x * precision) / precision, Mathf.Round(finalPosition.y * precision) / precision, zPosition);

        //Set the transform position to the final position
        transform.position = finalPosition;
    }

    public void FadeScreen(float amount)
    {
        screenFaderColor.a = amount;
        screenFader.color = screenFaderColor;
    }
}