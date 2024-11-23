using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleScene
{
    public class HpBarManager : MonoBehaviour
    {
        private Slider _hpBar;
        
        private void Start()
        {
            _hpBar = GetComponent<Slider>();
        }

        public void SetHp(int currentHp)
        {
            _hpBar.value = currentHp;
        }
    }
}
