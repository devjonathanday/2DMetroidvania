using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager = null;

    [Header("Input")]
    [SerializeField] float movementDeadzone = 0;
    Rewired.Player inputHandler = null; //Represents a player to which a controller is assigned

    //Parameters that the player cares about, directly affects user experience
    [Header("Gameplay Parameters")]
    [SerializeField] float moveAcceleration = 0;
    [SerializeField] float maxMoveSpeed = 0;
    [SerializeField] float jumpForce = 0;
    [SerializeField] float gravity = 0;
    [Tooltip("Buffer on each side of each BoxCast to smooth out collision detection.")]
    [SerializeField] float cornerCollisionBuffer = 0;
    bool jumpReleased = true;

    //Affects the physics response to other objects
    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    [SerializeField] BoxCollider2D boxCollider = null;
    [Space(10)]
    [SerializeField] [ReadOnlyField] bool grounded = true;
    [SerializeField] [ReadOnlyField] Vector2 velocity = Vector2.zero;
    [SerializeField] float groundDrag = 0;
    [SerializeField] float terminalVelocity = 0;
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    enum BoxCastDirection { LEFT, RIGHT, UP, DOWN };
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[1];

    [Header("Animation")]
    [SerializeField] Animator animator = null;
    [SerializeField] Transform sprite = null;
    [SerializeField] Vector3 doubleJumpEffectOffset = Vector2.zero;

    [Header("Audio")]
    [SerializeField] float landSFXMinVelocity = 0;

    void Awake()
    {
        //Assigns the main input handler to player 0, since there will only be one player
        inputHandler = Rewired.ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        grounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.extents * 2, 0, Vector3.down, contactFilter, castResults, 1) > 0;
        if (grounded)
        {
            playerManager.doubleJumpUsed = false;
        }

        if(inputHandler.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                Jump(false);
                jumpReleased = false;
            }
            else if (!playerManager.doubleJumpUsed && GameManager.instance.CheckUpgradeUnlocked("DoubleJump"))
            {
                Jump(true);
                jumpReleased = false;
                playerManager.doubleJumpUsed = true;
            }
        }
        if (inputHandler.GetButtonUp("Jump") && !jumpReleased && velocity.y > 0)
        {
            velocity.y /= 2;
            jumpReleased = true;
        }

        //If we are not aiming
        if (inputHandler.GetAxis("AimX") == 0 && inputHandler.GetAxis("AimY") == 0)
        {
            //Aim the player based on their movement instead
            if (inputHandler.GetAxis("Move") < -movementDeadzone)
                playerManager.facingRight = false;
            if (inputHandler.GetAxis("Move") > movementDeadzone)
                playerManager.facingRight = true;
        }

        if (playerManager.facingRight == true)
            sprite.localScale = Vector3.one;
        else sprite.localScale = new Vector3(-1, 1, 1);

        UpdateAnimations();
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

        if ((!inputHandler.GetButton("Stabilize") || !grounded) && Mathf.Abs(inputHandler.GetAxis("Move")) > movementDeadzone)
        {
            Move();
        }
    }

    void ApplyConstraints()
    {
        //Max falling speed
        if (velocity.y < -terminalVelocity)
            velocity.y = -terminalVelocity;

        //Slow down the player if they are on the ground and not pressing anything, or if they are trying to stabilize
        if (grounded && (Mathf.Abs(inputHandler.GetAxis("Move")) < movementDeadzone || inputHandler.GetButton("Stabilize")))
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
                            rBody.position = new Vector2(castResults[0].point.x + boxCollider.bounds.extents.x, rBody.position.y);
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
                            rBody.position = new Vector2(castResults[0].point.x - boxCollider.bounds.extents.x, rBody.position.y);
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
                            rBody.position = new Vector2(rBody.position.x, castResults[0].point.y - boxCollider.bounds.extents.y);
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
                            rBody.position = new Vector2(rBody.position.x, castResults[0].point.y + boxCollider.bounds.extents.y);

                            if (velocity.y < -landSFXMinVelocity)
                                GlobalAudio.instance.PlayOneShot(playerManager.landSFX);

                            velocity.y = 0;
                        }
                    }
                }
                break;
        }
    }

    #endregion

    #region Player Input Actions

    void Move()
    {
        velocity.x += inputHandler.GetAxis("Move") * moveAcceleration * Time.deltaTime;
    }

    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    void Jump(bool isDoubleJump)
    {
        velocity.y = jumpForce;
        if(isDoubleJump)
        {
            GlobalAudio.instance.PlayOneShot(playerManager.doubleJumpSFX);
            GameObject doubleJumpEffect = Instantiate(playerManager.doubleJumpEffectPrefab, sprite);
            doubleJumpEffect.transform.localPosition = doubleJumpEffectOffset;
        }
        else
        {
            GlobalAudio.instance.PlayOneShot(playerManager.jumpSFX);
        }
    }

    #endregion

    #region Animation

    void UpdateAnimations()
    {
        animator.SetFloat("SpeedX", Mathf.Abs(velocity.x));
        animator.SetFloat("SpeedY", velocity.y);
        animator.SetBool("Grounded", grounded);
    }

    #endregion
}