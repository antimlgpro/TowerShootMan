using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionUIManager : UIManagerBase
{
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;
	[SerializeField] private Button selectButton;
	[SerializeField] private Button backButton;

	[SerializeField] private TextMeshProUGUI titleText;


    public override bool LoadUIManager()
    {
		MenuController.Instance.m_ToggleLevelSelection.AddListener(Toggle);
		MenuController.Instance.m_LevelSwapResult.AddListener(UpdateTitle);
		leftButton.onClick.AddListener(() => MenuController.Instance.m_OnLevelSwap.Invoke(true));
		rightButton.onClick.AddListener(() => MenuController.Instance.m_OnLevelSwap.Invoke(false));
		rightButton.onClick.AddListener(() => MenuController.Instance.m_OnLevelSwap.Invoke(false));
		backButton.onClick.AddListener(() => MenuController.Instance.m_FlipCamera.Invoke());

		selectButton.onClick.AddListener(() => MenuController.Instance.m_OnClickSelectLevel.Invoke());


		Toggle(isVisible);
        return true;
    }

	private void UpdateTitle(string value) {
		titleText.text = value;
	}

}
