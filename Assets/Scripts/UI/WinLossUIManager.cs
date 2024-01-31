using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLossUIManager : UIManagerBase
{
	[SerializeField] private Button backButton;
	[SerializeField] private Button restartButton;
	[SerializeField] private TextMeshProUGUI winLossText;

	[SerializeField] private string winText;
	[SerializeField] private string lossText;


	public override bool LoadUIManager()
	{
		Toggle(isVisible);

		backButton.onClick.AddListener(OnBack);
		restartButton.onClick.AddListener(OnRestart);

		GameController.Instance.m_OnWinGame.AddListener(OpenWin);
		GameController.Instance.m_OnLoseGame.AddListener(OpenLoss);

		return true;
	}

	private void OpenWin() {
		Toggle(true);
		winLossText.text = winText;
	}

	private void OpenLoss() {
		Toggle(true);
		winLossText.text = lossText;
	}


	private void OnBack() {
		GameController.Instance.LoadLevel(GameController.Instance.mainMenuScene);
		GameController.Instance.m_OnGameStart.Invoke();
	}
	
	// TODO: Add restarts.
	private void OnRestart() {
		Debug.Log("You cant restart yet");
	}

}
