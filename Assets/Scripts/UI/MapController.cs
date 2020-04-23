using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    public enum ExitDoor
    {
        Left =  (1 << 0),
        Right = (1 << 1),
        Up =    (1 << 2),
        Down =  (1 << 3)
    }

    [System.Serializable]
    public class MapTile
    {
        public bool discovered;
        public int tileBG;
        public ExitDoor doors;
    }

    public List<Sprite> TileBackgrounds = new List<Sprite>();
    //ROW-MAJOR
    public MapTile[,] map;

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
}