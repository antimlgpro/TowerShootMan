using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSphere : MonoBehaviour
{
	public bool active;
	[SerializeField] private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
		SetActive(false);
    }

	public void SetActive(bool value) {
		active = value;

		meshRenderer.enabled = active;
	}
	
	// A sphere in unity has radius 0.5 so value needs to be doubled.
	public void SetRadius(float value) {
		transform.localScale = value * 2f * Vector3.one;
	}
}
