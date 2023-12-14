using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIManager : UIManagerBase
{
    public override bool LoadUIManager()
    {
        Toggle(isVisible);
		return true;
    }
}
