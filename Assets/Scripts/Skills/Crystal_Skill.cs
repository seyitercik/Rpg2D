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

    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal==null)
        {
            currentCrystal = Instantiate(crystalprefab,player.transform.position,quaternion.identity);
            currentCrystalController = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            currentCrystalController.SetupCrystal(crystalDuration);
        }
        else
        {
            player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }
    }
}
