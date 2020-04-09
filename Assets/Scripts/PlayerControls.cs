using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerControls : MonoBehaviour
{
    [Header("Input")]
    Player inputHandler = null; //Represents a player to which a controller is assigned

    [Header("Gameplay Parameters")]
    [SerializeField] float moveAcceleration = 0;
    [SerializeField] float maxMoveSpeed = 0;
    [SerializeField] float jumpForce = 0;
    [SerializeField] float terminalVelocity = 0;

    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    [SerializeField] BoxCollider2D boxCollider = null;
    [Space(10)]
    [SerializeField] [ReadOnlyField] bool grounded = true;
    [SerializeField] [ReadOnlyField] Vector2 velocity = Vector2.zero;
    [SerializeField] float gravity = 0;
    [SerializeField] float groundDrag = 0;
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    [Tooltip("Buffer on each side of each BoxCast to smooth out collision detection.")]
    [SerializeField] float cornerCollisionBuffer = 0;
    enum BoxCastDirection { LEFT, RIGHT, UP, DOWN };
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[1];

    void Awake()
    {
        //Assigns the main input handler to player 0, since there will only be one player.
        inputHandler = ReInput.players.GetPlayer(0);
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        grounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.extents * 2, 0, Vector3.down, contactFilter, castResults, 1) > 0;

        if (grounded && inputHandler.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        ApplyLocomotion();
        ApplyConstraints();

        rBody.position += velocity;
    }

    #region Physics Functions

    void ApplyLocomotion()
    {
        velocity.y -= gravity * Time.deltaTime;

        velocity.x += inputHandler.GetAxisRaw("Move") * moveAcceleration * Time.deltaTime;
    }

    void ApplyConstraints()
    {
        //Max falling speed
        if (velocity.y < -terminalVelocity)
            velocity.y = -terminalVelocity;

        //Slow down the player if they are on the ground and not pressing anything
        if (grounded && inputHandler.GetAxis("Move") == 0)
            velocity.x *= groundDrag;

        //Max horizontal movement speed
        if (velocity.x < -maxMoveSpeed)
            velocity.x = -maxMoveSpeed;
        if (velocity.x > maxMoveSpeed)
            velocity.x = maxMoveSpeed;

        //Checks collision based on your velocity, then snaps to the nearest edge hit
        if (velocity.y > 0) VelocityCollisionCheck(BoxCastDirection.UP, velocity.y);
        else if (velocity.y < 0) VelocityCollisionCheck(BoxCastDirection.DOWN, velocity.y);
        if (velocity.x > 0) VelocityCollisionCheck(BoxCastDirection.RIGHT, velocity.x);
        else if (velocity.x < 0) VelocityCollisionCheck(BoxCastDirection.LEFT, velocity.x);
    }

    void VelocityCollisionCheck(BoxCastDirection direction, float distance)
    {
        switch (direction)
        {
            case BoxCastDirection.LEFT:
                {
                    if (Physics2D.BoxCast(boxCollider.bounds.center, (boxCollider.bounds.extents * 2) - (Vector3.up * cornerCollisionBuffer * 2),
                        0, Vector2.left, contactFilter, castResults, Mathf.Abs(distance)) > 0)
                    {
                        if (castResults[0].point.x < boxCollider.bounds.center.x)
                        {
                            rBody.position = new Vector2(castResults[0].point.x + (boxCollider.bounds.extents.x), rBody.position.y);
                            velocity.x = 0;
                        }
                    }
                }
                break;
            case BoxCastDirection.RIGHT:
                {
                    if (Physics2D.BoxCast(boxCollider.bounds.center, (boxCollider.bounds.extents * 2) - (Vector3.up * cornerCollisionBuffer * 2),
                        0, Vector2.right, contactFilter, castResults, Mathf.Abs(distance)) > 0)
                    {
                        if (castResults[0].point.x > boxCollider.bounds.center.x)
                        {
                            rBody.position = new Vector2(castResults[0].point.x - (boxCollider.bounds.extents.x), rBody.position.y);
                            velocity.x = 0;
                        }
                    }
                }
                break;
            case BoxCastDirection.UP:
                {
                    if (Physics2D.BoxCast(boxCollider.bounds.center, (boxCollider.bounds.extents * 2) - (Vector3.right * cornerCollisionBuffer * 2),
                        0, Vector2.up, contactFilter, castResults, Mathf.Abs(distance)) > 0)
                    {
                        if (castResults[0].point.y > boxCollider.bounds.center.y)
                        {
                            rBody.position = new Vector2(rBody.position.x, castResults[0].point.y - (boxCollider.bounds.extents.y));
                            velocity.y = 0;
                        }
                    }
                }
                break;
            case BoxCastDirection.DOWN:
                {
                    if (Physics2D.BoxCast(boxCollider.bounds.center, (boxCollider.bounds.extents * 2) - (Vector3.right * cornerCollisionBuffer * 2),
                        0, Vector2.down, contactFilter, castResults, Mathf.Abs(distance)) > 0)
                    {
                        if (castResults[0].point.y < boxCollider.bounds.center.y)
                        {
                            rBody.position = new Vector2(rBody.position.x, castResults[0].point.y + (boxCollider.bounds.extents.y));
                            velocity.y = 0;
                        }
                    }
                }
                break;
        }
    }

    #endregion

    #region Player Input Actions

    void Jump()
    {
        velocity.y = jumpForce;
    }

    #endregion
}