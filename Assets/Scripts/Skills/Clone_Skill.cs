using System.Collections;
using Skills.Skill_Controllers;
using UnityEngine;

namespace Skills
{
    public class Clone_Skill : Skill
    {
        [Header("Clone info")]
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private float cloneDuration;
        [Space]
        [SerializeField] private bool canAttack;

        [SerializeField] private bool createCloneOnDashStart;
        [SerializeField] private bool createCloneOnDashOver;
        [SerializeField] private bool canCreateCloneOnCounterAttack;
    

        public void CreateClone(Transform _clonePosition,Vector3 _offset)
        {
            GameObject newClone = Instantiate(clonePrefab);
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform));
        }

        public void CreateCloneOnDashStart()
        {
            if(createCloneOnDashStart)
                CreateClone(player.transform,Vector3.zero);
        }
        public void CreateCloneOnDashOver()
        {
            if(createCloneOnDashOver)
                CreateClone(player.transform,Vector3.zero);
        }

        public void CreateCloneOnCounterAttack(Transform _enemyTransform)
        {
            if (canCreateCloneOnCounterAttack)
            {
                StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));

            }
        }

        private IEnumerator CreateCloneWithDelay(Transform _enemyTransform,Vector3 _offset)
        {
            yield return new WaitForSeconds(0.4f);
            CreateClone(_enemyTransform, _offset);
        }
        
        
    }
}
