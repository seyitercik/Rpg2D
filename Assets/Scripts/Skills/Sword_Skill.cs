using UnityEngine;
using UnityEngine.Serialization;

namespace Skills
{
   public class Sword_Skill : Skill
   {
      [Header("Skill info")] 
      [SerializeField] private GameObject swordPrefab; 
      [SerializeField] private Vector2 launchForce;
      [SerializeField] private float swordGravity;

      [Header("Aim Dots info")] 
      [SerializeField] private int numberOfDots;
      [SerializeField] private float spaceBeetwenDots;
      [SerializeField] private GameObject dotPrefab;
      [SerializeField] private Transform dotsParrent;
      private GameObject[] dots;

      private Vector2 finalDir;

      protected override void Start()
      {
         base.Start();
         GenerateDots();
      }

      protected override void Update()
      {
         if (Input.GetKeyUp(KeyCode.Q))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
               AimDirection().normalized.y * launchForce.y);
         if (Input.GetKey(KeyCode.Q))
         {
            for (int i = 0; i < dots.Length; i++)
            {
               dots[i].transform.position = DotPosition(i * spaceBeetwenDots);
            }
            
         }
        
      }
      

      public void CreateSword()
      {
         GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
         Sword_Skill_Controller newSwordScripts = newSword.GetComponent<Sword_Skill_Controller>();
         newSwordScripts.SetupSword(finalDir,swordGravity);
         DotsActive(false);
      }

      public Vector2 AimDirection()
      {
         Vector2 playerPosition = player.transform.position;
         Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         Vector2 direction = mousePosition - playerPosition;
         return direction;
      }

      public void DotsActive(bool _isActive)
      {
         for (int i = 0; i < dots.Length; i++)
         {
            dots[i].SetActive(_isActive);
            
         }
      }

      private void GenerateDots()
      {
         dots = new GameObject[numberOfDots];
         for (int i = 0; i < numberOfDots; i++)
         {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParrent);
            dots[i].SetActive(false);
         }
      }

      private Vector2 DotPosition(float t)
      {
         Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * Physics2D.gravity * swordGravity * (t * t);
         return position;
      }
   }
}
