using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player.Player  player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _direction, float _gravityScale)
    {
        rb.velocity = _direction;
        rb.gravityScale = _gravityScale;
    }
}
