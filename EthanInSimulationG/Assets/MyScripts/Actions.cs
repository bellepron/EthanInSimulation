using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        animator.SetFloat("Speed", 0f);
    }
    public void Walk()
    {
        animator.SetFloat("Speed", 0.5f);
    }
    public void Run()
    {
        animator.SetFloat("Speed", 1f);
    }

    public void Jump()
    {
        animator.SetFloat("Speed", 0f);
        //animator.SetTrigger("Jump");
        animator.SetTrigger("JumpTrigger");
    }
    public void Fall()
    {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Fall");
    }
    public void Sitting(bool v)
    {
        //bool v = !animator.GetBool("Sitting");
        animator.SetBool("Sitting", v);
    }

    public void Thumble()
    {
        animator.SetTrigger("Thumble");
    }

    public void Laser()
    {
        animator.SetTrigger("Laser");
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void Attack2()
    {
        animator.SetTrigger("Attack2");
    }
    public void NEW()
    {
        animator.SetTrigger("NEW");
    }
    public void RRR()
    {
        animator.SetTrigger("RRR");
    }

    public void Damaged()
    {
        //animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Damaged");
    }

    //archer
    public void Aiming(bool v)
    {
        // if (v == true)
        //     transform.Rotate(0, 90, 0);

        //transform.Rotate(0, 90, 0);
        animator.SetBool("Aiming", v);
    }
    public void DropArrow()
    {
        animator.SetTrigger("DropArrow");
    }
    public void DrawArrow()
    {
        animator.SetTrigger("DrawArrow");
    }
}
