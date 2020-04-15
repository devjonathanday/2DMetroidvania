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

    [Header("Gameplay Parameters")]
    [SerializeField] float positionLerp = 0;
    [SerializeField] float minimumTranslation = 0;
    [SerializeField] Transform focus = null;

    [Header("Visuals")]
    [SerializeField] RawImage screenFader = null;
    Color screenFaderColor = Color.black;

    void FixedUpdate()
    {
        //Set the desired position to the focus position, clamped to the level bounds
        desiredPosition.x = Mathf.Clamp(focus.position.x, LevelManager.instance.topLeftBounds.position.x + (size.x / 2), LevelManager.instance.bottomRightBounds.position.x - (size.x / 2));
        desiredPosition.y = Mathf.Clamp(focus.position.y, LevelManager.instance.bottomRightBounds.position.y + (size.y / 2), LevelManager.instance.topLeftBounds.position.y - (size.y / 2));
        desiredPosition.z = zPosition;

        //Set the final position to lerp between the current and desired positions
        finalPosition = Vector3.Lerp(transform.position, desiredPosition, positionLerp);

        //Set the final position to be the desired position directly if the translation is small enough
        if (Mathf.Abs(desiredPosition.x - finalPosition.x) < minimumTranslation) finalPosition.x = desiredPosition.x;
        if (Mathf.Abs(desiredPosition.y - finalPosition.y) < minimumTranslation) finalPosition.y = desiredPosition.y;

        //Set the transform position to the final position
        transform.position = finalPosition;
    }

    public void FadeScreen(float amount)
    {
        screenFaderColor.a = amount;
        screenFader.color = screenFaderColor;
    }
}