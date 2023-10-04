using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float timeUntilDeath;
	private float timeElapsed;


	private float damage;
	public float Damage => damage;

	public void Initialize(float _damage) {
		damage = _damage;
	}

	void Update() {
		timeElapsed += Time.deltaTime;

		if (timeElapsed >= timeUntilDeath) {
			Destroy(gameObject);
		}
	}
}
