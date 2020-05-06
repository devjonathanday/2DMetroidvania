using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    public void DestroySelf()
    {
        animator.SetTrigger("Destroy");
    }
    public void Terminate()
    {
        Destroy(gameObject);
    }
}