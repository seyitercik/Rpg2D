using System.Collections;
using System.Collections.Generic;
using Skills;
using Unity.Mathematics;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalprefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;
    private Crystal_Skill_Controller currentCrystalController;

    [Header("Explosive crystal")] 
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")] 
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal==null)
        {
            currentCrystal = Instantiate(crystalprefab,player.transform.position,quaternion.identity);
            currentCrystalController = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalController.SetupCrystal(crystalDuration,moveSpeed,canExplode,canMoveToEnemy,FindClosestEnemy(currentCrystal.transform));
        }
        else
        {
            if(canMoveToEnemy)
                return;
            
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            currentCrystalController.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }
    }
}
