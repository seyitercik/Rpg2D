using UnityEngine;

namespace Skills
{
   public class Sword_Skill : Skill
   {
      [Header("Skill info")] 
      [SerializeField] private GameObject swordPrefab;
      [SerializeField] private Vector2 launchDir;
      [SerializeField] private float swordGravity;

      public void CreateSword()
      {
         GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
         Sword_Skill_Controller newSwordScripts = newSword.GetComponent<Sword_Skill_Controller>();
         newSwordScripts.SetupSword(launchDir,swordGravity);
      }
   }
}
