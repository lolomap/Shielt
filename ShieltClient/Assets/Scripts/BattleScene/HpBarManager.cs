using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleScene
{
    public class HpBarManager : MonoBehaviour
    {
        private Slider _hpBar;

        public int SqueezeSpeed = 60;
        
        private void Start()
        {
            _hpBar = GetComponent<Slider>();
        }

        public void SetHp(int currentHp)
        {
            //_hpBar.value = currentHp;
            StartCoroutine(Squeeze(currentHp));
        }

        private IEnumerator Squeeze(int value)
        {
            while (_hpBar.value - value > SqueezeSpeed * Time.deltaTime)
            {
                _hpBar.value -= SqueezeSpeed * Time.deltaTime;
                yield return null;
            }

            _hpBar.value = value;
        }
    }
}
