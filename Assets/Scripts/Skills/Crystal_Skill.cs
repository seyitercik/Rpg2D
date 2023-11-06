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

    [Header("Multi stacking crystal")] 
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStack;
    [SerializeField] private float multiStackCooldowm;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>(); 
    

    public override void UseSkill()
    {
        base.UseSkill();
        if(CanUsemultiCrystal())
            return; 
        
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

    private bool CanUsemultiCrystal()
    {
        if (canUseMultiStack)
        {
            
            if (crystalLeft.Count > 0)
            {
                if(crystalLeft.Count == amountOfStack)
                    Invoke("ResetAbblity",useTimeWindow);
                coolDown = 0;
                GameObject crystalSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalSpawn, player.transform.position, 
                    Quaternion.identity);
                
                crystalLeft.Remove(crystalSpawn);
                
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration,moveSpeed,canExplode,
                    canMoveToEnemy,
                        FindClosestEnemy(newCrystal.transform));
                if (crystalLeft.Count <= 0)
                {
                    coolDown = multiStackCooldowm;
                    RefilCrystal();

                }
                
                return true;
            }
        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStack - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalprefab);
        }
    }

    private void ResetAbblity()
    {
        if (coolDownTimer>0)
        {
            return;
        }
        coolDownTimer = multiStackCooldowm;
        RefilCrystal();
    }
}
