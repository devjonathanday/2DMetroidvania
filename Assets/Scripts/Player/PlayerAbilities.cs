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
    [SerializeField] WeaponType[] weaponTypes = new WeaponType[1];
    [SerializeField] int currentWeapon = 0;
    float fireTimestamp;

    [System.Serializable]
    public class WeaponType
    {
        public string name = null;
        public GameObject prefab = null;
        public float speed = 0;
        public float spawnOffset = 0;
        public bool autoFire = false;
        public float fireIncrement = 0;
        public float damage = 0;
        public AudioClip shootSFX = null;
        public AudioClip impactSFX = null;
        public float spawnTorque = 0;
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

        if (Time.time - fireTimestamp > weaponTypes[currentWeapon].fireIncrement)
        {
            if (weaponTypes[currentWeapon].autoFire && inputHandler.GetButton("Shoot") ||
                !weaponTypes[currentWeapon].autoFire && inputHandler.GetButtonDown("Shoot"))
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
        GameObject spawnedBullet = Instantiate(weaponTypes[currentWeapon].prefab,
                                               firePoint.position + (firePoint.right * (playerManager.facingRight ? 1 : -1) * weaponTypes[currentWeapon].spawnOffset),
                                               Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * Mathf.Atan2(aimInput.y, aimInput.x))));
        Bullet bulletComponent = spawnedBullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Initialize(firePoint.right * (playerManager.facingRight ? 1 : -1) * weaponTypes[currentWeapon].speed,
                                                          weaponTypes[currentWeapon].damage,
                                                          weaponTypes[currentWeapon].shootSFX,
                                                          weaponTypes[currentWeapon].impactSFX);
        }
        else
        {
            spawnedBullet.GetComponent<Rigidbody2D>().AddForce(aimInput * weaponTypes[currentWeapon].speed);
            spawnedBullet.GetComponent<Rigidbody2D>().AddTorque(playerManager.facingRight ? -weaponTypes[currentWeapon].spawnTorque : weaponTypes[currentWeapon].spawnTorque);
        }
        if(weaponTypes[currentWeapon].shootSFX != null)
        {
            GlobalAudio.instance.PlayOneShot(weaponTypes[currentWeapon].shootSFX);
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