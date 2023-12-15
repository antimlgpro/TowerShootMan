using System.Collections;
using System.Collections.Generic;
using Google.MaterialDesign.Icons;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIManager : UIManagerBase
{
	[SerializeField] private bool active;

	[Header("References")]
	[SerializeField] private Button playButton;
	[SerializeField] private Button fastForwardButton;
	[SerializeField] private MaterialIcon fastForwardMaterialIcon;

	private Color fastForwardDefault;
	[SerializeField] private Color fastForwardHighlight;
	private bool fastForwardSelected = false;


    public override bool LoadUIManager()
    {
		playButton.onClick.AddListener(OnClickPlay);
		fastForwardButton.onClick.AddListener(OnClickFastforward);

		fastForwardDefault = fastForwardMaterialIcon.color;
		
		// Used to toggle button active state when a wave actually starts.
		GameController.Instance.m_OnWaveStart.AddListener(OnStart);
		GameController.Instance.m_OnWaveStop.AddListener(OnStop);

		return true;
    }

	private void OnClickPlay() {
		GameController.Instance.m_OnWaveTrigger.Invoke();
	}

	private void OnClickFastforward() {
		GameController.Instance.ToggleFastForward();

		if (!fastForwardSelected) {
			fastForwardMaterialIcon.color = fastForwardHighlight;
		} else {
			fastForwardMaterialIcon.color = fastForwardDefault;
		}

		fastForwardSelected = !fastForwardSelected;
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
