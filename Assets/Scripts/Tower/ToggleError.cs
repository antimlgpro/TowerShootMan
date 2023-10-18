using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleError : MonoBehaviour
{
	public bool Error {
		set {
			error = value;
			OnErrorChange();
		}
	}

	private bool error;

	[SerializeField] private MeshRenderer[] towerRenderer;

    private void OnErrorChange() {
		foreach (MeshRenderer renderer in towerRenderer) {
			renderer.material.SetFloat("_Error", error ? 1f : 0f);
		}
	}
}
