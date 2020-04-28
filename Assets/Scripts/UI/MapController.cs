using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    [Flags]
    public enum ExitDoor
    {
        None  = 0, //0000
        Left  = 1, //0001
        Right = 2, //0010
        Up    = 4, //0100
        Down  = 8  //1000
    }

    [System.Serializable]
    public class MapTile
    {
        public int backgroundID;
        public bool discovered;
        public ExitDoor doors;
    }

    public List<Sprite> TileBackgrounds = new List<Sprite>();
    //ROW-MAJOR
    public MapTile[,] map;
    public int mapWidth = 0;
    public int mapHeight = 0;

    void Awake()
    {
        //Ensure only one MapController instance exists in the game
        MapController[] mapControllers = FindObjectsOfType<MapController>();
        if (mapControllers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Assign self as static MapController instance
        if (instance == null) instance = this;
    }

    void SaveMapData()
    {
        string mapDataPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName +
                             @"\Local\" + Application.productName + @"\mapData.csv";

        Debug.Log("Saving map data to file at " + mapDataPath);

        StreamWriter fileWriter = File.CreateText(mapDataPath);

        for (int i = 0; i < mapHeight; i++)
        {
            for (int k = 0; k < mapWidth; k++)
            {
                fileWriter.Write(GetSerializedTile(map[k, i]));
            }
            //Append a space to the end of each line, except the last
            if (i < mapHeight - 1) fileWriter.Write('\n');
        }

        fileWriter.Flush();
        fileWriter.Close();

        string GetSerializedTile(MapTile tile)
        {
            string output = string.Empty;

            //Append a 0 to the beginning of the tile background ID if single digit
            if (tile.backgroundID < 10) output += "0" + tile.backgroundID.ToString();
            else output += tile.backgroundID.ToString();

            output += tile.discovered ? "1" : "0";

            output += tile.doors.HasFlag(ExitDoor.Left) ? "1" : "0";
            output += tile.doors.HasFlag(ExitDoor.Right) ? "1" : "0";
            output += tile.doors.HasFlag(ExitDoor.Up) ? "1" : "0";
            output += tile.doors.HasFlag(ExitDoor.Down) ? "1" : "0";

            output += ",";

            return output;
        }
    }

    void LoadMapData()
    {
        string mapDataPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName +
                             @"\Local\" + Application.productName + @"\mapData.csv";

        if (!File.Exists(mapDataPath))
        {
            //Initialize a clean map file if the player has no save data.
            InitializeMapFile();
        }
        else
        {
            try
            {
                StreamReader fileReader = File.OpenText(mapDataPath);

                //row = iterator for map Y axis
                //column = iterator for map X axis
                //chunks = array of each tile in the current row

                int row = 0;
                string currentLine;
                while ((currentLine = fileReader.ReadLine()) != null)
                {
                    string[] chunks = currentLine.Split(',');
                    for (int column = 0; column < chunks.Length; column++)
                    {
                        //Characters 0-1 represent the tile background index
                        map[column, row].backgroundID = int.Parse(new string(chunks[column][0], chunks[column][1]));
                        //Character 2 represents discovered status (0 = false, 1 = true)
                        map[column, row].discovered = (chunks[column][2] == '0' ? false : true);
                        //Characters 3-6 represent doors in this tile (Left, Right, Up, Down)
                        if (chunks[column][3] == '1') map[column, row].doors |= ExitDoor.Left;
                        if (chunks[column][4] == '1') map[column, row].doors |= ExitDoor.Right;
                        if (chunks[column][5] == '1') map[column, row].doors |= ExitDoor.Up;
                        if (chunks[column][6] == '1') map[column, row].doors |= ExitDoor.Down;
                    }
                }

                fileReader.Close();
            }
            catch
            {
                Debug.Log("Map data file was modified or corrupted, could not load.");
            }
        }
    }

    //TODO: Before final build, mapData.csv is created, then the final map data pass should be written to it.
    //This file does not exist after a clean download, this function should only be run if the player has zero save data.
    void InitializeMapFile()
    {
        string mapDataPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName +
                             @"\Local\" + Application.productName + @"\mapData.csv";

        StreamWriter fileWriter = File.CreateText(mapDataPath);

        fileWriter.Write("");

        fileWriter.Flush();
        fileWriter.Close();
    }
}