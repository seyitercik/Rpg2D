using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
   

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
         PlayerStats  playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
      
        if (collision.GetComponent<Enemy.Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
