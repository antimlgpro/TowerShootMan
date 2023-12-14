using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUIManager : UIManagerBase
{
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;

	[SerializeField] private Button selectButton;

    public override bool LoadUIManager()
    {
		MenuController.Instance.m_ToggleLevelSelection.AddListener(Toggle);
		leftButton.onClick.AddListener(() => MenuController.Instance.m_OnLevelSwap.Invoke(true));
		rightButton.onClick.AddListener(() => MenuController.Instance.m_OnLevelSwap.Invoke(false));

		selectButton.onClick.AddListener(() => MenuController.Instance.m_OnClickSelectLevel.Invoke());

		Toggle(isVisible);
        return true;
    }

}
