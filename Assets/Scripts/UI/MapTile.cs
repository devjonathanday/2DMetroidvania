using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [Header("Door Images")]
    [SerializeField] GameObject leftDoor = null;
    [SerializeField] GameObject rightDoor = null;
    [SerializeField] GameObject upDoor = null;
    [SerializeField] GameObject downDoor = null;

    public void UpdateFromData(MapController.MapTile tile)
    {
        leftDoor.SetActive(tile.doors.HasFlag(MapController.ExitDoor.Left));
        rightDoor.SetActive(tile.doors.HasFlag(MapController.ExitDoor.Right));
        upDoor.SetActive(tile.doors.HasFlag(MapController.ExitDoor.Up));
        downDoor.SetActive(tile.doors.HasFlag(MapController.ExitDoor.Down));
    }
}