using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager = null;

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
        public Weapon.WeaponTypes type = Weapon.WeaponTypes.DEFAULT;
        [HideInInspector] public Weapon weaponData = null;
    }

    void Start()
    {
        #region Initialize Weapon Components

        for (int i = 0; i < weaponTypes.Length; i++)
        {
            weaponTypes[i].weaponData = weaponTypes[i].prefab.GetComponent<Weapon>();

            if (weaponTypes[i].weaponData == null)
            {
                Debug.LogError("Weapon prefab for " + name + " is missing a Weapon component!");
                return;
            }
        }

        #endregion
    }

    void Update()
    {
        aimInput.x = Mathf.RoundToInt(InputHandler.instance.player.GetAxis("AimX"));
        aimInput.y = Mathf.RoundToInt(InputHandler.instance.player.GetAxis("AimY"));
        //aimInput.Normalize();

        Aim();

        weaponTypes[currentWeapon].weaponData.UpdateUsageConditions();
        if (weaponTypes[currentWeapon].weaponData.usageConditions) UseWeapon();
    }

    void Aim()
    {
        //Rotate arm to face aiming direction
        if (aimInput.x > 0) playerManager.facingRight = true;
        else if (aimInput.x < 0) playerManager.facingRight = false;
        else if(aimInput.x == 0 && aimInput.y == 0) aimInput = playerManager.facingRight ? Vector2.right : Vector2.left;

        aimAngle = Mathf.Rad2Deg * Mathf.Atan2(aimInput.y, Mathf.Abs(aimInput.x));
        arm.localEulerAngles = Vector3.forward * aimAngle;

        UpdateAnimations(false);
    }

    void UseWeapon()
    {
        switch (weaponTypes[currentWeapon].type)
        {
            default:
            case Weapon.WeaponTypes.DEFAULT:
                break;
            case Weapon.WeaponTypes.PROJECTILE:
                {
                    GameObject weaponInstance = Instantiate(weaponTypes[currentWeapon].prefab,
                                                            firePoint.position + (firePoint.right * (playerManager.facingRight ? 1 : -1) * weaponTypes[currentWeapon].weaponData.spawnOffset),
                                                            Quaternion.Euler(Vector3.forward * (Mathf.Rad2Deg * Mathf.Atan2(aimInput.y, aimInput.x))));
                    weaponInstance.GetComponent<Weapon>().Initialize(firePoint.right * (playerManager.facingRight ? 1 : -1));
                }
                break;
            case Weapon.WeaponTypes.PHYSICS:
                {

                }
                break;
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