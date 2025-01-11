using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimatorController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void TakeDamageAnimation()
    {
        animator.SetTrigger("Hit");
    }

    public void DieAnimation()
    {
        animator.SetBool("Dead", true);
    }
}
