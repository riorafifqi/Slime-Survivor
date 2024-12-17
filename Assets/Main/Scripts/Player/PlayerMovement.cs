using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats stats;
    Joystick joystick;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isMoving;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
    }

    private void Move()
    {
        Vector2 moveTarget = new Vector2(transform.position.x + joystick.Horizontal * stats.Speed, transform.position.y + joystick.Vertical * stats.Speed);
        transform.position = Vector2.MoveTowards(transform.position, moveTarget, stats.Speed * Time.deltaTime);

        isMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;
        RunAnimator();
    }

    private void RunAnimator()
    {
        if (!isMoving) 
        {
            animator.SetBool("isMoving", false);
            return;
        }

        animator.SetBool("isMoving", true);

        if (joystick.Horizontal < 0)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }
    }
}
