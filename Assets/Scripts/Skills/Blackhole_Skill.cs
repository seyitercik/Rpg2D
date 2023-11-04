using Unity.Mathematics;
using UnityEngine;

namespace Skills
{
    public class Blackhole_Skill : Skill
    {
        [SerializeField] private int amountOfAttack;
        [SerializeField] private float cloneCooldown;
        [Space] [SerializeField] private GameObject blackholePrefabs;
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;

        [SerializeField] private float blackholeDuration;
        private BlackHole_Skill_Controller currentBlackhole;

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
            GameObject newBlackHole = Instantiate(blackholePrefabs, player.transform.position, quaternion.identity);
            currentBlackhole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
            currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneCooldown,blackholeDuration);
        }

        public bool SkillComleted()
        {
            if (!currentBlackhole)
            {
                return false;

            }

            if (currentBlackhole.playerCanExitState)
            {
                currentBlackhole = null;
                return true;

            }

            return false;
        }
    }
}