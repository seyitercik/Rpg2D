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
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public int amountOfAttack=4;
    public float cloneAttackCoolDown=.3f;
    private float cloneAttackTimer;
    private bool canAttack;
    
    private List<Transform> targets = new List<Transform>();
    
    


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.R))
            canAttack = true;
        
        if (cloneAttackTimer < 0 && canAttack)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;
            if (Random.Range(0,100) < 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }
            SkillManager.instance.clone.CreateClone(targets[randomIndex],new Vector3(xOffset,0));
            amountOfAttack--;
            if (amountOfAttack <=0 )
            {
                canAttack = false;
            }
        }
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);
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

    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodeList.Count<=0)
        {
            return;
        }
        GameObject newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2),
            quaternion.identity);
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);
        Blackhole_Hotkey_Contreller newHotkeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Contreller>();
        newHotkeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransfor) => targets.Add(_enemyTransfor);
}
