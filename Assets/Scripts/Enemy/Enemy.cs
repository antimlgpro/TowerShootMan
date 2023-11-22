using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private bool dead = false;
	public bool Dead => dead;

	public EnemyMover m_Mover;
	public EnemyObject m_EnemyObject;
	public EnemySpawner m_Spawner;

	[SerializeField]
	private float health;

	[SerializeField]
	private float speed;

	[SerializeField]
	private int armorTier;

	private int currentArmorLayer;
	private float currentArmorHealth;

	public void Initialize() {
		m_Mover.Initialize(m_EnemyObject, m_Spawner);
		health = m_EnemyObject.baseHealth;
		speed = m_EnemyObject.speed;
		armorTier = m_EnemyObject.armorTier;
		currentArmorLayer = armorTier;
	}

	float CalculateArmorHealth(int x) {
		return x * Mathf.Log(x);
	}

	public void TakeDamage(float damage)
	{
		if (dead) return;
		if (currentArmorLayer >= 1) {
			float plateHealth = CalculateArmorHealth(armorTier) * health;

			currentArmorHealth = currentArmorLayer * (plateHealth / health);
			currentArmorHealth -= damage;

			// Armor layer was broken
			if (currentArmorHealth <= 0) {
				currentArmorLayer -= 1;
			}
		} else {
			health -= damage;

			if (health <= 0)
			{
				Die();
			}
		}
	}

	public void Die()
	{
		m_Mover.Stop();
		dead = true;
		gameObject.SetActive(false);
		m_Spawner.OnEnemyKilled.Invoke(m_EnemyObject);
	}


	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Projectile")) {
			TakeDamage(other.gameObject.GetComponent<Projectile>().Damage);
			Destroy(other.gameObject);
		}
	}
}
