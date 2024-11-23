using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public interface IPlayerActions
{
    public void Attack();
    public void Defend();
    public void Die();
}

public class PlayerScript : MonoBehaviour, IPlayerActions
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        this.Die();
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
}
