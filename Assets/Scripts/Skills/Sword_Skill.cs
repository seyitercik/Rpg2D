using Skills.Skill_Controllers;
using UnityEngine;


namespace Skills
{

   public enum SwordType
   {
      Regular,
      Bounce,
      Pierce,
      Spin
   }
   
   
   
   
   public class Sword_Skill : Skill
   {
      [SerializeField] public SwordType swordType = SwordType.Regular;
      
      [Header("Bounce info")] 
      [SerializeField] private int bounceAmount;
      [SerializeField] private float bounceGravity;
      [SerializeField] private float bounceSpeed;

      [Header("Peirce info")] 
      [SerializeField] private int pierceAmount;
      [SerializeField] private float pierceGravity;

      [Header("Spin info")] 
      [SerializeField] private float maxTravelDistance = 7;
      [SerializeField] private float spinDuration = 2;
      [SerializeField] private float spinGravity=1;
      [SerializeField] private float hitCooldown=.35f;
      
      [Header("Aim Dots info")] 
      [SerializeField] private int numberOfDots;
      [SerializeField] private float spaceBeetwenDots;
      [SerializeField] private GameObject dotPrefab;
      [SerializeField] private Transform dotsParrent;
      
      [Header("Skill info")] 
      [SerializeField] private GameObject swordPrefab; 
      [SerializeField] private Vector2 launchForce;
      [SerializeField] private float swordGravity;
      [SerializeField] private float freezeTimeDuration;
      [SerializeField] private float returnSpeed;

     
      
      
      private GameObject[] dots;

      private Vector2 finalDir;

      protected override void Start()
      {
         base.Start();
         GenerateDots();
         SetupGravity();
      }

      private void SetupGravity()
      {
         if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
         else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
         else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
         
      }

      protected override void Update()
      {
         if (Input.GetKey(KeyCode.Q))
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

         if (swordType==SwordType.Bounce)
            newSwordScripts.SetupBounce(true,bounceAmount,bounceSpeed);
         
         else if (swordType==SwordType.Pierce)
            newSwordScripts.SetupPierce(pierceAmount);
         else if (swordType==SwordType.Spin)
            newSwordScripts.SetupSpip(true,maxTravelDistance,spinDuration,hitCooldown);
               
            
            
         
         
         newSwordScripts.SetupSword(finalDir,swordGravity,player,freezeTimeDuration,returnSpeed);
         player.AssingNewSword(newSword);
         DotsActive(false);
      }

      #region Aim Region

      

      

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
      #endregion
   }
}
