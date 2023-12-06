using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSphere : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;

	public void SetActive(bool value) {
		meshRenderer.enabled = value;
	}
	
	// A sphere in unity has radius 0.5 so value needs to be doubled.
	public void SetRadius(float value) {
		transform.localScale = value * 2f * Vector3.one;
	}
}
