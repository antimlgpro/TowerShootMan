using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : UIManagerBase
{
	[SerializeField] private Button playButton;
	[SerializeField] private Button settingsButton;

    public override bool LoadUIManager()
    {
		playButton.onClick.AddListener(PlayButtonProxy);
		settingsButton.onClick.AddListener(SettingsButtonProxy);

		MenuController.Instance.m_ToggleMainMenu.AddListener(Toggle);

		Toggle(isVisible);

		return true;
    }



	void PlayButtonProxy() {
		MenuController.Instance.m_OnClickPlay.Invoke();
	}

	void SettingsButtonProxy() {
		MenuController.Instance.m_OnClickSettings.Invoke();
	}
}
