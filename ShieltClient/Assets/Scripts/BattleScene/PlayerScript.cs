using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public interface IPlayerActions
{
    public void Idle();
    public void Attack();
    public void PowerAttack();
    public void Defend();
    public void PowerDefend();
    public void Die();
}

public class PlayerScript : MonoBehaviour, IPlayerActions
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void Idle()
    {
        animator.SetTrigger("idle");
    }
    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void Defend()
    {
        animator.SetTrigger("defend");
    }

    public void Die()
    {
        animator.SetTrigger("die");
    }

    public void PowerAttack()
    {
        animator.SetTrigger("powerAttack");
    }

    public void PowerDefend()
    {
        animator.SetTrigger("powerDefend");
    }
}
