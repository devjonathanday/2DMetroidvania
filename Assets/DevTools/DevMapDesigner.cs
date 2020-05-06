using System;
using System.IO;
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateMapData();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SerializeMapData();
            UpdateMapData();
        }
    }

    void CreateMapGrid()
    {
        for (int i = 0; i < mapWidth; i++) //Iterate through rows
        {
            for (int k = 0; k < mapHeight; k++) //Iterate through columns
            {
                DevMapTile newDevMapTile = Instantiate(devMapTile, new Vector3(i * mapTileSize.x, -k * mapTileSize.y, 0), Quaternion.identity).GetComponent<DevMapTile>();
                mapCopy[i, k] = newDevMapTile;
                newDevMapTile.mapPosX = i;
                newDevMapTile.mapPosY = k;
            }
        }
    }

    void UpdateMapData()
    {
        MapController.instance.LoadMapData();

        for (int i = 0; i < mapWidth; i++) //Iterate through rows
        {
            for (int k = 0; k < mapHeight; k++) //Iterate through columns
            {
                //mapCopy[i, k].background = MapController.instance.map[i, k].backgroundID;
                //mapCopy[i, k].spriteRenderer.sprite = MapController.instance.TileBackgrounds[MapController.instance.map[i, k].backgroundID];
                //mapCopy[i, k].doors = MapController.instance.map[i, k].doors;
                mapCopy[i, k].UpdateMapTile();
            }
        }
    }

    void SerializeMapData()
    {
        string mapDataPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName +
                             @"\Local\" + Application.productName + @"\mapData.csv";
        Debug.Log("Saving map data to file at " + mapDataPath);

        StreamWriter fileWriter = File.CreateText(mapDataPath);

        for (int i = 0; i < mapHeight; i++)
        {
            for (int k = 0; k < mapWidth; k++)
            {
                fileWriter.Write(GetSerializedTile(mapCopy[k, i], k < mapWidth - 1));
            }
            //Append a space to the end of each line, except the last
            if (i < mapHeight - 1) fileWriter.Write('\n');
        }

        fileWriter.Flush();
        fileWriter.Close();
    }

    string GetSerializedTile(DevMapTile tile, bool appendComma)
    {
        string output = string.Empty;

        //Append a 0 to the beginning of the tile background ID if single digit
        if (tile.background < 10) output += "0" + tile.background.ToString();
        else output += tile.background.ToString();

        output += tile.discovered ? "1" : "0";

        output += tile.doors.HasFlag(MapController.ExitDoor.Left) ? "1" : "0";
        output += tile.doors.HasFlag(MapController.ExitDoor.Right) ? "1" : "0";
        output += tile.doors.HasFlag(MapController.ExitDoor.Up) ? "1" : "0";
        output += tile.doors.HasFlag(MapController.ExitDoor.Down) ? "1" : "0";

        if (appendComma) output += ",";

        return output;
    }
}