using UnityEngine;

namespace BattleScene
{
   public class BattleScene : MonoBehaviour
   {
      public HpBarManager HealthBar1, HealthBar2;
      public PlayerScript Player1, Player2;
      
      private void Start()
      {
         Network.Instance.Connect();
         EmotionalPage.Instance.StartScanning();
         
         Network.UpdatePlayers += (players) =>
         {
            HealthBar1.SetHp(players.Player1Health);
            HealthBar2.SetHp(players.Player2Health);

            bool isFinish = false;
            if (players.Player1Health <= 0)
            {
               Player1.Die();
               isFinish = true;
            }
            if (players.Player2Health <= 0)
            {
               Player2.Die();
               isFinish = true;
            }

            if (isFinish)
            {
               // MODAL
            }
            
            if (players.Player1IsDefend)
                 Player1.Defend(); 
             else Player1.Attack();
            if (players.Player2IsDefend)
                 Player2.Defend(); 
             else Player2.Attack();
         };
         
      }
   }
}
