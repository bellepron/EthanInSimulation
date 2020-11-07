using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Fall()
    {
        //animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Falled");
    }
    public void Damaged()
    {
        animator.SetTrigger("Damaged");
    }

    public void Jump()
    {
        animator.SetFloat("Speed", 0f);
    }
}
