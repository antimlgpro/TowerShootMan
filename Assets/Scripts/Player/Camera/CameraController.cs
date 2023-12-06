using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Camera cameraRef;
	[SerializeField] private Transform focusObject;
	[SerializeField] private LayerMask layerMask;
	private Vector3 center;


	[Range(1, 500f), SerializeField] private float minDistance = 130f;
	[Range(1, 1000f), SerializeField] private float maxDistance = 500f;
	[SerializeField] private float rotationSpeed = 90f;
	[SerializeField] private float zoomSpeed = 30f;

	[SerializeField, Range(-89f, 89f)]
	float minVerticalAngle = -30f, maxVerticalAngle = 60f;

	private float distance = 5f;
	private bool rotateCamera = false;
	[SerializeField] private Vector2 orbitAngles = new(0f, 0f);

    void Start()
    {
        center = focusObject.position;
		transform.rotation = Quaternion.Euler(orbitAngles);

		distance = Vector3.Distance(transform.position, center);
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}

		if (Input.GetKeyDown(KeyCode.LeftControl)) {
			Cursor.lockState = CursorLockMode.Locked;
			rotateCamera = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			Cursor.lockState = CursorLockMode.None;
			rotateCamera = false;
		}


		distance = Vector3.Distance(transform.position, center);
		float scrollDelta = Input.mouseScrollDelta.y;
		float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);

		var canRotate = (normalizedDistance >= 0 && scrollDelta > 0) || (normalizedDistance <= 1 && scrollDelta < 0);

		if (canRotate) {
			transform.position = Vector3.MoveTowards(
				transform.position, 
				center, 
				scrollDelta * (zoomSpeed * Mathf.Max(normalizedDistance, 0.05f))
			);
		}


		float _distance = CollideWithIsland() + 2f;
		distance = Mathf.Max(Vector3.Distance(transform.position, center), _distance);

		Quaternion lookRotation;
		if (rotateCamera) {
			if (ManualRotation()) {
				ConstrainAngles();
				lookRotation = Quaternion.Euler(orbitAngles);
			}
			else {
				lookRotation = transform.rotation;
			}
		} else {
			lookRotation = transform.rotation;
		}

		Vector3 lookDirection = lookRotation * Vector3.forward;
		Vector3 lookPosition = center - lookDirection * distance;
		transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

	bool ManualRotation() {
		Vector2 input = new(
			-Input.GetAxis("Mouse Y"),
			Input.GetAxis("Mouse X")
		);

		const float e = 0.001f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e) {
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
			return true;
		}
		return false;
	}

	void ConstrainAngles() {
		orbitAngles.x =
			Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

		if (orbitAngles.y < 0f) {
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 360f) {
			orbitAngles.y -= 360f;
		}
	}

	float CollideWithIsland() {
		Physics.Raycast(transform.position - transform.forward * 10f, transform.forward, out RaycastHit hit, 14f, layerMask);
		Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);

		return Vector3.Distance(hit.point, center);
	}
}
