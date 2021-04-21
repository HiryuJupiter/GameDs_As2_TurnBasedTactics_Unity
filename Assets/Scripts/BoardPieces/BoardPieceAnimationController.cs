using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoardPieceAnimationController : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            PlayIdle();

        if (Input.GetKeyDown(KeyCode.W))
            PlayMoving();

        if (Input.GetKeyDown(KeyCode.E))
            PlayAttack();

        if (Input.GetKeyDown(KeyCode.R))
            PlayDead();
    }

    void PlayIdle ()
    {
        animator.Play("Idle");
    }
    void PlayDead()
    {
        animator.Play("Dead");
    }

    void PlayMoving()
    {
        animator.Play("Moving");
    }

    void PlayAttack()
    {
        animator.Play("Attack");
    }
}
