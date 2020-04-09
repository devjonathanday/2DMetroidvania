using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
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
        GameObject spawnedBullet = Instantiate(bulletTypes[currentBullet].bulletPrefab, firePoint.position, Quaternion.identity);
        spawnedBullet.transform.localScale = firePoint.localScale;
        spawnedBullet.GetComponent<Bullet>().Initialize(firePoint.right * bulletTypes[currentBullet].speed);
    }
}