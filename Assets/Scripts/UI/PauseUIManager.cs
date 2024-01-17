using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : UIManagerBase
{
	[SerializeField] private Button backButton;
	[SerializeField] private Button restartButton;
	[SerializeField] private Button continueButton;
	[SerializeField] private Swatch autoStartSwatch;
	[SerializeField] private Swatch	buildModeSwatch;



    public override bool LoadUIManager()
    {
		Toggle(isVisible);

		backButton.onClick.AddListener(OnBack);
		restartButton.onClick.AddListener(OnRestart);
		continueButton.onClick.AddListener(OnContinue);
		autoStartSwatch.OnValueChange.AddListener(OnAutoStart);
		buildModeSwatch.OnValueChange.AddListener(OnBuildMode);


		GameController.Instance.m_OnGamePause.AddListener(OpenSettings);
		GameController.Instance.m_OnPreferencesLoad.AddListener(OnLoadPreferences);

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

	private void OnAutoStart(bool value) {
		GameController.Instance.Preferences.AutoStart = value;
	}

	private void OnBuildMode(bool value) {
		GameController.Instance.Preferences.DisableBuildModeOnBuild = value;
	}

	private void OnLoadPreferences(GameController.GameSettings prefs) {
		autoStartSwatch.ChangeValue(prefs.AutoStart);
		buildModeSwatch.ChangeValue(prefs.DisableBuildModeOnBuild);
	}
}
