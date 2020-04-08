using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerControls : MonoBehaviour
{
    [Header("Input")]
    Player inputHandler = null; //Represents a player to which a controller is assigned

    [Header("Physics")]
    [SerializeField] Vector2 velocity = Vector2.zero;
    [SerializeField] float gravity = 0;
    [SerializeField] Rigidbody2D rBody = null;

    void Start()
    {

    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        velocity += Vector2.down * gravity;
        rBody.position += velocity;
    }
}