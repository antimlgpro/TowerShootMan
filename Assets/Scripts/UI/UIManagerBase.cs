using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManagerBase : MonoBehaviour
{
	public abstract bool LoadUIManager();

	public void LogStatus(string message) {
		// TOOD: Implement
		// if (!logging) return

		Debug.Log(message);
	}
}
