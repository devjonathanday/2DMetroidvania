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

    void UpdateMapTile()
    {
        //Accesses the tile background sprite contained at this grid position in the map
        spriteRenderer.sprite = MapController.instance.TileBackgrounds[MapController.instance.map[mapPosX, mapPosY].backgroundID];
    }
}