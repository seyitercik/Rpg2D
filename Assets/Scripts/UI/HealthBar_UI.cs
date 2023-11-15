using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar_UI : MonoBehaviour
    {
        private CharacterStats myStats;
        private Entity entity;
        private RectTransform myTransform;
        private Slider slider;

        private void Start()
        {
            myTransform = GetComponent<RectTransform>();
            entity = GetComponentInParent<Entity>();
            slider = GetComponentInChildren<Slider>();
            myStats = GetComponentInParent<CharacterStats>();
           
           
            entity.onFlipped += FlipUI;
            myStats.onHealthChanged += UpdateToHealtUI;
            UpdateToHealtUI();
        }

       

        private void UpdateToHealtUI()
        {
            slider.maxValue = myStats.GetMaxHealthValue();
            slider.value = myStats.currentHealth;
        }

        private void FlipUI() => myTransform.Rotate(0,180,0);

        private void OnDisable()
        {
            entity.onFlipped -= FlipUI;
            myStats.onHealthChanged -= UpdateToHealtUI;
        } 
        
    }
}