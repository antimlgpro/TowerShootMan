using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManagerBase : MonoBehaviour
{
	[Header("Hide UIManager")]
	[SerializeField] protected bool isVisible;
	[SerializeField] protected List<GameObject> objectsToHide;

	public abstract bool LoadUIManager();

	public void LogStatus(string message) {
		// TOOD: Implement
		// if (!logging) return

		Debug.Log(message);
	}

	public void Toggle(bool value) {
		isVisible = value;

		foreach (GameObject toHide in objectsToHide) {
			toHide.SetActive(isVisible);
		}
	}
}
