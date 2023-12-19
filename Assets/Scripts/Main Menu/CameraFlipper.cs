using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour
{
	private bool isFlipping;
	private bool isFlipped;
	public bool IsFlipped => isFlipped;

	private float elapsedTime = 0f;
	[SerializeField, Range(0f, 100f)] private float timeToFlip = 1f;

    // Start is called before the first frame update
    void Start()
    {
		isFlipped = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		elapsedTime = 0f;

		MenuController.Instance.m_FlipCamera.AddListener(Flip);
    }

	float Elastic(float t) {
		return 1 - Mathf.Pow(1 - t, 4);
	}

	public void Flip() {
		if (isFlipping) return;

		MenuController.Instance.m_OnCameraFlip.Invoke();
		StartCoroutine(FlipCoroutine());
	}

	IEnumerator FlipCoroutine() {
		isFlipping = true;

		while (elapsedTime < timeToFlip) {
			if (!isFlipped) {
				transform.rotation = Quaternion.Slerp(
					Quaternion.Euler(0, 0, 0), 
					Quaternion.Euler(0, 180, 0), 
					Elastic(elapsedTime / timeToFlip)
				);
			} else {
				transform.rotation = Quaternion.Slerp(
					Quaternion.Euler(0, -180, 0), 
					Quaternion.Euler(0, 360, 0),
					Elastic(elapsedTime / timeToFlip)
				);
			}

			elapsedTime += Time.deltaTime;
      		yield return new WaitForEndOfFrame ();
    	}

		if(elapsedTime >= timeToFlip) {
			if (!isFlipped) {
				transform.rotation = Quaternion.Euler(0, -180, 0);
			} else {
				transform.rotation = Quaternion.Euler(0, 0, 0);
			}
			isFlipped = !isFlipped;
			elapsedTime = 0f;
			isFlipping = false;
		}
	}
}
