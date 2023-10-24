
using System;
using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
   private SpriteRenderer sr;
   [Header("Flash Fx")] 
   [SerializeField] private float flashDuration=0.2f;
   [SerializeField] private Material hitMaterial;
    private Material orijinalMaterial;

   private void Start()
   {
      sr = GetComponentInChildren<SpriteRenderer>();
      orijinalMaterial = sr.material;
   }

   private IEnumerator FlashFx()
   {
       sr.material = hitMaterial;
       yield return new WaitForSeconds(flashDuration);
       sr.material = orijinalMaterial;
   }
}
