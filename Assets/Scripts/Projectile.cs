using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float timeUntilDeath;

	private float timeElapsed;


	void Update() {
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= timeUntilDeath) {
			Destroy(gameObject);
		}
	}
}
