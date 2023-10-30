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
    

        public void CreateClone(Transform _clonePosition)
        {
            GameObject newClone = Instantiate(clonePrefab);
            newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack);
        }
    }
}
