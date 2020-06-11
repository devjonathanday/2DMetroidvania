using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager = null;

    [Header("Input")]
    Rewired.Player inputHandler = null; //Represents a player to which a controller is assigned

    [Header("Orientation")]
    [SerializeField] Transform firePoint = null;
    [SerializeField] Transform arm = null;
    [SerializeField] [ReadOnlyField] Vector2 aimInput;
    [SerializeField] [ReadOnlyField] float aimAngle;

    [Header("Cosmetic")]
    [SerializeField] SpriteRenderer headSprite = null;
    [SerializeField] Sprite headUp = null;
    [SerializeField] Sprite headForward = null;
    [SerializeField] Sprite headDown = null;

    [Header("Abilities")]
    [SerializeField] BulletType[] bulletTypes = new BulletType[1];
    [SerializeField] int currentBullet = 0;
    float fireTimestamp;

    [System.Serializable]
    public class BulletType
    {
        public string name = null;
        public GameObject bulletPrefab = null;
        public float speed = 0;
        public float spawnOffset = 0;
        public bool autoFire = false;
        public float fireIncrement = 0;
        public float damage = 0;
        public AudioClip shootSFX = null;
        public AudioClip impactSFX = null;
    }

    void Awake()
    {
        //Assigns the main input handler to player 0, since there will only be one player
        inputHandler = Rewired.ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        aimInput.x = Mathf.RoundToInt(inputHandler.GetAxis("AimX"));
        aimInput.y = Mathf.RoundToInt(inputHandler.GetAxis("AimY"));
        //aimInput.Normalize();

        Aim();

        if (Time.time - fireTimestamp > bulletTypes[currentBullet].fireIncrement)
        {
            if (bulletTypes[currentBullet].autoFire && inputHandler.GetButton("Shoot") ||
                !bulletTypes[currentBullet].autoFire && inputHandler.GetButtonDown("Shoot"))
            {
                Shoot();
                fireTimestamp = Time.time;
            }
        }
    }

    void Aim()
    {
        //Rotate arm to face aiming direction
        if (aimInput.x > 0)
        {
            playerManager.facingRight = true;
        }
        else if (aimInput.x < 0)
        {
            playerManager.facingRight = false;
        }
        else if(aimInput.x == 0 && aimInput.y == 0)
        {
            aimInput = playerManager.facingRight ? Vector2.right : Vector2.left;
        }

        aimAngle = Mathf.Rad2Deg * Mathf.Atan2(aimInput.y, Mathf.Abs(aimInput.x));

        arm.localEulerAngles = Vector3.forward * aimAngle;

        UpdateAnimations(false);
    }

    void Shoot()
    {
        GameObject spawnedBullet = Instantiate(bulletTypes[currentBullet].bulletPrefab,
                                               firePoint.position + (firePoint.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].spawnOffset),
                                               Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * Mathf.Atan2(aimInput.y, aimInput.x))));
        spawnedBullet.GetComponent<Bullet>().Initialize(firePoint.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].speed,
                                                        bulletTypes[currentBullet].damage,
                                                        bulletTypes[currentBullet].shootSFX,
                                                        bulletTypes[currentBullet].impactSFX);
        if(bulletTypes[currentBullet].shootSFX != null)
        {
            GlobalAudio.instance.PlayOneShot(bulletTypes[currentBullet].shootSFX);
        }
    }

    void UpdateAnimations(bool forward)
    {
        if (aimInput.y > 0.5f)
        {
            headSprite.sprite = headUp;
        }
        else if (aimInput.y < -0.5f)
        {
            headSprite.sprite = headDown;
        }
        else headSprite.sprite = headForward;
    }
}