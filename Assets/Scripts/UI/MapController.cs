using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [System.Serializable]
    public class MapTile
    {
        bool discovered;
        RawImage outline;
    }

    public MapTile[,] mapArray = new MapTile[10,10];

    void Start()
    {

    }

    void Update()
    {

    }
}