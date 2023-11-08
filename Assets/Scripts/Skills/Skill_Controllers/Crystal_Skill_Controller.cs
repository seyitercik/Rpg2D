using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private float crystalExistTimer;
    private float moveSpeed;
    private bool canExplode;
    private bool canMove;
    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTarget;
    
    public void SetupCrystal(float _crytalDuration,float _moveSpeed,bool _canExplode, bool _canMove,Transform _closestTarget)
    {
        crystalExistTimer = _crytalDuration;
        moveSpeed = _moveSpeed;
        canExplode = _canExplode;
        canMove = _canMove;
        closestTarget = _closestTarget;

    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer <= 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position,
                moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
                
        }
            

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3),
                growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy.Enemy>() != null)
                hit.GetComponent<Enemy.Enemy>().Damage();
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
            
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}