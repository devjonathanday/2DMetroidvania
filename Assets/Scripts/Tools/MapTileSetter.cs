using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains a collider that sets the player's map tile position, used for multi-tile scenes.
public class MapTileSetter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Set the player's map position to this map tile
        }
    }
}