using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("collected", true);
    }
}
