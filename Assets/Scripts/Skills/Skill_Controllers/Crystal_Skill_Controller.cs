using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    public void SetupCrystal(float _crytalDuration)
    {
        crystalExistTimer = _crytalDuration;

    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer <= 0)
        {
            SelfDestroy();
            
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
