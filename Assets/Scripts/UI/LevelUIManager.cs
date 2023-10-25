using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelUIManager : MonoBehaviour
{
	public class UILoadingException : Exception
	{
		public UILoadingException() { }
	}


	public List<UIManagerBase> childUIManagers;


	// This class will contain all UI Logic inside levels

    void Start()
    {
		foreach (var manager in childUIManagers) {
			bool status = manager.LoadUIManager();

			if (!status) throw new UILoadingException();
		}
    }
}
