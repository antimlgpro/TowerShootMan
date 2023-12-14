using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : UIManagerBase
{
	[SerializeField] private Button backButton;
	[SerializeField] private Button restartButton;
	[SerializeField] private Button continueButton;

	
    public override bool LoadUIManager()
    {
		Toggle(isVisible);

		backButton.onClick.AddListener(OnBack);
		restartButton.onClick.AddListener(OnRestart);
		continueButton.onClick.AddListener(OnContinue);

		GameController.Instance.m_OnGamePause.AddListener(OpenSettings);

        return true;
    }

	private void OpenSettings() {
		Toggle(true);
	}


	private void OnBack() {
		GameController.Instance.LoadLevel(GameController.Instance.mainMenuScene);
		GameController.Instance.m_OnGameStart.Invoke();
	}
	
	// TODO: Add restarts.
	private void OnRestart() {
		Debug.Log("You cant restart yet");
	}

	private void OnContinue() {
		Toggle(false);
	}
}
