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
    [SerializeField] [ReadOnlyField] Vector3 aimDirection = Vector3.zero;

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
        public AudioClip shootSFX = null;
        public bool autoFire;
        public float fireIncrement;
    }

    void Awake()
    {
        //Assigns the main input handler to player 0, since there will only be one player
        inputHandler = Rewired.ReInput.players.GetPlayer(0);
    }

    void Update()
    {
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
        if (inputHandler.GetAxis("AimX") != 0 || inputHandler.GetAxis("AimY") != 0)
        {
            if (inputHandler.GetAxis("AimX") > 0)
            {
                playerManager.facingRight = true;
            }
            if (inputHandler.GetAxis("AimX") < 0)
            {
                playerManager.facingRight = false;
            }

            aimDirection = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(inputHandler.GetAxis("AimY"), Mathf.Abs(inputHandler.GetAxis("AimX"))));

            arm.localEulerAngles = aimDirection;

            UpdateAnimations(false);
        }
        else
        {
            arm.localEulerAngles = Vector3.zero;
            UpdateAnimations(true);
        }
    }

    void Shoot()
    {
        GameObject spawnedBullet = Instantiate(bulletTypes[currentBullet].bulletPrefab,
                                               firePoint.position + (firePoint.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].spawnOffset),
                                               Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(inputHandler.GetAxis("AimY"), inputHandler.GetAxis("AimX"))));
        //spawnedBullet.transform.localScale = firePoint.lossyScale;
        spawnedBullet.GetComponent<Bullet>().Initialize(firePoint.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].speed);
        if(bulletTypes[currentBullet].shootSFX != null)
        {
            GlobalAudio.instance.PlayOneShot(bulletTypes[currentBullet].shootSFX);
        }
    }

    void UpdateAnimations(bool forward)
    {
        //Change head sprite based on aiming direction
        if (forward)
        {
            headSprite.sprite = headForward;
        }
        else
        {
            if (inputHandler.GetAxis("AimY") > 0.5f)
            {
                headSprite.sprite = headUp;
            }
            else if (inputHandler.GetAxis("AimY") < -0.5f)
            {
                headSprite.sprite = headDown;
            }
            else headSprite.sprite = headForward;
        }
    }
}