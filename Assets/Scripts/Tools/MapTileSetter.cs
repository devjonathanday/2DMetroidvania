using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a collider that sets the player's map tile position, used for multi-tile scenes.
public class MapTileSetter : MonoBehaviour
{
    Vector2 mapCoordinate = Vector2.zero;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MapController.instance.currentTile = mapCoordinate;
        }
    }
}