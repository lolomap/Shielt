using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleScene
{
	public class BattleScene : MonoBehaviour
	{
		public HpBarManager HealthBar1, HealthBar2;
		public PlayerScript Player1, Player2;
		public TMP_Text WinnerText;
		public GameObject EndRoundPanel;
		private void Start()
		{
            Network.Instance.Connect();
			EmotionalPage.Instance.StartScanning();

			Network.UpdatePlayers += (players) =>
			{
				HealthBar1.SetHp(players.Player1Health);
				HealthBar2.SetHp(players.Player2Health);

                String Winner = "";
                bool isFinish = false;
				if (players.Player1Health <= 0)
				{
					Player1.Die();
					Winner = "Player 2";
					isFinish = true;
				}
				if (players.Player2Health <= 0)
				{
					Player2.Die();
					if (Winner == "Player 2")
						Winner = "Draw";
					else
						Winner = "Player 1";
					isFinish = true;
				}

				if (isFinish)
				{
					DisplayWinner(Winner);
					EndRoundPanel.SetActive(true);
				}

				if (players.Player1IsDefend)
					Player1.Defend();
				else Player1.Attack();
				if (players.Player2IsDefend)
					Player2.Defend();
				else
					Player2.Attack();
			};

		}

		public void DisplayWinner(String winner)
		{
			WinnerText.text = $"The winner is: + \n + {winner}!";
		}

		public void ExitGame()
		{
			Application.Quit();
		}

		public void ResetBattle()
		{
			HealthBar1.SetHp(100);
            HealthBar1.SetHp(100);
			Player1.Idle();
			Player2.Idle();
			EndRoundPanel.SetActive(false);
			//Special server package
        }
	}
}
