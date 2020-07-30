using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    //Represents a player to which a controller is assigned
    public Rewired.Player player = null;

    private void Awake()
    {
        if (instance == null) instance = this;

        //Assigns the main input handler to player 0, since there will only be one player
        player = Rewired.ReInput.players.GetPlayer(0);
    }
}
