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

    [Header("Abilities")]
    [SerializeField] BulletType[] bulletTypes = new BulletType[1];
    [SerializeField] int currentBullet = 0;

    [System.Serializable]
    public class BulletType
    {
        public string name = null;
        public GameObject bulletPrefab = null;
        public float speed;
        public float spawnOffset;
    }

    void Awake()
    {
        //Assigns the main input handler to player 0, since there will only be one player
        inputHandler = Rewired.ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (inputHandler.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject spawnedBullet = Instantiate(bulletTypes[currentBullet].bulletPrefab,
                                               firePoint.position + (Vector3.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].spawnOffset),
                                               Quaternion.identity);
        spawnedBullet.transform.localScale = firePoint.lossyScale;
        spawnedBullet.GetComponent<Bullet>().Initialize(firePoint.right * (playerManager.facingRight ? 1 : -1) * bulletTypes[currentBullet].speed);
    }
}