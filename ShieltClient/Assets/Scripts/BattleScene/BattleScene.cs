using UnityEngine;

namespace BattleScene
{
   public class BattleScene : MonoBehaviour
   {
      public HpBarManager HealthBar1, HealthBar2;
      
      private void Start()
      {
         EmotionalPage.Instance.StartScanning();
         
         Network.HealthChanged += (health1, health2) =>
         {
            HealthBar1.SetHp(health1);
            HealthBar2.SetHp(health2);
         };
         
      }
   }
}
