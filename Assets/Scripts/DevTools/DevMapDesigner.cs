using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMapDesigner : MonoBehaviour
{
    public GameObject devMapTile;
    public int mapWidth, mapHeight;
    public Vector2 mapTileSize;
    DevMapTile[,] mapCopy;

    void Start()
    {
        mapCopy = new DevMapTile[mapWidth, mapHeight];
        CreateMapGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateMapData();
        }
    }

    void CreateMapGrid()
    {
        for (int i = 0; i < mapWidth; i++) //Iterate through rows
        {
            for (int k = 0; k < mapHeight; k++) //Iterate through columns
            {
                DevMapTile newDevMapTile = Instantiate(devMapTile, new Vector3(i * mapTileSize.x, k * mapTileSize.y, 0), Quaternion.identity).GetComponent<DevMapTile>();
                mapCopy[i, k] = newDevMapTile;
                newDevMapTile.mapPosX = i;
                newDevMapTile.mapPosY = k;
            }
        }
    }

    void UpdateMapData()
    {
        for (int i = 0; i < mapWidth; i++) //Iterate through rows
        {
            for (int k = 0; k < mapHeight; k++) //Iterate through columns
            {
                mapCopy[i, k].spriteRenderer.sprite = MapController.instance.TileBackgrounds[mapCopy[i, k].background];
            }
        }
    }
}