using System;
using TMPro;
using UnityEngine;

namespace Skills.Skill_Controllers
{
    public class Blackhole_Hotkey_Contreller : MonoBehaviour
    {
        private SpriteRenderer sr;
        private KeyCode myHotkey;
        private TextMeshProUGUI myText;

        private Transform myEnemy;
        private BlackHole_Skill_Controller blackhole;

        public void SetupHotkey(KeyCode _myNewHotkey,Transform _myEnemy,BlackHole_Skill_Controller _myBlackhole)
        {
            sr=GetComponent<SpriteRenderer>();
            myText = GetComponentInChildren<TextMeshProUGUI>();
            myEnemy = _myEnemy;
            blackhole = _myBlackhole;
            
            myHotkey = _myNewHotkey;
            myText.text = _myNewHotkey.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(myHotkey))
            {
                blackhole.AddEnemyToList(myEnemy);
                myText.color = Color.clear;
                sr.color=Color.clear;
            }
        }
    }
}