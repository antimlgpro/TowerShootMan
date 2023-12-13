using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUIManager : UIManagerBase
{
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;

    public override bool LoadUIManager()
    {
		MenuController.Instance.ToggleLevelSelection.AddListener(Toggle);
		leftButton.onClick.AddListener(() => MenuController.Instance.OnLevelSwap.Invoke(true));
		rightButton.onClick.AddListener(() => MenuController.Instance.OnLevelSwap.Invoke(false));

		Toggle(isVisible);
        return true;
    }

}
