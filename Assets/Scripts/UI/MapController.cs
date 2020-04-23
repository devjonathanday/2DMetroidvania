using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    enum Door
    {
        Left =  (1 << 0),
        Right = (1 << 1),
        Up =    (1 << 2),
        Down =  (1 << 3)
    }

    [System.Serializable]
    public class MapTile
    {
        bool discovered;
        int tileBG;
        Door doors;
    }

    [SerializeField] static List<Texture2D> TileBackgrounds = new List<Texture2D>();
}