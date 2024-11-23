using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleScene
{
    public class HpBarManager : MonoBehaviour
    {
        [SerializeField] private Slider _hpBar;
        public void SetHp(int currentHp)
        {
            _hpBar.value = currentHp;
        }
    }
}
