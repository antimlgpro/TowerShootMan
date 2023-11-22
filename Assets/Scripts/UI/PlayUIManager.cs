using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIManager : UIManagerBase
{
	[SerializeField] private bool active;

	[Header("References")]
	[SerializeField] private Button playButton;
	//[SerializeField] private Button fastForwardButton;


    public override bool LoadUIManager()
    {
		playButton.onClick.AddListener(OnClickPlay);
		
		// Used to toggle button active state when a wave actually starts.
		GameController.Instance.m_OnWaveStart.AddListener(OnStart);
		GameController.Instance.m_OnWaveStop.AddListener(OnStop);

		return true;
    }

	private void OnClickPlay() {
		GameController.Instance.m_OnWaveTrigger.Invoke();
	}

	void OnStart() {
		active = false;
		ToggleButton(active);
	}

	void OnStop() {
		active = true;
		ToggleButton(active);
	}

	void ToggleButton(bool value) {
		playButton.interactable = value;
	}
}
