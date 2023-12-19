using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallax : MonoBehaviour
{
	[SerializeField] private Transform cameraChild;
	private Vector3 startPosition;
	private Camera cameraRef;
	[SerializeField] private float moveMultiplier;

	[SerializeField] private bool isActive;

    void Start()
    {
        startPosition = cameraChild.position;
		cameraRef = cameraChild.GetComponent<Camera>();
    
		MenuController.Instance.m_OnCameraFlip.AddListener(ToggleActive);
	}

	void ToggleActive() {
		isActive = MenuController.Instance.m_isCameraFlipped;
	}

    void Update()
    {
		if (isActive == false) return;

		Vector3 mousePos = cameraRef.ScreenToViewportPoint(Input.mousePosition);

		cameraChild.position = Vector3.Lerp(cameraChild.position, startPosition + (mousePos * moveMultiplier), 2f * Time.deltaTime);
    }
}
