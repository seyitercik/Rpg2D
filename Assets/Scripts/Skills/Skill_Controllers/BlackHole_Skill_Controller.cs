using System;
using System.Collections;
using System.Collections.Generic;
using Skills;
using Skills.Skill_Controllers;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    private float maxSize;
    
    private float growSpeed;
    private bool canGrow=true;
    private bool canShrink;
    
    private int amountOfAttack=4;
    private float cloneAttackCoolDown=.3f;
    private float ShirnkSpeed;

    private bool canCreateHotkeys=true;
    private float cloneAttackTimer;
    private bool cloneAttackReleased;
    
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public void SetupBlackhole(float _maxSize,float _growSpeed, float _shirnkSpeed,int _amountOfAttack,float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        ShirnkSpeed = _shirnkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCoolDown = _cloneAttackCooldown;

    }
    
    


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
            
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1),
                ShirnkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotkey();
        cloneAttackReleased = true;
        canCreateHotkeys = false;
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;
            if (Random.Range(0, 100) < 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy.Enemy>() != null)
        {
            collision.GetComponent<Enemy.Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
        
    }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.GetComponent<Enemy.Enemy>() != null) 
    //         collision.GetComponent<Enemy.Enemy>().FreezeTime(false);
    //         
    //
    // }
    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy.Enemy>()?.FreezeTime(false); 
    

    private void DestroyHotkey()
    {
        if(createdHotkey.Count <= 0)
            return;
        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodeList.Count<=0)
        {
            return;
        }

        if (!canCreateHotkeys)
        {
            return;
        }
        GameObject newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2),
            quaternion.identity);
        createdHotkey.Add(newHotkey);
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        Blackhole_Hotkey_Contreller newHotkeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Contreller>();
        newHotkeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransfor) => targets.Add(_enemyTransfor);
}
