using Unity.Mathematics;
using UnityEngine;

namespace Skills
{
    public class Blackhole_Skill : Skill
    {
        [SerializeField] private int amountOfAttack;
        [SerializeField] private float cloneCooldown;
        [Space]
        [SerializeField] private GameObject blackholePrefabs;
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void UseSkill()
        {
            base.UseSkill();
            GameObject newBlackHole = Instantiate(blackholePrefabs,player.transform.position,quaternion.identity);
            BlackHole_Skill_Controller newBlackholeScripts = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
            newBlackholeScripts.SetupBlackhole(maxSize,growSpeed,shrinkSpeed,amountOfAttack,cloneCooldown);
        }
    }
}