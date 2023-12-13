using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionUIManager : UIManagerBase
{
    public override bool LoadUIManager()
    {
		MenuController.Instance.ToggleLevelSelection.AddListener(Toggle);

		Toggle(isVisible);
        return true;
    }

}
