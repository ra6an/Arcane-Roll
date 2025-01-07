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
        Debug.Log("Postavljamo animaciju attack");
        animator.SetTrigger("Attack");
    }

    public void TakeDamageAnimation()
    {
        animator.SetTrigger("Hit");
        Debug.Log("Postavljamo animaciju hit");
    }

    public void DieAnimation()
    {
        Debug.Log("Postavljamo animaciju die");
        animator.SetBool("Dead", true);
    }
}
