using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public List<Transform> targets;
    


    private void Update()
    {
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
            //respawn prefab of hotkey above enemy
           // targets.Add(collision.transform);
        }
        
    }
}
