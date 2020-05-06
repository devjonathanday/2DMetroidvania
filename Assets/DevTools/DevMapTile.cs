using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMapTile : MonoBehaviour
{
    public int background = 0;
    public bool discovered = false;
    public SpriteRenderer spriteRenderer = null;
    public int mapPosX, mapPosY;
    public MapController.ExitDoor doors;

    public GameObject leftDoor = null;
    public GameObject rightDoor = null;
    public GameObject upDoor = null;
    public GameObject downDoor = null;

    public void UpdateMapTile()
    {
        //Accesses the tile background sprite contained at this grid position in the map
        spriteRenderer.sprite = MapController.instance.TileBackgrounds[MapController.instance.map[mapPosX, mapPosY].backgroundID];
        //Do the same thing with doors
        doors = MapController.instance.map[mapPosX, mapPosY].doors;

        leftDoor.SetActive(doors.HasFlag(MapController.ExitDoor.Left));
        rightDoor.SetActive(doors.HasFlag(MapController.ExitDoor.Right));
        upDoor.SetActive(doors.HasFlag(MapController.ExitDoor.Up));
        downDoor.SetActive(doors.HasFlag(MapController.ExitDoor.Down));
    }
}