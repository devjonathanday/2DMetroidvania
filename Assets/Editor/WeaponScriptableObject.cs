using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScriptableObject : ScriptableObject
{
    public float damage;
    public float lifeTime;
    public AudioClip spawnSFX;
}

[CreateAssetMenu(fileName = "NewProjectileWeapon", menuName = "Weapon/ProjectileWeapon", order = 1)]
public class ProjectileWeapon : WeaponScriptableObject
{
    public float velocity;
}